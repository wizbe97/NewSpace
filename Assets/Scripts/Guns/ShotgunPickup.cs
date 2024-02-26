using UnityEngine;

public class ShotgunPickup : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Gun Picked Up");

            // Find the GameObject with PlayerAttack component
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                // Get the PlayerAttack component from the player GameObject
                PlayerAttack playerAttack = player.GetComponent<PlayerAttack>();
                PlayerController playerController = player.GetComponent<PlayerController>();

                if (playerAttack != null)
                {
                    // Set the currentWeapon to "shotgun"
                    playerAttack.currentWeapon = "shotgun";
                    playerController.isHoldingGun = true;
                }
                else
                {
                    Debug.LogError("PlayerAttack component not found on the player GameObject.");
                }
            }
            else
            {
                Debug.LogError("Player GameObject not found.");
            }

            Destroy(gameObject);
        }
    }
}
