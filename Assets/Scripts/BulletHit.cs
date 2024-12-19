using UnityEngine;

public class BulletHit : MonoBehaviour
{
    
    
    private PlayerShooting shooting;
    private void Start()
    {
        
        shooting = FindAnyObjectByType<PlayerShooting>();
    }
    private void OnTriggerEnter(Collider other)
    {
        // E�er vurulan nesne d��mansa
        if (other.CompareTag("Enemy"))
        {
            Health enemyHealth = other.GetComponent<Health>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(shooting.GetDamage()); // D��mana hasar ver

            }

            Destroy(gameObject); // Mermiyi sahneden kald�r
        }
    }
    
}
