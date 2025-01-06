using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private float movimentInputDirection;
    private bool isFaceRight = true;
    private Rigidbody2D rb;

    public float movimentSpeed = 10.0f;
    public float jumpForce = 18.0f;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        CheckInput();
        CheckMovementeDirection();
        
    }

    private void FixedUpdate(){
        ApplyMovement();
    }

    private void CheckMovementeDirection(){
        if(isFaceRight && movimentInputDirection < 0){
            Flip();
        } else if(!isFaceRight && movimentInputDirection > 0){
            Flip();
        }
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
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
    }
}
