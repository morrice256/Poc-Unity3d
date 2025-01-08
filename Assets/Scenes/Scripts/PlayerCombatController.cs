using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombatController : MonoBehaviour
{

    [SerializeField]
    private bool isAttacking;
    [SerializeField]
    private float attack1Radius, attack1Damage;
    [SerializeField]
    private Transform atttack1HitBoxPos;
    [SerializeField]
    private LayerMask whatIsDamageable;
    private Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();
    }
    
    void Update()
    {
        CheckCombatInput();
        UpdateAnimations();
    }

    private void CheckCombatInput(){
        if(Input.GetButton("Fire1")){
            Attack1();
        }
    }

    private void Attack1(){
        isAttacking = true;        
    }

    public void FinishAttack(){
        isAttacking = false;
        anim.SetBool("isAttacking", isAttacking);
    }

    private void UpdateAnimations(){
        anim.SetBool("isAttacking", isAttacking);
    }

    private void CheckAttackHitEnemy(){
        Collider2D[] detectedObjects = Physics2D.OverlapCircleAll(atttack1HitBoxPos.position, attack1Radius, whatIsDamageable);
        foreach(Collider2D collider in detectedObjects){
            collider.transform.parent.SendMessage("Damage", attack1Damage);
        }
    }

    private void OnDrawGizmos(){
        Gizmos.DrawWireSphere(atttack1HitBoxPos.position, attack1Radius); 
    }

}
