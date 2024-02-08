using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float raycastDistance = 10f;

    private GameObject player;
    private bool playerVisible = false;

    private void Start()
    {
        // Find the GameObject with the "Player" tag
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void Update()
    {
        if (player != null)
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
        transform.position += (Vector3)direction * moveSpeed * Time.deltaTime;
    }

    private void MoveTowardsLastKnownPosition()
    {
        // Move towards the last known player position
        Vector2 direction = (player.transform.position - transform.position).normalized;
        transform.position += (Vector3)direction * moveSpeed * Time.deltaTime;
    }
}
