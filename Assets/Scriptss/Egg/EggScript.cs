using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EggScript : MonoBehaviour
{
    private int noOfEggs = 0;

    private int Points;


    /*private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Egg")
        {
            CalculateEggsCollected();
            Debug.Log("Collected 1");
            Destroy(collision.gameObject);
        }
    }*/
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Egg"))
        {
            CalculateEggsCollected();
            Debug.Log("Collected 1");
            Destroy(collision.gameObject);
        }
    }


    private void CalculateEggsCollected()
    {
        noOfEggs = noOfEggs + 1;
        SetNoEgg(noOfEggs);
        
    }
    
    public float SetNoEgg(int EggsRecieved)
    {
        Points = EggsRecieved;
        return EggsRecieved;
    }

    public int GetNoEgg()
    {
        return Points;
    }

}
