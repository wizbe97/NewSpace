using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public enum PlayerStates
    {
        IDLE,
        WALK,
        ATTACK
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
                    case PlayerStates.ATTACK:
                        animator.Play("Attack");
                        stateLock = true;
                        canMove = false;
                        break;
                }
            }

        }
    }

    public float moveSpeed = 3f;

    public float attackCooldown = 0.5f; // Cooldown time between attacks
    private float lastAttackTime;
    private Vector2 attackDirection;
    public Collider2D attackCollider;
    public float attackRange = 1.5f;
    public MeleeAttack meleeAttack;



    private Vector2 moveInput = Vector2.zero;

    private Rigidbody2D rb;
    private Animator animator;

    private PlayerStates currentStateValue;

    private bool stateLock = false;
    private bool canMove = true;
    private string currentWeapon;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        attackCollider.enabled = false;
        currentWeapon = "wrench";
        meleeAttack = GetComponentInChildren<MeleeAttack>();


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
        else
        {
            rb.velocity = Vector2.zero;
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
                    currentState = PlayerStates.WALK;
                    animator.SetFloat("xMove", moveInput.x);
                    animator.SetFloat("yMove", moveInput.y);
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
                Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());

                Vector2 attackDirection = (mousePosition - transform.position).normalized;

                animator.SetFloat("xMove", attackDirection.x);
                animator.SetFloat("yMove", attackDirection.y);

                currentState = PlayerStates.ATTACK;

                lastAttackTime = Time.time;
            }

        }
        
    }

    void OnAttackStart()
    {
        // Triggered when the attack animation starts
        UpdateAttackColliderPosition();
        attackCollider.enabled = true; // Enable the collider
    }

    void OnAttackFinished()
    {
        stateLock = false;
        if (moveInput != Vector2.zero)
        {
            currentState = PlayerStates.WALK;
            animator.SetFloat("xMove", moveInput.x);
            animator.SetFloat("yMove", moveInput.y);
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

    
}