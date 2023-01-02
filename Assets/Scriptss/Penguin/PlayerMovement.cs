using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    private Rigidbody2D penguRb;

    [SerializeField]
    private float moveSpeed = 10f;

    [SerializeField]
    private float jumpForce = 5f;

    [SerializeField]
    private float fallMultiplier = 2.5f , lowJumpMultiplier = 3f;

    [SerializeField]
    private float airMovementSpeed;
    
    [SerializeField]
    private GameObject footPos;

    private bool canJump, canMove;

    private float movementVector;

    private Animator anim;

    private SpriteRenderer sr;


    private void Awake()
    {
        penguRb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        movementVector = Input.GetAxis("Horizontal");
        AnimatePlayer();
        FlipPlayer();
        JumpPhysics();
        CheckFootStat();
    }

    private void FixedUpdate()
    {
        JumpPlayer();
        PlayerMove();
    }

    void PlayerMove()
    {
        /*moveVector = new Vector2(moveSpeed, penguRb.velocity.y);
        penguRb.MovePosition(penguRb.position + moveVector*movementVector * Time.deltaTime);*/
        
        if (canMove)
        {
            transform.position = new Vector2(transform.position.x + moveSpeed * movementVector * Time.deltaTime, transform.position.y);
        }
      
    }

    void AnimatePlayer()
    {
        if (movementVector != 0)
            anim.Play("Walk");

        if (movementVector == 0)
            anim.Play("Idle");
    }

    void FlipPlayer()
    {
        if (movementVector > 0)
            sr.flipX = false;
        if (movementVector < 0)
            sr.flipX = true;
    }

    void JumpPlayer()
    {
        if (Input.GetKey(KeyCode.Space) && canJump)
        {
            penguRb.velocity = new Vector2(penguRb.velocity.x, jumpForce);
        }
    }


    void JumpPhysics()
    {
        if (penguRb.velocity.y < 0)
        {
            penguRb.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
            transform.position = new Vector2(transform.position.x + airMovementSpeed * movementVector * Time.deltaTime, transform.position.y);
            canMove = false;
        }
        if (penguRb.velocity.y > 0)
        {
            penguRb.velocity += Vector2.up * Physics2D.gravity *(lowJumpMultiplier - 1)* Time.deltaTime;
            canMove = true;

        }
    }

    void CheckFootStat()
    {
        float radius = .3f;
        RaycastHit2D hit = Physics2D.Raycast(footPos.transform.position,-transform.up,radius,3 << LayerMask.NameToLayer("Ground"));
        Debug.DrawRay(footPos.transform.position, -transform.up, Color.white);

        if (hit)
        {
            canJump = true;
            canMove = true;
            Debug.Log(canJump);
            
        }
        else
        {
            canJump = false;
            anim.Play("Jump");
            Debug.Log(canJump);
        }

        
    }

    

}//class



















