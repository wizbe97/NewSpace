using UnityEngine;

public class TriggerWalk : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Walk");

            // Find the GameObject with PlayerAttack component
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                PlayerController playerController = player.GetComponent<PlayerController>();

                if (playerController != null)
                {
                    // Set the currentWeapon to "shotgun"
                    playerController.moveSpeed = 2.9f;
                }
                else
                {
                    Debug.LogError("PlayerController component not found on the player GameObject.");
                }
            }
            else
            {
                Debug.LogError("Player GameObject not found.");
            }

        }
    }
}
