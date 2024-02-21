using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputActions : MonoBehaviour
{
    PlayerController playerController;
    PlayerAttack playerAttack;

    void Start()
    {
        playerController = GetComponent<PlayerController>();
        playerAttack = GetComponent<PlayerAttack>();

    }

    private void OnMove(InputValue value)
    {
        playerController.moveInput = value.Get<Vector2>();

        if (playerController.canMove && playerController.currentStateValue != PlayerController.PlayerStates.ATTACK)
        {
            playerController.UpdateAnimationState(playerController.moveInput);
        }
    }

    private void OnCombat()
    {
        playerAttack.Attack();
    }
}