using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttack : MonoBehaviour
{
    PlayerController playerController;
    Wrench wrench;

    public float lastAttackTime;
    private Vector2 attackDirection;
    public string currentWeapon;

    void Start()
    {
        playerController = GetComponent<PlayerController>();
        currentWeapon = "wrench";
        wrench = GetComponentInChildren<Wrench>();

    }
    public void Attack()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        attackDirection = (mousePosition - transform.position).normalized;
        
        if (currentWeapon == "wrench")
        {
            wrench.Attack(attackDirection);
        }

        else if (currentWeapon == "shotgun")
        {
            Debug.Log("Shotgun attack");
        }
    }

    public void OnAttackStart()
    {
        wrench.UpdateAttackColliderPosition(attackDirection);
    }

    public void OnAttackFinished()
    {
        playerController.stateLock = false;
        playerController.canMove = true;
        playerController.UpdateAnimationState(playerController.moveInput);
        wrench.attackCollider.enabled = false;
    }
}
