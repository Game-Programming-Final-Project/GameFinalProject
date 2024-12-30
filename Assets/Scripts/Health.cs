using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    public Animator animator;
    public float maxHealth = 100; // Maksimum can de�eri
    private float currentHealth; // �u anki can de�eri
    public GameObject gameOverScreen;
    public Slider healthBar; // Sa�l�k �ubu�u slider referans�
    private FinanceManager financeManager;
    public int buyhealth = 10;
    public int BuyMaxHealth = 10;
    public int soulValueEnemy = 1;
    private PlayerController playerController;
    public bool isPlayerAlive = true;

    public AudioSource audioSource; // Ses kaynağı referansı
    public AudioClip damageSound;   // Hasar sesi
    public AudioClip enemyDeathSound; // Düşman öldüğünde çalınacak ses
    public AudioClip PlayerDeathSound;

    void Start()
    {
        if (audioSource == null)
   {
    audioSource = GetComponent<AudioSource>();
   }
        playerController = GetComponent<PlayerController>();
        financeManager = FindObjectOfType<FinanceManager>(); // FinanceManager referans�n� bul
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
        if (damageSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(damageSound);
        }

        currentHealth -= damage;

        currentHealth = Mathf.Max(currentHealth, 0);
        

        if (healthBar != null)
        {
            healthBar.value = currentHealth;
            playerController.UpdateHealtCounter();
        }

        if (currentHealth <= 0)
        {
            Die();
        }
    }

private void Die()
{
    if (gameObject.CompareTag("Player")) // Eğer ölen oyuncuysa
    {   if (PlayerDeathSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(PlayerDeathSound);
        }
        isPlayerAlive = false;
        GameObject[] remainingEnemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in remainingEnemies)
        {
            Destroy(enemy);
        }
        GameObject[] remainingBoss = GameObject.FindGameObjectsWithTag("Boss");
        foreach (GameObject boss in remainingBoss)
        {
            Destroy(boss);
        }

        PlayerController playerController = GetComponent<PlayerController>();
        if (playerController != null)
        {
            playerController.IsPlayerDead();
        }
        animator.ResetTrigger("RunTrigger");
        animator.ResetTrigger("ShootTrigger");
        animator.ResetTrigger("RunFastTrigger");
        animator.ResetTrigger("ReloadTrigger");
        animator.ResetTrigger("IdleTrigger");

        animator.SetTrigger("DieTrigger");
        StartCoroutine(WaitAndGameOver());
    }
    else if (gameObject.CompareTag("Enemy")) // Eğer ölen bir düşmansa
    {
        if (enemyDeathSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(enemyDeathSound); // Düşman öldüğünde ses çal
        }

        DisableEnemyPhysics();
        Destroy(gameObject, 2); // Düşman yok edilir
        financeManager.AddSoul(soulValueEnemy); // Soul ekle
        animator.SetTrigger("EnemyDeathTrigger");
    }
    else // Eğer ölen bir boss ise
    {
        if (enemyDeathSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(enemyDeathSound); // Boss öldüğünde ses çal
        }

        // Boss'un parent'ı varsa, onu da yok et
        Transform parentTransform = gameObject.transform.parent;
        if (parentTransform != null)
        {
            Destroy(parentTransform.gameObject, 2); // Parent'ı yok et
        }

        DisableEnemyPhysics();
        Destroy(gameObject, 2); // Boss yok edilir
        financeManager.AddSoul(soulValueEnemy); // Soul ekle
        animator.SetTrigger("EnemyDeathTrigger");
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

    private void DisableEnemyPhysics()
    {
        // D��man�n �arp��malar�n� devre d��� b�rak
        Collider[] colliders = GetComponentsInChildren<Collider>();
        foreach (var collider in colliders)
        {
            collider.enabled = false; // T�m collider'lar� devre d��� b�rak
        }

        // AI veya hareket sistemlerini kapatma (e�er d��man sadece fiziksel olarak durmal�ysa)
        EnemyAI enemyAI = GetComponent<EnemyAI>();
        if (enemyAI != null)
        {
            enemyAI.enabled = false;
        }
    }
    public bool GetPlayerStatus()
    {
        return isPlayerAlive;
    }
}
