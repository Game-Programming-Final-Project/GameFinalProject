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
        // Eðer vurulan nesne düþmansa
        if (other.CompareTag("Enemy") || other.CompareTag("Boss"))
        {
            Health enemyHealth = other.GetComponent<Health>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(shooting.GetDamage()); 

            }

            Destroy(gameObject); 
        }
    }
    
}
