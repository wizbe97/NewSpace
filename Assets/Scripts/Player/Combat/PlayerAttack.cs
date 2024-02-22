using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerAttack : MonoBehaviour
{
    PlayerController playerController;
    Wrench wrench;


    [SerializeField] public float attackCooldown = 0.5f; // Cooldown time between attacks
    [SerializeField] public float attackRange = 1.5f;
    public float lastAttackTime;
    public Vector2 attackDirection;
    [SerializeField] public Collider2D attackCollider;
    public string currentWeapon;


    // Start is called before the first frame update
    void Start()
    {
        playerController = GetComponent<PlayerController>();
        attackCollider.enabled = false;
        currentWeapon = "wrench";
        wrench = GetComponentInChildren<Wrench>();
    }

    // Update is called once per frame


    public void Attack()
    {
        wrench.Attack();
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

    public void OnAttackFinished()
    {
        playerController.stateLock = false;
        playerController.UpdateAnimationState(playerController.moveInput);
        attackCollider.enabled = false; // Disable the collider
    }

    public void UpdateAttackColliderPosition()
    {
        // Calculate the new position based on the attack direction
        Vector3 newPosition = transform.position + (Vector3)attackDirection.normalized * attackRange;

        // Set the collider's position
        attackCollider.transform.position = newPosition;
    }

    public void OnAttackStart()
    {
        // Triggered when the attack animation starts
        UpdateAttackColliderPosition();
    }


}
