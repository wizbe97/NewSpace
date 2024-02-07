using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public float moveSpeed = 3f;
    public float detectionRange = 5f; // Range within which the enemy detects the player

    private Rigidbody2D rb;
    private Transform player;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        if (player != null)
        {
            float distanceToPlayer = Vector2.Distance(transform.position, player.position);

            // Check if the player is within the detection range
            if (distanceToPlayer <= detectionRange)
            {
                Vector2 direction = (player.position - transform.position).normalized;
                rb.velocity = direction * moveSpeed;
            }
            else
            {
                // If the player is outside the detection range, stop moving
                rb.velocity = Vector2.zero;
            }
        }
    }

}
