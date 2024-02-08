using UnityEngine;

public class MeleeAttack : MonoBehaviour
{
    [SerializeField] private int damage = 20;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            // Reduce enemy health
            EnemyHealth enemyHealth = other.GetComponent<EnemyHealth>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(damage);
            }
        }
    }
}
