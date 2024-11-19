using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int health;
    public int maxHealth;
    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        Debug.Log($"hit by {other.collider.name}");
        if (other.gameObject.CompareTag("bullet"))
        {
            health--;
        }
    }
}
