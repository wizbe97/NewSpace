using UnityEngine;

public class PlayerKnockback : MonoBehaviour
{
    public float knockbackForce = 10f;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("PlayerDamageCollider"))
        {
            // Calculate knockback direction
            Vector2 knockbackDirection = (other.transform.position - transform.position).normalized;

            // Apply knockback force to the enemy
            Rigidbody2D playerRigidBody = other.GetComponent<Rigidbody2D>();
            if (playerRigidBody != null)
            {
                playerRigidBody.velocity = Vector2.zero; // Reset velocity before applying knockback
                playerRigidBody.AddForce(knockbackDirection * knockbackForce, ForceMode2D.Impulse);
            }
        }
    }
}
