using UnityEngine;

public class BulletHit : MonoBehaviour
{
    public int damage = 20; // Mermi hasar�
    
    
    private void OnTriggerEnter(Collider other)
    {
        // E�er vurulan nesne d��mansa
        if (other.CompareTag("Enemy"))
        {
            Health enemyHealth = other.GetComponent<Health>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(damage); // D��mana hasar ver

            }

            Destroy(gameObject); // Mermiyi sahneden kald�r
        }
    }
}
