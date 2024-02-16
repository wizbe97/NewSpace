using UnityEngine;

public class EnemyKnockback : MonoBehaviour
{
    public float knockbackForce = 10f;
    public float knockbackDuration = 0.5f; // Adjust this value as needed
    private PlayerController playerController;

    private void Start()
    {
        playerController = FindObjectOfType<PlayerController>(); // Find the player controller in the scene
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Calculate knockback direction
            Vector2 knockbackDirection = (other.transform.position - transform.position).normalized;

            // Apply knockback force to the player
            Rigidbody2D playerRigidbody = other.GetComponent<Rigidbody2D>();
            if (playerRigidbody != null)
            {
                playerController.canMove = false; // Disable player movement
                playerRigidbody.velocity = Vector2.zero; // Reset velocity before applying knockback
                playerRigidbody.AddForce(knockbackDirection * knockbackForce, ForceMode2D.Impulse);

                // Re-enable movement after a delay
                Invoke("EnablePlayerMovement", knockbackDuration);
            }
        }
    }

    void EnablePlayerMovement()
    {
        playerController.canMove = true; // Re-enable player movement
    }
}
