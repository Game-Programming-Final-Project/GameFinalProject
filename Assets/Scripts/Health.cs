using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    public Animator animator;
    public float maxHealth = 100; // Maksimum can deðeri
    private float currentHealth; // Þu anki can deðeri
    public GameObject gameOverScreen;
    public Slider healthBar; // Saðlýk çubuðu slider referansý
    private FinanceManager financeManager;
    public int buyhealth = 10;
    public int BuyMaxHealth = 10;
    public int soulValueEnemy = 1;

    void Start()
    {
        financeManager = FindObjectOfType<FinanceManager>(); // FinanceManager referansýný bul
        if (financeManager == null)
        {
            Debug.LogError("FinanceManager is missing in the scene!");
        }

        currentHealth = maxHealth;
        if (healthBar != null)
        {
            healthBar.maxValue = maxHealth;
            healthBar.minValue = 0;
            healthBar.value = currentHealth;
        }
    }


    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Max(currentHealth, 0);

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
        if (gameObject.CompareTag("Player")) // Eðer ölen oyuncuysa
        {
            PlayerController playerController = GetComponent<PlayerController>();
            if (playerController != null)
            {
                playerController.IsPlayerDead();
            }
            animator.ResetTrigger("RunTrigger");
            animator.ResetTrigger("ShootTrigger");
            animator.ResetTrigger("RunFastTrigger");
            animator.SetTrigger("IdleTrigger");
            
            animator.SetTrigger("DieTrigger");
            StartCoroutine(WaitAndGameOver());
        }
        else
        {
            Destroy(gameObject,2); // Düþman yok edilir
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
    private IEnumerator WaitAndGameOver()
    {
        yield return new WaitForSeconds(2f);
        GameOver();
    }

    // Market Özellikleri
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

    // Can ekleme ve iyileþtirme metodlarý
    public void Heal(int amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Min(currentHealth, maxHealth); // Can maksimum deðeri aþamaz
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
