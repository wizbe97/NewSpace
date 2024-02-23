using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class Wrench : MonoBehaviour
{
    PlayerAttack playerAttack;
    PlayerController playerController;

    [SerializeField] private int damage = 20;

    void Start()
    {
        playerAttack = GetComponentInParent<PlayerAttack>();
        playerController = GetComponentInParent<PlayerController>();

    }

    void Update()
    {
        playerAttack.attackDirection = new Vector2(playerController.animator.GetFloat("xMove"), playerController.animator.GetFloat("yMove"));

    }

    public void Attack()
    {
        if (playerAttack.currentWeapon == "wrench")
        {
            if (Time.time - playerAttack.lastAttackTime >= playerAttack.attackCooldown)
            {
                // Store the current velocity before the attack
                Vector2 preAttackVelocity = playerController.rb.velocity;

                playerController.currentState = PlayerController.PlayerStates.ATTACK;
                playerController.canMove = false;
                playerAttack.lastAttackTime = Time.time;
                Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
                Vector2 attackDirection = (mousePosition - transform.position).normalized;
                playerController.animator.SetFloat("xMove", attackDirection.x);
                playerController.animator.SetFloat("yMove", attackDirection.y);

                // Start a coroutine to manage the attack collider's activation with a delay
                StartCoroutine(ActivateAttackColliderWithDelay());

                // Restore the pre-attack velocity after a short delay
                StartCoroutine(RestoreVelocityAfterAttack(preAttackVelocity, "Attack"));
            }
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

        public IEnumerator ActivateAttackColliderWithDelay()
    {
        // Wait for a short delay before activating the collider
        yield return new WaitForSeconds(0.25f);

        // Enable the collider
        playerAttack.attackCollider.enabled = true;

        // Wait for 0.1 seconds
        yield return new WaitForSeconds(0.1f);

        // Disable the collider
        playerAttack.attackCollider.enabled = false;
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
