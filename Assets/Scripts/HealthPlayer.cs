using UnityEngine;
using UnityEngine.UI;
public class Health : MonoBehaviour
{
    public float maxHealth = 100; // Maksimum can deðeri
    private float currentHealth; // Þu anki can deðeri
    public GameObject gameOverScreen;
    public Slider healthBar; // Saðlýk çubuðu slider referansý

    void Start()
    {
        currentHealth = maxHealth; // Baþlangýçta maksimum can
        if (healthBar != null)
        {
            healthBar.maxValue = maxHealth; // Slider maksimum deðeri
            healthBar.value = currentHealth; // Slider baþlangýç deðeri
        }
    }

    // Can azaltma metodu
    public void TakeDamage(float damage)
    {
        currentHealth -= damage; // Caný azalt
        Debug.Log(gameObject.name + " took damage! Current HP: " + currentHealth);
        
        if (healthBar != null)

        {
            healthBar.value = currentHealth; // Saðlýk çubuðunu güncelle
        }
        if (currentHealth <= 0)
        {
            Die(); // Saðlýk sýfýra ulaþtýðýnda öl
        }
    }

    // Ölme metodu
    private void Die()
    {
        Debug.Log(gameObject.name + " has died!");
        if (gameObject.CompareTag("Player")) // Eðer ölen oyuncuysa
        {
            GameOver();
        }
        else
        {
            Destroy(gameObject); // Eðer düþmansa yok et
        }
    }

    private void GameOver()
    {
        Time.timeScale = 0f; // Oyunu durdur
        if (gameOverScreen != null)
        {
            gameOverScreen.SetActive(true); // Game Over ekranýný aç
        }
        else
        {
            Debug.LogWarning("GameOverScreen panel is not assigned in the Inspector!");
        }
    }

    // Can ekleme metodu (isteðe baðlý)
    public void Heal(int amount)
    {
        currentHealth += amount;
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth; // Maksimum caný aþma
        }
    }
}
