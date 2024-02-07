using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    public int damage = 10; // Amount of damage dealt to the player per second
    public float attackRange = 1.5f; // Range within which the enemy can attack
    private float lastDamageTime;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // Check if enough time has passed since the last damage
            if (Time.time - lastDamageTime >= 1f) // Adjust interval as needed
            {
                // Deal damage to the player
                collision.GetComponent<PlayerHealth>().TakeDamage(damage);
                Debug.Log("Player damaged by " + damage);

                // Update the last damage time
                lastDamageTime = Time.time;
            }
        }
    }
}
