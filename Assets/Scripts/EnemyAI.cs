using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public Transform player; // Reference to the player's transform
    public float moveSpeed = 3f; // Speed at which the enemy moves towards the player
    public float damage = 10f;  // D��man�n verece�i hasar
    private bool isPlayerInRange = false; // Oyuncu trigger alan�nda m�?
    private float damageCooldown = 1f; // 1 saniye aral�kla hasar verme
    private float lastDamageTime = 0f; // Son hasar verme zaman�

    void Start()
    {
        if (player == null) // E�er player atanmad�ysa
        {
            GameObject playerObject = GameObject.FindWithTag("Player");
            if (playerObject != null)
            {
                player = playerObject.transform; // Oyuncunun Transform bile�enine eri�
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

            // E�er oyuncu trigger alan�nda ve yeterince zaman ge�mi�se hasar ver
            if (isPlayerInRange && Time.time - lastDamageTime >= damageCooldown)
            {
                Health playerHealth = player.GetComponent<Health>();
                if (playerHealth != null)
                {
                    playerHealth.TakeDamage(damage); // Oyuncuya hasar ver
                    lastDamageTime = Time.time; // Son hasar zaman�n� g�ncelle
                }
            }
        }
    }

    void MoveTowardsPlayer()
    {
        // Oyuncuya do�ru hareket y�n�n� hesapla
        Vector3 direction = (player.position - transform.position).normalized;
        direction.y = 0; // D��man�n dikey d�zlemde hareket etmesini engelle

        // D��man� oyuncuya do�ru hareket ettir
        transform.position += direction * moveSpeed * Time.deltaTime;
    }

    void RotateTowardsPlayer()
    {
        // Oyuncuya do�ru y�n� hesapla
        Vector3 direction = (player.position - transform.position).normalized;
        direction.y = 0; // D�n��� sadece XZ d�zleminde tut

        // Oyuncuya d�nme rotas�n� hesapla
        Quaternion targetRotation = Quaternion.LookRotation(direction);

        // D��man� yava��a oyuncuya do�ru d�nd�r
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * 10f);
    }

    // Oyuncu ile temas halinde hasar ver
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = true; // Oyuncu trigger alan�na girdi
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false; // Oyuncu trigger alan�ndan ��kt�
        }
    }
}
