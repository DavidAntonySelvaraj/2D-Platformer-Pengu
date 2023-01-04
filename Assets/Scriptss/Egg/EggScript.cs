using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EggScript : MonoBehaviour
{
    private float points = 0;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Egg"))
        {
            CalculateEggsCollected();
            Destroy(collision.gameObject);
        }
    }


    void CalculateEggsCollected()
    {
        points = points + 1;
        Debug.Log(points);
    }
}
