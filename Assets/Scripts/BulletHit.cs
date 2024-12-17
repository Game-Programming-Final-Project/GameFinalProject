using UnityEngine;

public class BulletHit : MonoBehaviour
{
    public int damage = 20; // Mermi hasarý
    
    
    private void OnTriggerEnter(Collider other)
    {
        // Eðer vurulan nesne düþmansa
        if (other.CompareTag("Enemy"))
        {
            Health enemyHealth = other.GetComponent<Health>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(damage); // Düþmana hasar ver

            }

            Destroy(gameObject); // Mermiyi sahneden kaldýr
        }
    }
}
