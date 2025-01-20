using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicEnemyController : MonoBehaviour
{
    // Start is called before the first frame update
    private enum State{
        Moving,
        KnockBack,
        Dead
    }

    [SerializeField]
    private State currentState;
    [SerializeField]
    private float groundCheckDistance, wallCheckDistance, movementSpeed, maxHealth, knockbackDuration,
        lastTouchDamageTime,
        touchDamageCooldown,
        touchDamage,
        touchDamageWidth,
        touchDamageHeight;
    [SerializeField]
    private Transform groundCheck, wallCheck, touchDamageCheck;
    [SerializeField]
    private LayerMask whatIsGround, whatIsPlayer;
    [SerializeField]
    private Vector2 knockbackSpeed;
    [SerializeField]
    private GameObject hitParticle, deathChunkParticle, deathBloodParticle;

    private float currentHealth, knockbackStartTime;

    private float[] attackDetails = new float[2];

    private int facingDirection, damageDirection;

    private Vector2 movement, 
        touchDamageBothLeft, touchDamageBothRight;

    private bool groundDetected, wallDetected;

    private GameObject alive;
    private Rigidbody2D aliveRb;
    private Animator aliveAnim;

    private void Start(){
        alive = transform.Find("Alive").gameObject;
        aliveRb = alive.GetComponent<Rigidbody2D>();
        aliveAnim = alive.GetComponent<Animator>();

        facingDirection = 1;
        currentHealth = maxHealth;
    }

    private void Update(){
        
        switch (currentState){
            case State.Moving:
                UpdateMovingState();
                break;
            case State.KnockBack:
                UpdateKnockbackState();
                break;
            case State.Dead:
                UpdateDeadkState();
                break;

        }
    }

    //Walk state

    private void EnterMovingState(){

    }

    private void UpdateMovingState(){
        groundDetected = Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, whatIsGround);
        wallDetected = Physics2D.Raycast(wallCheck.position, Vector2.down, wallCheckDistance, whatIsGround);

        CheckTouchDamage();

        if(!groundDetected || wallDetected){
            Flip();
        } else {
            movement.Set(movementSpeed * facingDirection, aliveRb.velocity.y);
            aliveRb.velocity = movement;
        }
    }

    private void ExitMovingState(){

    }

    //Knockback

    private void EnterKnockbackState(){
        knockbackStartTime = Time.time;
        movement.Set(knockbackSpeed.x * damageDirection, knockbackSpeed.y);
        aliveRb.velocity = movement;
        aliveAnim.SetBool("Knockback", true);
    }

    private void UpdateKnockbackState(){
        if(Time.time >= knockbackStartTime + knockbackDuration){
            SwitchState(State.Moving);
        }
    }

    private void ExitKnockbackState(){
        aliveAnim.SetBool("Knockback", false);
    }

    //Dead

    private void EnterDeadState(){
        //Spawn Chunks and blood; - Revisar
        aliveAnim.SetBool("isDead", true);
        alive.GetComponent<Collider2D>().enabled = false;
        Destroy(aliveRb);
        Destroy(gameObject, 10);
    }

    private void UpdateDeadkState(){

    }

    private void ExitDeadkState(){

    }

    private void Damage(float[] attackDetails){
         currentHealth -= attackDetails[0];

        Instantiate(hitParticle, alive.transform.position, Quaternion.Euler(0.0f, 0.0f, Random.Range(0.0f, 360.0f)));

         if(attackDetails[1] > alive.transform.position.x){
            damageDirection = -1;
         } else {
            damageDirection = 1;
         }
         
         //Hit Particule

         if(currentHealth > 0.0f){
            SwitchState(State.KnockBack);
         } else if (currentHealth <= 0.0f) {
            SwitchState(State.Dead);
         }
    }

    private void CheckTouchDamage(){
        if(Time.time >= lastTouchDamageTime + touchDamageCooldown){
            touchDamageBothLeft.Set(touchDamageCheck.position.x - (touchDamageWidth / 2), touchDamageCheck.position.y - (touchDamageHeight / 2));
            touchDamageBothRight.Set(touchDamageCheck.position.x + (touchDamageWidth / 2), touchDamageCheck.position.y + (touchDamageHeight / 2));

            Collider2D hit = Physics2D.OverlapArea(touchDamageBothLeft, touchDamageBothRight, whatIsPlayer);
            if(hit != null){
                lastTouchDamageTime = Time.time;
                attackDetails[0] = touchDamage;
                attackDetails[1] = alive.transform.position.x;
                hit.SendMessage("Damage", attackDetails);
            }
        }
    }

    private void Flip(){
        facingDirection *= -1;
        alive.transform.Rotate(0.0f, 180.0f, 0.0f);
    }

    private void SwitchState(State state){
        switch (currentState){
            case State.Moving:
                ExitMovingState();
                break;
            case State.KnockBack:
                ExitKnockbackState();
                break;
            case State.Dead:
                ExitDeadkState();
                break;
        }

        switch (state){
            case State.Moving:
                EnterMovingState();
                break;
            case State.KnockBack:
                EnterKnockbackState();
                break;
            case State.Dead:
                EnterDeadState();
                break;
        }

        currentState = state;
    }

    private void OnDrawGizmos(){
        Gizmos.DrawLine(groundCheck.position, new Vector2(groundCheck.position.x, groundCheck.position.y - groundCheckDistance));
        Gizmos.DrawLine(wallCheck.position, new Vector2(wallCheck.position.x + wallCheckDistance, wallCheck.position.y));

        Vector2 botLeft = new Vector2(touchDamageCheck.position.x - (touchDamageWidth / 2), touchDamageCheck.position.y - (touchDamageHeight / 2));
        Vector2 botRight = new Vector2(touchDamageCheck.position.x + (touchDamageWidth / 2), touchDamageCheck.position.y - (touchDamageHeight / 2));
        Vector2 topRight = new Vector2(touchDamageCheck.position.x + (touchDamageWidth / 2), touchDamageCheck.position.y + (touchDamageHeight / 2));
        Vector2 topLeft = new Vector2(touchDamageCheck.position.x - (touchDamageWidth / 2), touchDamageCheck.position.y + (touchDamageHeight / 2));
        
        Gizmos.DrawLine(botLeft, botRight);
        Gizmos.DrawLine(botRight, topRight);
        Gizmos.DrawLine(topRight, topLeft);
        Gizmos.DrawLine(topLeft, botLeft);
    }
}
