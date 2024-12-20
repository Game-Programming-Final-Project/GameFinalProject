using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public Transform player; // Reference to the player's transform
    public float moveSpeed = 3f; // Speed at which the enemy moves towards the player
    public float damage = 10f;  // Düþmanýn vereceði hasar
    private bool isPlayerInRange = false; // Oyuncu trigger alanýnda mý?
    private float damageCooldown = 1f; // 1 saniye aralýkla hasar verme
    private float lastDamageTime = 0f; // Son hasar verme zamaný

    void Start()
    {
        if (player == null) // Eðer player atanmadýysa
        {
            GameObject playerObject = GameObject.FindWithTag("Player");
            if (playerObject != null)
            {
                player = playerObject.transform; // Oyuncunun Transform bileþenine eriþ
            }
            else
            {
                Debug.LogWarning("Player object not found in the scene. Make sure the Player has the correct tag.");
            }
        }
    }

    void Update()
    {
        if (player != null)
        {
            MoveTowardsPlayer();
            RotateTowardsPlayer();

            // Eðer oyuncu trigger alanýnda ve yeterince zaman geçmiþse hasar ver
            if (isPlayerInRange && Time.time - lastDamageTime >= damageCooldown)
            {
                Health playerHealth = player.GetComponent<Health>();
                if (playerHealth != null)
                {
                    playerHealth.TakeDamage(damage); // Oyuncuya hasar ver
                    lastDamageTime = Time.time; // Son hasar zamanýný güncelle
                }
            }
        }
    }

    void MoveTowardsPlayer()
    {
        // Oyuncuya doðru hareket yönünü hesapla
        Vector3 direction = (player.position - transform.position).normalized;
        direction.y = 0; // Düþmanýn dikey düzlemde hareket etmesini engelle

        // Düþmaný oyuncuya doðru hareket ettir
        transform.position += direction * moveSpeed * Time.deltaTime;
    }

    void RotateTowardsPlayer()
    {
        // Oyuncuya doðru yönü hesapla
        Vector3 direction = (player.position - transform.position).normalized;
        direction.y = 0; // Dönüþü sadece XZ düzleminde tut

        // Oyuncuya dönme rotasýný hesapla
        Quaternion targetRotation = Quaternion.LookRotation(direction);

        // Düþmaný yavaþça oyuncuya doðru döndür
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * 10f);
    }

    // Oyuncu ile temas halinde hasar ver
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = true; // Oyuncu trigger alanýna girdi
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false; // Oyuncu trigger alanýndan çýktý
        }
    }
}
