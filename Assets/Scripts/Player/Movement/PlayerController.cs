using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public enum PlayerStates
    {
        IDLE,
        IDLE_HOLDING_GUN,
        WALK,
        WALK_HOLDING_GUN,
        RUN,
        RUN_HOLDING_GUN,
        ATTACK,
        DIE
    }

    public PlayerStates currentState
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
                    case PlayerStates.IDLE_HOLDING_GUN:
                        animator.Play("Idle_Holding_Gun");
                        canMove = true;
                        break;
                    case PlayerStates.WALK:
                        animator.Play("Walk");
                        canMove = true;
                        break;
                    case PlayerStates.WALK_HOLDING_GUN:
                        animator.Play("Walk_Holding_Gun");
                        canMove = true;
                        break;
                    case PlayerStates.RUN:
                        animator.Play("Run");
                        canMove = true;
                        break;
                    case PlayerStates.RUN_HOLDING_GUN:
                        animator.Play("Run_Holding_Gun");
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

    [SerializeField] public float moveSpeed = 3f;
    public Vector2 moveInput = Vector2.zero;

    public Rigidbody2D rb;
    public Animator animator;


    public PlayerStates currentStateValue;
    public bool isHoldingGun = false;
    public bool stateLock = false;
    public bool canMove = true;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
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
        UpdateAnimationState(moveInput);

    }

    public void UpdateAnimationState(Vector2 moveInput)
    {
        bool isMoving = moveInput != Vector2.zero;

        int stateIdentifier;

        if (isMoving)
        {
            if (isHoldingGun)
            {
                stateIdentifier = moveSpeed >= 3 ? 1 : 2;
            }
            else
            {
                stateIdentifier = moveSpeed >= 3 ? 3 : 4;
            }
        }
        else
        {
            stateIdentifier = isHoldingGun ? 5 : 6;
        }

        switch (stateIdentifier)
        {
            case 1: // PLAYER IS MOVING AND HOLDING A GUN, MOVE SPEED IS GREATER THAN 3
                PlayerFollowMouse();
                currentState = PlayerStates.RUN_HOLDING_GUN;
                break;

            case 2: // PLAYER IS MOVING AND HOLDING A GUN, MOVE SPEED IS LESS THAN 3
                PlayerFollowMouse();
                currentState = PlayerStates.WALK_HOLDING_GUN;
                break;

            case 3: // PLAYER IS MOVING, NOT HOLDING A GUN, MOVE SPEED IS GREATER THAN 3
                PlayerFaceMovementDirection();
                currentState = PlayerStates.RUN;
                break;

            case 4: // PLAYER IS MOVING, NOT HOLDING A GUN, MOVE SPEED IS LESS THAN 3
                PlayerFaceMovementDirection();
                currentState = PlayerStates.WALK;
                break;

            case 5: // PLAYER IS IDLE AND HOLDING A GUN
                PlayerFollowMouse();
                currentState = PlayerStates.IDLE_HOLDING_GUN;
                break;

            case 6: // PLAYER IS IDLE AND NOT HOLDING A GUN
                currentState = PlayerStates.IDLE;
                break;
        }
    }


    void PlayerFollowMouse()
    {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 playerToMouse = (mousePosition - (Vector2)transform.position).normalized;
        animator.SetFloat("mouseX", playerToMouse.x);
        animator.SetFloat("mouseY", playerToMouse.y);
    }

    void PlayerFaceMovementDirection()
    {
        animator.SetFloat("xMove", moveInput.x);
        animator.SetFloat("yMove", moveInput.y);
    }

}