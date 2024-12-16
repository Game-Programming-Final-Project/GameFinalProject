using UnityEngine;
using UnityEngine.UI;
public class Health : MonoBehaviour
{
    public float maxHealth = 100; // Maksimum can de�eri
    private float currentHealth; // �u anki can de�eri
    public GameObject gameOverScreen;
    public Slider healthBar; // Sa�l�k �ubu�u slider referans�

    void Start()
    {
        currentHealth = maxHealth; // Ba�lang��ta maksimum can
        if (healthBar != null)
        {
            healthBar.maxValue = maxHealth; // Slider maksimum de�eri
            healthBar.value = currentHealth; // Slider ba�lang�� de�eri
        }
    }

    // Can azaltma metodu
    public void TakeDamage(float damage)
    {
        currentHealth -= damage; // Can� azalt
        Debug.Log(gameObject.name + " took damage! Current HP: " + currentHealth);
        
        if (healthBar != null)

        {
            healthBar.value = currentHealth; // Sa�l�k �ubu�unu g�ncelle
        }
        if (currentHealth <= 0)
        {
            Die(); // Sa�l�k s�f�ra ula�t���nda �l
        }
    }

    // �lme metodu
    private void Die()
    {
        Debug.Log(gameObject.name + " has died!");
        if (gameObject.CompareTag("Player")) // E�er �len oyuncuysa
        {
            GameOver();
        }
        else
        {
            Destroy(gameObject); // E�er d��mansa yok et
        }
    }

    private void GameOver()
    {
        Time.timeScale = 0f; // Oyunu durdur
        if (gameOverScreen != null)
        {
            gameOverScreen.SetActive(true); // Game Over ekran�n� a�
        }
        else
        {
            Debug.LogWarning("GameOverScreen panel is not assigned in the Inspector!");
        }
    }

    // Can ekleme metodu (iste�e ba�l�)
    public void Heal(int amount)
    {
        currentHealth += amount;
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth; // Maksimum can� a�ma
        }
    }
}
