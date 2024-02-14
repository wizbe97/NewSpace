using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    public int damage = 10; // Amount of damage dealt to the player per second
    public float attackRange = 1.5f; // Range within which the enemy can attack
    private float lastDamageTime;
    private Collider2D enemyCollider;
    private PlayerHealth playerHealth;

    void Start()
    {
        enemyCollider = GetComponent<Collider2D>();
        playerHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealth>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (playerHealth.currentHealth > 0)
            {
                // Check if enough time has passed since the last damage
                if (Time.time - lastDamageTime >= 1f) // Adjust interval as needed
                {
                    // Deal damage to the player
                    collision.GetComponent<PlayerHealth>().TakeDamage(damage);
                    Debug.Log("Player damaged by " + damage);

                    // Update the last damage time
                    lastDamageTime = Time.time;
                }
            }
            else
            {
                // Disable the enemy collider if the player's health is 0 or below
                enemyCollider.enabled = false;
            }
        }
    }
}
