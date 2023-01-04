using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowScript : MonoBehaviour
{
    [SerializeField]
    private float maxY, maxX, minY, minX;


    private void Update()
    {
        OutOfBoundries();
    }

    void OutOfBoundries()
    {
        if(transform.position.x > maxX)
            transform.position = new Vector3(maxX,transform.position.y,transform.position.z);

        if(transform.position.x < minX)
            transform.position = new Vector3(minX, transform.position.y, transform.position.z);

        if(transform.position.y < minY)
            transform.position = new Vector3(transform.position.x, minY, transform.position.z);

        if(transform.position.y > maxY)
            transform.position = new Vector3(transform.position.x, maxY, transform.position.z);
    }

}//class









