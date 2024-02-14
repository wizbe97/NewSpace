using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.Collections;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;
    private bool immune = false;
    [SerializeField] private float immunityDuration = 1f;

    // Define event for when the player dies
    public static event Action OnPlayerDeath;

    private PlayerController playerController; // Reference to the PlayerController script

    void Start()
    {
        currentHealth = maxHealth;
        playerController = GetComponent<PlayerController>();
    }

    public void TakeDamage(int damage)
    {
        if (!immune) // Only take damage if not immune
        {
            currentHealth -= damage;

            if (currentHealth <= 0)
            {
                Die();
            }
            else
            {
                StartCoroutine(EnableImmunity()); // Start the immunity cooldown
            }
        }
    }

    private void Die()
    {
        Debug.Log("Player has died!");
        // Trigger the death animation and state in the PlayerController
        playerController.TriggerDeath();

        // Invoke the OnPlayerDeath event
        OnPlayerDeath?.Invoke();

        // Start coroutine to reload scene after delay
        StartCoroutine(ReloadSceneAfterDelay(5f)); // Change 5f to the desired delay time
    }

    private IEnumerator EnableImmunity()
    {
        immune = true;
        yield return new WaitForSeconds(immunityDuration);
        immune = false;
    }

    private IEnumerator ReloadSceneAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
