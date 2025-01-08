using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private float movimentInputDirection;
    private bool isFaceRight = true;
    private bool isWalking;
    private bool isGrounded;
    private bool canJump;

    private Rigidbody2D rb;
    private Animator anim;

    public float movimentSpeed = 8.0f;
    public float jumpForce = 15.0f;
    public float groundCheckRadius;

    public Transform goundCheck;
    public LayerMask whatIsGround;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        CheckInput();
        CheckMovementeDirection();
        UpdateAnimations();
        CheckIfCanJump();
    }

    private void FixedUpdate(){
        ApplyMovement();
        CheckSurroundings();
    }

    private void CheckSurroundings(){
        isGrounded = Physics2D.OverlapCircle(goundCheck.position, groundCheckRadius, whatIsGround);
    }

    private void CheckIfCanJump(){
        canJump = (isGrounded && rb.velocity.y <= 0);
    }

    private void CheckMovementeDirection(){

        if(isFaceRight && movimentInputDirection < 0){
            Flip();
        } else if(!isFaceRight && movimentInputDirection > 0){
            Flip();
        }

        isWalking = (rb.velocity.x != 0);

    }

    private void UpdateAnimations(){
        anim.SetBool("isWalking", isWalking);
        anim.SetBool("isGrounded", isGrounded);
        anim.SetFloat("yVelocity", rb.velocity.y);
    }

    private void CheckInput(){
        movimentInputDirection = Input.GetAxisRaw("Horizontal");
        if(Input.GetButtonDown("Jump")){
            Jump();
        }
    }

    private void ApplyMovement(){
        rb.velocity = new Vector2(movimentSpeed * movimentInputDirection, rb.velocity.y);
    }

    private void Flip(){
        isFaceRight = !isFaceRight;
        transform.Rotate(0.0f, 180.0f, 0.0f);
    }

    private void Jump(){
        if(canJump){
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }
    }

    private void OnDrawGizmos(){
        Gizmos.DrawWireSphere(goundCheck.position, groundCheckRadius); 
    }
}
