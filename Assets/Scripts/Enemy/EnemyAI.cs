using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float raycastDistance = 10f;

    private GameObject player;
    private bool playerVisible = false;
    private bool playerAlive = true;

    private void Start()
    {
        // Find the GameObject with the "Player" tag
        player = GameObject.FindGameObjectWithTag("Player");

        // Subscribe to the OnPlayerDeath event
        PlayerHealth.OnPlayerDeath += PlayerDied;
    }

    private void OnDestroy()
    {
        // Unsubscribe from the OnPlayerDeath event when this object is destroyed
        PlayerHealth.OnPlayerDeath -= PlayerDied;
    }

    private void Update()
    {
        if (playerAlive && player != null)
        {
            UpdatePlayerVisibility();

            if (playerVisible)
            {
                MoveTowardsPlayer();
            }
            else
            {
                MoveTowardsLastKnownPosition();
            }
        }
        else
        {
            Debug.LogWarning("No GameObject with the tag 'Player' found.");
        }
    }

    private void UpdatePlayerVisibility()
    {
        // Check if player is visible
        RaycastHit2D hit = Physics2D.Raycast(transform.position, (Vector2)(player.transform.position - transform.position), raycastDistance);
        if (hit.collider != null && hit.collider.CompareTag("Player"))
        {
            playerVisible = true;
        }
        else
        {
            playerVisible = false;
        }
    }

    private void MoveTowardsPlayer()
    {
        Vector2 direction = (player.transform.position - transform.position).normalized;
        transform.position += moveSpeed * Time.deltaTime * (Vector3)direction;
    }

    private void MoveTowardsLastKnownPosition()
    {
        if (player != null)
        {
            Vector2 direction = (player.transform.position - transform.position).normalized;
            transform.position += moveSpeed * Time.deltaTime * (Vector3)direction;
        }
    }

    // Method to handle player death
    private void PlayerDied()
    {
        playerAlive = false;
    }
}
