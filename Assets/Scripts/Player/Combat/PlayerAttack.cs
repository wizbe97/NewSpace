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

    public void Attack()
    {
        if (currentWeapon == "wrench")
        {
            wrench.Attack();
        }
    }

    public void OnAttackStart()
    {
        // Triggered when the attack animation starts
        UpdateAttackColliderPosition();
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

}
