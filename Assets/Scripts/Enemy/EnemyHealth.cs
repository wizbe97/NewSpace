using UnityEngine;
using System;

public class EnemyHealth : MonoBehaviour
{
    public int maxHealth = 100;
    private int currentHealth;
    private PlayerStats playerStats; // Reference to the PlayerStats component

    public event Action OnDeath; // Event to be invoked when the enemy dies

    private void Start()
    {
        currentHealth = maxHealth;
        playerStats = FindObjectOfType<PlayerStats>(); // Find the PlayerStats component in the scene
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
        // Check if PlayerStats component is found
        if (playerStats != null)
        {
            // Update player's gold when the enemy dies
            playerStats.AddGold(10); // Adjust the gold amount as needed
        }
        else
        {
            Debug.LogWarning("PlayerStats component not found.");
        }

        // Invoke the OnDeath event when the enemy dies
        OnDeath?.Invoke();

        // Implement death behavior, e.g., play death animation, disable game object, etc.
        Destroy(gameObject);
    }
}
