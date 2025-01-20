using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombatController : MonoBehaviour
{

    [SerializeField]
    private float attack1Radius, attack1Damage;
    [SerializeField]
    private Transform atttack1HitBoxPos;
    [SerializeField]
    private LayerMask whatIsDamageable;

    private bool isAttacking;
    private float[] attackDetails = new float[2];

    private Animator anim;
    private PlayerController PC;
    private PlayerStats PS;

    void Start()
    {
        anim = GetComponent<Animator>();
        PC = GetComponent<PlayerController>();
        PS = GetComponent<PlayerStats>();
    }
    
    void Update()
    {
        CheckCombatInput();
        UpdateAnimations();
    }

    private void CheckCombatInput(){
        if(!isAttacking){
            if(Input.GetButton("Fire1")){
                Attack1();
            }
        }
    }

    private void Attack1(){
        isAttacking = true;

    }

    public void FinishAttack(){
        isAttacking = false;
    }

    private void UpdateAnimations(){
        anim.SetBool("isAttacking", isAttacking);
    }

    private void CheckAttackHitEnemy(){
        Collider2D[] detectedObjects = Physics2D.OverlapCircleAll(atttack1HitBoxPos.position, attack1Radius, whatIsDamageable);

        attackDetails[0] = attack1Damage;
        attackDetails[1] = transform.position.x;

        foreach(Collider2D collider in detectedObjects){
            collider.transform.parent.SendMessage("Damage", attackDetails);
        }
    }
    
    private void Damage(float[] attackDetails){
        int direction;
        
         if(attackDetails[1] < transform.position.x){
            direction = 1;
         } else {
            direction = -1;
         }

         PC.Knockback(direction);
         PS.DecreaseHealth(attackDetails[0]);
    }

    private void OnDrawGizmos(){
        Gizmos.DrawWireSphere(atttack1HitBoxPos.position, attack1Radius); 
    }

}
