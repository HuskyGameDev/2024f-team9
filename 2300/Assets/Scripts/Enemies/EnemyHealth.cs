using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int health;
    public int maxHealth;


    private void OnEnable()
    {
        health = maxHealth;
    }


    private void OnTriggerEnter2D(Collider2D other) {
         if (other.gameObject.CompareTag("bullet"))
        {
            health--;
        }
    }
}
