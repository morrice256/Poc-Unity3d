using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatController : MonoBehaviour
{
    [SerializeField]
    private bool applyKnockback;
    private bool knockback, playerOnLeft;
    [SerializeField]
    private float maxHealth, knockbackSpeedX, knockbackSpeedY, knockbackDuration;
    private float currentHealth, knockbackStart;

    private PlayerController pc;
    private GameObject goEnemy;
    private Rigidbody2D rbEnemy;
    private Animator animEnemy;

    private void Start(){
        currentHealth = maxHealth;
        pc = GameObject.Find("Player").GetComponent<PlayerController>();
        goEnemy = transform.Find("Enemy-SK").gameObject;
        animEnemy = goEnemy.GetComponent<Animator>();
        rbEnemy = goEnemy.GetComponent<Rigidbody2D>();
        goEnemy.SetActive(true);
    }

    void Update()
    {
        CheckKnockBack();
    }

    private void Damage(float amount){
        currentHealth -= amount;
        animEnemy.SetTrigger("damage");

        if(applyKnockback && currentHealth > 0.0f){
            Knockback();
        }
        if(currentHealth <= 0.0f){
            Die();
        }

    }

    private void Die(){
        Knockback();
        animEnemy.SetBool("isDead", true);
    }

    private void Knockback(){
        knockback = true;
        knockbackStart = Time.time;
        rbEnemy.velocity = new Vector2(knockbackSpeedX * pc.GetFacingDirection(), knockbackSpeedY);
    }

    private void CheckKnockBack(){
        if(Time.time >= knockbackStart + knockbackDuration && knockback){
            knockback = false;
            rbEnemy.velocity = new Vector2(0.0f, rbEnemy.velocity.y);
        }
    }
}
