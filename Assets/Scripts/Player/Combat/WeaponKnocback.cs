using UnityEngine;

public class WeaponKnockback : MonoBehaviour
{
    public float knockbackForce = 10f;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            // Calculate knockback direction based on mouse position
            Vector2 knockbackDirection = (GetMouseWorldPosition() - (Vector2)other.transform.position).normalized;

            // Apply knockback force to the enemy
            Rigidbody2D enemyRigidbody = other.GetComponent<Rigidbody2D>();
            if (enemyRigidbody != null)
            {
                enemyRigidbody.velocity = Vector2.zero; // Reset velocity before applying knockback
                enemyRigidbody.AddForce(knockbackDirection * knockbackForce, ForceMode2D.Impulse);
            }
        }
    }

    // Function to get mouse position in world coordinates
    private Vector2 GetMouseWorldPosition()
    {
        return Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }
}
