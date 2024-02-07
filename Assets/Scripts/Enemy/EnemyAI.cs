using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public float moveSpeed = 3f;
    public Animator animator;

    public float detectionRange = 5f; // Range within which the enemy detects the player
    public float avoidanceRange = 1.5f; // Range within which the enemy avoids other enemies

    private Rigidbody2D rb;
    private Transform player;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        animator = GetComponent<Animator>();
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

                // Check for nearby enemies
                Collider2D[] nearbyEnemies = Physics2D.OverlapCircleAll(transform.position, avoidanceRange);
                Vector2 avoidanceMove = Vector2.zero;

                foreach (Collider2D enemy in nearbyEnemies)
                {
                    if (enemy.gameObject != gameObject) // Exclude self
                    {
                        Vector2 avoidDir = (transform.position - enemy.transform.position).normalized;
                        avoidanceMove += avoidDir;
                    }
                }

                direction += avoidanceMove; // Add avoidance move

                rb.velocity = direction.normalized * moveSpeed;
            }
            else
            {
                // If the player is outside the detection range, stop moving
                rb.velocity = Vector2.zero;
            }
        }
    }
}
