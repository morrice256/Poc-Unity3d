using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [SerializeField]
    private float maxHealth;

    [SerializeField]
    private float currentHealth;

    private PlayerController PC;

    private void Start(){
        PC = GetComponent<PlayerController>();
        currentHealth = maxHealth;
    }

    public void DecreaseHealth(float amount){
        currentHealth -= amount;

        if(currentHealth <= 0.0f){
            PC.Die();
        }
    }
}
