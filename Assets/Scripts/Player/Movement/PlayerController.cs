using System.Collections;
using UnityEngine;

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

    [SerializeField] public float moveSpeed = 3f;
    public Vector2 moveInput = Vector2.zero;

    public Rigidbody2D rb;
    public Animator animator;

    public PlayerStates currentStateValue;

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

    }

    public void UpdateAnimationState(Vector2 moveInput)
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