using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Wrench : MonoBehaviour
{
    PlayerAttack playerAttack;
    PlayerController playerController;
    [SerializeField] public Collider2D attackCollider;
    [SerializeField] private int damage = 20;
    [SerializeField] public float attackCooldown = 0.5f; // Cooldown time between attacks
    [SerializeField] public float attackRange = 1.5f;

    void Start()
    {
        playerAttack = GetComponentInParent<PlayerAttack>();
        playerController = GetComponentInParent<PlayerController>();
        attackCollider.enabled = false;
    }

    public void Attack(Vector2 attackDirection)
    {
        if (Time.time - playerAttack.lastAttackTime >= attackCooldown)
        {
            // Store the current velocity before the attack
            Vector2 preAttackVelocity = playerController.rb.velocity;

            playerController.currentState = PlayerController.PlayerStates.ATTACK;
            playerController.canMove = false;
            playerAttack.lastAttackTime = Time.time;

            // Set the animation parameters
            playerController.animator.SetFloat("mouseX", attackDirection.x);
            playerController.animator.SetFloat("mouseY", attackDirection.y);

            // Update the attack collider's position based on the attack direction
            UpdateAttackColliderPosition(attackDirection);

            // Start a coroutine to manage the attack collider's activation with a delay
            StartCoroutine(ActivateAttackColliderWithDelay());

            // Restore the pre-attack velocity after a short delay
            StartCoroutine(RestoreVelocityAfterAttack(preAttackVelocity, "Attack"));
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            // Reduce enemy health
            EnemyHealth enemyHealth = other.GetComponent<EnemyHealth>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(damage);
            }
        }
    }

    public void UpdateAttackColliderPosition(Vector2 attackDirection)
    {
        // Calculate the new position based on the attack direction
        Vector3 newPosition = playerController.transform.position + (Vector3)attackDirection.normalized * attackRange;

        // Set the collider's position
        attackCollider.transform.position = newPosition;
    }

    public IEnumerator ActivateAttackColliderWithDelay()
    {
        // Wait for a short delay before activating the collider
        yield return new WaitForSeconds(0.25f);

        // Enable the collider
        attackCollider.enabled = true;

        // Wait for 0.1 seconds
        yield return new WaitForSeconds(0.1f);

        // Disable the collider
        attackCollider.enabled = false;
        playerController.canMove = true;
    }

    public IEnumerator RestoreVelocityAfterAttack(Vector2 preAttackVelocity, string animationState)
    {
        // Wait until the attack animation finishes
        while (playerController.animator.GetCurrentAnimatorStateInfo(0).IsName(animationState))
        {
            yield return null;
        }

        // Restore the pre-attack velocity
        playerController.rb.velocity = preAttackVelocity;

        // Allow movement again
        playerController.canMove = true;
    }
}
