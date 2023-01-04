using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorScript : MonoBehaviour
{
    [SerializeField]
    private EggScript eggScript;

    [SerializeField]
    private float maxNo;


    private void Update()
    {
        OpenDoor();
    }

    void OpenDoor()
    {
        if (eggScript.GetNoEgg() == maxNo)
        {
            Debug.Log("Done");
            Destroy(gameObject);
        }
    }
}
