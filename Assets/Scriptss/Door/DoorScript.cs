using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorScript : MonoBehaviour
{
    
    private EggScript eggScript;

    [SerializeField]
    private float maxNo;

    private void Awake()
    {
        eggScript = GameObject.FindWithTag("Player").GetComponent<EggScript>();
    }

    private void Update()
    {
        OpenDoor();
    }

    void OpenDoor()
    {
        if (eggScript.GetNoEgg() == maxNo)
        {
            Debug.Log("Done"+ eggScript.GetNoEgg());
            //eggScript.SetNoEgg(0);
            Destroy(gameObject);
        }
    }
}
