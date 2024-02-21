using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerAttack : MonoBehaviour
{
    PlayerController playerController;


    [SerializeField] private float attackCooldown = 0.5f; // Cooldown time between attacks
    [SerializeField] private float attackRange = 1.5f;
    private float lastAttackTime;
    private Vector2 attackDirection;
    [SerializeField] private Collider2D attackCollider;
    public string currentWeapon;

    // Start is called before the first frame update
    void Start()
    {
        playerController = GetComponent<PlayerController>();
        attackCollider.enabled = false;
        currentWeapon = "wrench";
    }

    // Update is called once per frame
    void Update()
    {
        attackDirection = new Vector2(playerController.animator.GetFloat("xMove"), playerController.animator.GetFloat("yMove"));

    }

    public void Attack()
    {
        if (currentWeapon == "wrench")
        {
            if (Time.time - lastAttackTime >= attackCooldown)
            {
                // Store the current velocity before the attack
                Vector2 preAttackVelocity = playerController.rb.velocity;

                playerController.currentState = PlayerController.PlayerStates.ATTACK;
                playerController.canMove = false;
                lastAttackTime = Time.time;
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

    private IEnumerator RestoreVelocityAfterAttack(Vector2 preAttackVelocity, string animationState)
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

    private IEnumerator ActivateAttackColliderWithDelay()
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

    void OnAttackFinished()
    {
        playerController.stateLock = false;
        playerController.UpdateAnimationState(playerController.moveInput);
        attackCollider.enabled = false; // Disable the collider
    }

    void UpdateAttackColliderPosition()
    {
        // Calculate the new position based on the attack direction
        Vector3 newPosition = transform.position + (Vector3)attackDirection.normalized * attackRange;

        // Set the collider's position
        attackCollider.transform.position = newPosition;
    }

    void OnAttackStart()
    {
        // Triggered when the attack animation starts
        UpdateAttackColliderPosition();
    }

}
