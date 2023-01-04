using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowScript : MonoBehaviour
{

    private Transform player;

    private Vector3 tempPos;


    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void LateUpdate()
    {
        if(!player)
            return;

        tempPos = transform.position;
        tempPos.x = player.position.x;
        tempPos.y = player.position.y;
        transform.position = tempPos;
    }

}//class









