using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EggScript : MonoBehaviour
{
    private int noOfEggs = 0;

    private int Points;



    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Egg"))
        {
            CalculateEggsCollected();
            Destroy(collision.gameObject);
        }
    }


    private void CalculateEggsCollected()
    {
        noOfEggs = noOfEggs + 1;
        SetNoEgg(noOfEggs);
        
    }
    
    private float SetNoEgg(int EggsRecieved)
    {
        Points = EggsRecieved;
        return EggsRecieved;
    }

    public int GetNoEgg()
    {
        return Points;
    }

}
