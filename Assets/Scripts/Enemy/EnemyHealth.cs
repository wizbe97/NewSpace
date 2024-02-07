using UnityEngine;
using System;

public class EnemyHealth : MonoBehaviour
{
    public int maxHealth = 100;
    private int currentHealth;
    private Rigidbody2D rb;

    public event Action OnDeath; // Event to be invoked when the enemy dies

    private void Start()
    {
        currentHealth = maxHealth;
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("DefaultMeleeAttack"))
        {
            // Pass the player GameObject to TakeDamage method
            TakeDamage(20, collision.gameObject);
        }
    }

    private void TakeDamage(int damage, GameObject player)
    {
        currentHealth -= damage;
        Debug.Log("Enemy Health: " + currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        // Invoke the OnDeath event when the enemy dies
        OnDeath?.Invoke();

        // Implement death behavior, e.g., play death animation, disable game object, etc.
        Destroy(gameObject);
    }
}
