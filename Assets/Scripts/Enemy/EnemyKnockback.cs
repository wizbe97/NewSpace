using System.Collections;
using UnityEngine;

public class EnemyKnockback : MonoBehaviour
{
    public float knockbackForce = 5f;
    public float knockbackDuration = 0.25f; // Adjust this value as needed
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
            playerController.canMove = false; // Disable player movement
            Vector2 knockbackVector = knockbackDirection * knockbackForce;
            StartCoroutine(ApplyKnockback(other.transform, knockbackVector / 2, knockbackDuration));
        }
    }

    IEnumerator ApplyKnockback(Transform playerTransform, Vector2 knockbackVector, float duration)
    {
        float timer = 0f;
        while (timer < duration)
        {
            // Move the player in the knockback direction over time
            playerTransform.position += (Vector3)knockbackVector * Time.deltaTime;
            timer += Time.deltaTime;
            yield return null;
        }

        playerController.canMove = true; // Re-enable player movement
                                         // Update animation state based on movement input

        playerController.UpdateAnimationState(playerController.moveInput);

    }
}
