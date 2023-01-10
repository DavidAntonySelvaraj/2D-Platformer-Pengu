using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScorpianScript : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed;

    private Rigidbody2D ScorpianRb;

    private SpriteRenderer sr;

    [SerializeField]
    private float maxX, minX;

   


    private void Awake()
    {
        ScorpianRb = GetComponent<Rigidbody2D>();   
        sr = GetComponent<SpriteRenderer>();
    }

    private void FixedUpdate()
    {
        Move();
        ChangeDirection();
    }

    void ChangeDirection()
    {
        if(transform.position.x > maxX)
            sr.flipX = true;

        if(transform.position.x < minX)
            sr.flipX=false;
    }
    void Move()
    {
        if (sr.flipX == true)
        {
            ScorpianRb.velocity = new Vector2(-moveSpeed * Time.deltaTime, ScorpianRb.velocity.y);
        }
        else
        {
            ScorpianRb.velocity = new Vector2(moveSpeed * Time.deltaTime, ScorpianRb.velocity.y);
        }


        //ScorpianRb.velocity = new Vector2(moveSpeed * Time.deltaTime, ScorpianRb.velocity.y);
    }

    

    

    
}
