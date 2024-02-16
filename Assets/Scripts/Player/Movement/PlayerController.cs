using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public enum PlayerStates
    {
        IDLE,
        WALK,
        ATTACK,
        RUN,
        DIE
    }

    PlayerStates currentState
    {
        set
        {
            if (stateLock == false)
            {
                currentStateValue = value;
                switch (currentStateValue)
                {
                    case PlayerStates.IDLE:
                        animator.Play("Idle");
                        canMove = true;
                        break;
                    case PlayerStates.WALK:
                        animator.Play("Walk");
                        canMove = true;
                        break;
                    case PlayerStates.RUN:
                        animator.Play("Run");
                        canMove = true;
                        break;
                    case PlayerStates.ATTACK:
                        animator.Play("Attack");
                        stateLock = true;
                        canMove = false;
                        break;
                    case PlayerStates.DIE:
                        animator.Play("Die");
                        stateLock = true;
                        canMove = false;
                        break;
                }
            }

        }
    }

    [Header("Core Settings")]
    [SerializeField] private float moveSpeed = 3f;

    [Header("Attack Settings")]
    [SerializeField] private float attackCooldown = 0.5f; // Cooldown time between attacks
    [SerializeField] private float attackRange = 1.5f;

    private float lastAttackTime;
    private Vector2 attackDirection;
    public Collider2D attackCollider;
    private string currentWeapon;



    private Vector2 moveInput = Vector2.zero;

    private Rigidbody2D rb;
    private Animator animator;

    private PlayerStates currentStateValue;

    private bool stateLock = false;
    public bool canMove = true;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        attackCollider.enabled = false;
        currentWeapon = "wrench";

    }

    void Update()
    {
        attackDirection = new Vector2(animator.GetFloat("xMove"), animator.GetFloat("yMove"));
    }

    private void FixedUpdate()
    {
        if (canMove && !stateLock)
        {
            rb.velocity = moveInput * moveSpeed;
        }

    }

    private void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();

        if (canMove)
        {
            if (currentStateValue != PlayerStates.ATTACK)
            {
                if (moveInput != Vector2.zero)
                {
                    if (moveSpeed > 3)
                    {
                        currentState = PlayerStates.RUN;
                        animator.SetFloat("xMove", moveInput.x);
                        animator.SetFloat("yMove", moveInput.y);
                    }
                    else
                    {
                        currentState = PlayerStates.WALK;
                        animator.SetFloat("xMove", moveInput.x);
                        animator.SetFloat("yMove", moveInput.y);
                    }
                }
                else
                {
                    currentState = PlayerStates.IDLE;
                }
            }
        }
    }

    void OnCombat()
    {
        if (currentWeapon == "wrench")
        {
            if (Time.time - lastAttackTime >= attackCooldown)
            {
                // Store the current velocity before the attack
                Vector2 preAttackVelocity = rb.velocity;

                currentState = PlayerStates.ATTACK;
                canMove = false;
                lastAttackTime = Time.time;
                Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
                Vector2 attackDirection = (mousePosition - transform.position).normalized;
                animator.SetFloat("xMove", attackDirection.x);
                animator.SetFloat("yMove", attackDirection.y);

                // Start a coroutine to manage the attack collider's activation with a delay
                StartCoroutine(ActivateAttackColliderWithDelay());

                // Restore the pre-attack velocity after a short delay
                StartCoroutine(RestoreVelocityAfterAttack(preAttackVelocity, "Attack"));
            }
        }
    }

    IEnumerator RestoreVelocityAfterAttack(Vector2 preAttackVelocity, string animationState)
    {
        // Wait until the attack animation finishes
        while (animator.GetCurrentAnimatorStateInfo(0).IsName(animationState))
        {
            yield return null;
        }

        // Restore the pre-attack velocity
        rb.velocity = preAttackVelocity;

        // Allow movement again
        canMove = true;
    }



    IEnumerator ActivateAttackColliderWithDelay()
    {
        // Wait for a short delay before activating the collider
        yield return new WaitForSeconds(0.25f);

        // Enable the collider
        attackCollider.enabled = true;

        // Wait for 0.1 seconds
        yield return new WaitForSeconds(0.1f);

        // Disable the collider
        attackCollider.enabled = false;
        canMove = true;
    }



    void OnAttackStart()
    {
        // Triggered when the attack animation starts
        UpdateAttackColliderPosition();
    }


    void OnAttackFinished()
    {
        stateLock = false;
        if (moveInput != Vector2.zero)
        {
            if (moveSpeed > 3)
            {
                currentState = PlayerStates.RUN;
                animator.SetFloat("xMove", moveInput.x);
                animator.SetFloat("yMove", moveInput.y);
            }
            else
            {
                currentState = PlayerStates.WALK;
                animator.SetFloat("xMove", moveInput.x);
                animator.SetFloat("yMove", moveInput.y);
            }
        }
        else
        {
            currentState = PlayerStates.IDLE;
        }
        attackCollider.enabled = false; // Disable the collider
    }


    void UpdateAttackColliderPosition()
    {
        // Calculate the new position based on the attack direction
        Vector3 newPosition = transform.position + (Vector3)attackDirection.normalized * attackRange;

        // Set the collider's position
        attackCollider.transform.position = newPosition;
    }

    public void TriggerDeath()
    {
        currentState = PlayerStates.DIE;
    }

}