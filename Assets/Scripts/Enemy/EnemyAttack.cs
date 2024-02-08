using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    public int damage = 10; // Amount of damage dealt to the player per second
    public float attackRange = 1.5f; // Range within which the enemy can attack
    public float knockbackForce = 500f; // Force to apply to the player upon getting hit
    private float lastDamageTime;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // Check if enough time has passed since the last damage
            if (Time.time - lastDamageTime >= 1f) // Adjust interval as needed
            {
                // Deal damage to the player
                collision.GetComponent<PlayerHealth>().TakeDamage(damage);
                Debug.Log("Player damaged by " + damage);

                // Calculate knockback direction
                Vector2 knockbackDirection = (collision.transform.position - transform.position).normalized;

                // Apply knockback force to the player
                Rigidbody2D playerRigidbody = collision.GetComponent<Rigidbody2D>();
                if (playerRigidbody != null)
                {
                    // Apply the force in the opposite direction to the knockback direction
                    playerRigidbody.AddForce(-knockbackDirection * knockbackForce, ForceMode2D.Impulse);
                    Debug.Log("Knockback force applied");
                }
                else
                {
                    Debug.LogWarning("Player Rigidbody2D component not found");
                }

                // Update the last damage time
                lastDamageTime = Time.time;
            }
        }
    }
}
