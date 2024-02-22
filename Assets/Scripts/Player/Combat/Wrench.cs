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
                StartCoroutine(playerAttack.ActivateAttackColliderWithDelay());

                // Restore the pre-attack velocity after a short delay
                StartCoroutine(playerAttack.RestoreVelocityAfterAttack(preAttackVelocity, "Attack"));
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
}
