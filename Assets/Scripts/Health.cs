using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    public float maxHealth = 100; // Maksimum can de�eri
    private float currentHealth; // �u anki can de�eri
    public GameObject gameOverScreen;
    public Slider healthBar; // Sa�l�k �ubu�u slider referans�
    private FinanceManager financeManager;
    public int buyhealth = 10;
    public int BuyMaxHealth = 10;
    public int soulValueEnemy = 1;
    

    void Start()
    {
        financeManager = FindObjectOfType<FinanceManager>(); // FinanceManager referans�n� bul
        if (financeManager == null)
        {
            Debug.LogError("FinanceManager is missing in the scene!");
        }

        currentHealth = maxHealth;
        if (healthBar != null)
        {
            healthBar.maxValue = maxHealth;
            healthBar.value = currentHealth;
        }
    }
    
  

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;

        if (healthBar != null)
        {
            healthBar.value = currentHealth;
        }

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        if (gameObject.CompareTag("Player")) // E�er �len oyuncuysa
        {
            GameOver();
        }
        else
        {
            Destroy(gameObject); // D��man yok edilir
            financeManager.AddSoul(soulValueEnemy); // Soul ekle
        }
    }

    private void GameOver()
    {
        Time.timeScale = 0f;
        if (gameOverScreen != null)
        {
            gameOverScreen.SetActive(true);
        }
    }

    // Market �zellikleri
    public void BuyFullHeal(int cost)
    {
        if (financeManager != null && financeManager.SpendSoul(cost)) // Soul yeterli mi kontrol et
        {
            FullHeal();
            Debug.Log("Full Heal purchased!");
        }
        else
        {
            Debug.Log("Not enough souls to purchase Full Heal!");
        }
    }

    public void BuyAddHealth(int cost)
    {
        if (financeManager != null && financeManager.SpendSoul(cost))
        {
            Heal(buyhealth);
            Debug.Log(10 + " Health added!");
        }
        else
        {
            Debug.Log("Not enough souls to purchase Health!");
        }
    }

    public void BuyIncreaseMaxHealth(int cost)
    {
        if (financeManager != null && financeManager.SpendSoul(cost))
        {
            IncreaseMaxHealth(BuyMaxHealth);
            Debug.Log("Max Health increased by " + BuyMaxHealth + "!");
        }
        else
        {
            Debug.Log("Not enough souls to increase Max Health!");
        }
    }

    // Can ekleme ve iyile�tirme metodlar�
    public void Heal(int amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Min(currentHealth, maxHealth); // Can maksimum de�eri a�amaz
        healthBar.value = currentHealth;
    }

    public void FullHeal()
    {
        currentHealth = maxHealth;
        healthBar.value = currentHealth;
    }

    public void IncreaseMaxHealth(int amount)
    {
        maxHealth += amount;
        healthBar.maxValue = maxHealth;
        healthBar.value = currentHealth;
    }
    public float getCurrentHealth()
    {
        return currentHealth;
    }
    public float getMaxHealth()
    {
        return maxHealth;
    }
}