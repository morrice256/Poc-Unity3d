using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private float movimentInputDirection;
    private float knockbackStartTime;
    private bool isFaceRight = true;
    private bool isWalking;
    private bool isGrounded;
    private bool canJump;
    private bool isHurt;
    private bool isDead = false;

    [SerializeField]
    private bool knockBack;

    [SerializeField]
    private float knockbackDuration;

    [SerializeField]
    private Vector2 knockbackSpeed;
    
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
        if(!isDead){
            CheckInput();
            CheckMovementeDirection();
            UpdateAnimations();
            CheckIfCanJump();
            CheckKnockback();
        }
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
        anim.SetBool("isHurt", knockBack);
    }

    private void CheckInput(){
        movimentInputDirection = Input.GetAxisRaw("Horizontal");
        if(Input.GetButtonDown("Jump")){
            Jump();
        }
    }

    private void ApplyMovement(){
        if(!knockBack){
            rb.velocity = new Vector2(movimentSpeed * movimentInputDirection, rb.velocity.y);
        }
        CheckKnockback();
    }

    private void Flip(){
        if(!knockBack){
            isFaceRight = !isFaceRight;
            transform.Rotate(0.0f, 180.0f, 0.0f);
        }
    }

    private void Jump(){
        if(canJump && !knockBack){
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }
    }
    
    public void Knockback(int direction){        
         knockBack = true;
         knockbackStartTime = Time.time;
         rb.velocity = new Vector2(knockbackSpeed.x * direction, knockbackSpeed.y);

    }

    private void CheckKnockback(){
        if(Time.time >= knockbackStartTime + knockbackDuration && knockBack){
            knockBack = false;
            rb.velocity = new Vector2(0.0f, rb.velocity.y);
        }
    }

    private void OnDrawGizmos(){
        Gizmos.DrawWireSphere(goundCheck.position, groundCheckRadius); 
    }

    public int GetFacingDirection(){
        if(isFaceRight){
            return 1;
        } else {
            return -1;
        }
    }

    public void Die(){
        anim.SetBool("isDead", true);
        isDead = true;
        Destroy(gameObject, 10);
    }


}
