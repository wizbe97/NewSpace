using UnityEngine;
using System;

public class EnemyHealth : MonoBehaviour
{
    public int maxHealth = 100;
    private int currentHealth;

    public event Action OnDeath; // Event to be invoked when the enemy dies

    private void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
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
