using System.Collections;
using UnityEngine;
using Unity.EditorCoroutines.Editor;
using UnityEditor;
using UnityEditor.SceneManagement;
using System.IO;
using System.Collections.Generic;
using System;

namespace IntenseNation.EditorAutoSave
{
    [InitializeOnLoad]
    public class EditorAutoSave : EditorWindow
    {
        float SaveTime = 5;
        public string[] DebugOptions = new string[] { "Full", "Necessary", "None" };
        public int SelectedDebugOptions = 1;
        public bool Autosave = true;
        public bool BackUp = false;
        public bool SavePrompt = false;
        public bool CountDown = true;
        public bool SaveNotification = true;
        public bool VersionControl = false;
        public bool VersionControlLimitState = false;
        int CountDownTime = 5;
        int VersionControlLimit = 5;

        EditorCoroutine coroutine;

        [MenuItem("Tools/Editor Auto Save")]
        public static void OpenWindow()
        {
            GetWindow<EditorAutoSave>("Editor Auto Save");
        }

        void OnEnable()
        {
            SaveTime = EditorPrefs.GetFloat("SaveTime", SaveTime);
            CountDownTime = EditorPrefs.GetInt("CountDownTime", CountDownTime);
            VersionControlLimit = EditorPrefs.GetInt("VersionControlLimit", VersionControlLimit);
            Autosave = EditorPrefs.GetBool("Autosave", Autosave);
            BackUp = EditorPrefs.GetBool("BackUp", BackUp);
            VersionControl = EditorPrefs.GetBool("VersionControl", VersionControl);
            SavePrompt = EditorPrefs.GetBool("SavePrompt", SavePrompt);
            CountDown = EditorPrefs.GetBool("Countdown", CountDown);
            SaveNotification = EditorPrefs.GetBool("SaveNotification", SaveNotification);
            VersionControlLimitState = EditorPrefs.GetBool("VersionControlLimitState", VersionControlLimitState);

            coroutine = EditorCoroutineUtility.StartCoroutineOwnerless(AutoSaveWait());
        }

        void OnGUI()
        {
            //Title
            GUILayout.Label("Editor Auto Save", EditorStyles.boldLabel);
            EditorGUILayout.Space(8);

            //Main
            GUILayout.Label("Main", EditorStyles.boldLabel);
            Autosave = EditorGUILayout.Toggle("Enable Auto Save", Autosave);
            SaveTime = EditorGUILayout.Slider("Save Time (In Minutes)", SaveTime, 0.1f, 120);
            EditorGUILayout.Space(10);

            //Properties
            GUILayout.Label("Properties", EditorStyles.boldLabel);
            SavePrompt = EditorGUILayout.Toggle("Display Save Prompt", SavePrompt);
            EditorGUILayout.Space(10);

            //Version Control
            GUILayout.Label("Version Control", EditorStyles.boldLabel);
            BackUp = EditorGUILayout.Toggle("Backup Before Saving", BackUp);
            VersionControl = EditorGUILayout.Toggle("Enable Version Control", VersionControl);
            if (VersionControl)
            {
                VersionControlLimitState = EditorGUILayout.Toggle("Enable Version Control Limit", VersionControlLimitState);
                if (VersionControlLimitState)
                { VersionControlLimit = EditorGUILayout.IntSlider("Version Control Limit", VersionControlLimit, 2, 100); }
            }
            EditorGUILayout.Space(10);

            //Notifications
            GUILayout.Label("Notifications", EditorStyles.boldLabel);
            CountDown = EditorGUILayout.Toggle("Countdown", CountDown);

            if (CountDown)
            { CountDownTime = EditorGUILayout.IntSlider("Countdown Time (In Seconds)", CountDownTime, 3, 20); }
            SaveNotification = EditorGUILayout.Toggle("Save Notification", SaveNotification);
            EditorGUILayout.Space(10);

            //Debugging
            GUILayout.Label("Debugging", EditorStyles.boldLabel);
            SelectedDebugOptions = EditorGUILayout.Popup("Debug Messages", SelectedDebugOptions, DebugOptions);
            EditorGUILayout.Space(15);

            //Apply Button
            if (GUILayout.Button("Apply"))
            {
                EditorPrefs.SetFloat("SaveTime", SaveTime);
                EditorPrefs.SetInt("CountDownTime", CountDownTime);
                EditorPrefs.SetInt("VersionControlLimit", VersionControlLimit);
                EditorPrefs.SetBool("Autosave", Autosave);
                EditorPrefs.SetBool("BackUp", BackUp);
                EditorPrefs.SetBool("VersionControl", VersionControl);
                EditorPrefs.SetBool("SavePrompt", SavePrompt);
                EditorPrefs.SetBool("CountDown", CountDown);
                EditorPrefs.SetBool("SaveNotification", SaveNotification);
                EditorPrefs.SetBool("VersionControlLimitState", VersionControlLimitState);

                EditorCoroutineUtility.StopCoroutine(coroutine);
                coroutine = EditorCoroutineUtility.StartCoroutineOwnerless(AutoSaveWait());

                ShowNotification(new GUIContent("Applied new values successfully!"));

                if (SelectedDebugOptions <= 1)
                    Debug.Log("Applied new values successfully!");
            }
        }

        float remainingSaveTime = 0f;
        float frameTime = 0;
        private IEnumerator AutoSaveWait()
        {
            if (Autosave)
            {
                while (true)
                {
                    yield return new WaitForSecondsRealtime(1);
                    frameTime++;
                    remainingSaveTime = SaveTime * 60 - frameTime;

                    if (remainingSaveTime <= 0)
                    {
                        frameTime = 0;
                        Save();
                    }
                    else if (remainingSaveTime <= CountDownTime && remainingSaveTime > 0 && CountDown)
                    {
                        foreach (SceneView scene in SceneView.sceneViews)
                        { scene.ShowNotification(new GUIContent("Auto Save in " + (int)(remainingSaveTime))); }
                    }
                }
            }
        }

        public void Save()
        {
            if (!EditorApplication.isPlaying)
            {
                bool saveState = true;

                if (BackUp)
                {
                    string activePath = EditorSceneManager.GetActiveScene().path;
                    string[] path = activePath.Split(char.Parse("/"));
                    path[path.Length - 1] = "Backup/AutoSaveBackup_" + path[path.Length - 1];
                    string folderPath = "";

                    for (int i = 0; i < path.Length - 1; i++)
                    {
                        folderPath += path[i];

                        if (i < path.Length - 2)
                            folderPath += "/";
                    }

                    if (!AssetDatabase.IsValidFolder(folderPath + "/Backup"))
                    {
                        AssetDatabase.CreateFolder(folderPath, "Backup");

                        if (SelectedDebugOptions == 0)
                            Debug.Log("Backup folder didn't exist, created a new one");

                        if (SelectedDebugOptions == 1)
                            Debug.Log(folderPath);
                    }

                    if (VersionControl)
                    {
                        string[] locatedFiles = Directory.GetFiles(folderPath + "/Backup");
                        List<string> savedScenes = new List<string>();
                        List<int> savedScenesNumbers = new List<int>();
                        string sceneName = "";

                        for (int i = 0; i < locatedFiles.Length; i++)
                        {
                            string[] filePaths = activePath.Split(char.Parse("/"));
                            sceneName = filePaths[filePaths.Length - 1].Replace("Backup/", "").Replace(".unity", "");

                            if (!locatedFiles[i].Contains(".meta") && locatedFiles[i].Contains(sceneName))
                            { savedScenes.Add(locatedFiles[i]); }
                        }

                        for (int i = 0; i < savedScenes.Count; i++)
                        {
                            string[] fileNumbers = savedScenes[i].Split(char.Parse("_"));
                            string fileNumber = fileNumbers[fileNumbers.Length - 1].Replace(".unity", "");
                            savedScenesNumbers.Add(fileNumber == sceneName && i == 0 ? 0 : int.Parse(fileNumber));
                        }

                        savedScenesNumbers.Sort();
                        int scenesNumber = savedScenesNumbers.Count > 0 ? savedScenesNumbers[savedScenesNumbers.Count - 1] : 0;

                        if (VersionControlLimitState)
                        {
                            if (scenesNumber > savedScenes.Count || scenesNumber >= VersionControlLimit)
                            {
                                if (scenesNumber > 1)
                                {
                                    for (int i = 0; i < savedScenes.Count; i++)
                                    {
                                        string[] scenePath = savedScenes[i].Split(char.Parse(@"\"));
                                        scenePath[scenePath.Length - 1] = scenePath[scenePath.Length - 1].Replace(savedScenesNumbers[i].ToString(), (i + 1).ToString());
                                        AssetDatabase.RenameAsset(savedScenes[i], scenePath[scenePath.Length - 1]);
                                    }

                                    scenesNumber = savedScenes.Count;
                                }

                            }

                            if (savedScenes.Count > VersionControlLimit)
                            {
                                for (int i = 0; i < savedScenes.Count - VersionControlLimit; i++)
                                {
                                    AssetDatabase.DeleteAsset(savedScenes[i]);
                                    savedScenes.Remove(savedScenes[i]);
                                }
                            }
                        }

                        if (savedScenes.Count > 0)
                        { path[path.Length - 1] = "Backup/AutoSaveBackup_" + activePath.Split(char.Parse("/"))[path.Length - 1].Replace(".unity", "_") + (scenesNumber + (!VersionControlLimitState ? 1 : savedScenes.Count >= VersionControlLimit ? 0 : 1)) + ".unity"; }
                    }

                    AssetDatabase.CopyAsset(activePath, string.Join("/", path));

                    if (SavePrompt)
                    { saveState = EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo(); }

                    if (saveState)
                    { EditorSceneManager.SaveScene(EditorSceneManager.GetActiveScene(), activePath); }
                }
                else
                {
                    EditorSceneManager.SaveOpenScenes();
                }

                if (saveState && SaveNotification)
                {
                    foreach (SceneView scene in SceneView.sceneViews)
                    { scene.ShowNotification(new GUIContent("Scene Saved Successfully!")); }
                }

                if (SelectedDebugOptions == 0)
                    Debug.Log("Saved Open Scene");
            }
        }
    }
}