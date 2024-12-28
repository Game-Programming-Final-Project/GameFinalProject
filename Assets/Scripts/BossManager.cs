using System.Collections;
using UnityEngine;

public class BossManager : MonoBehaviour
{
    public GameObject winScreen; // WinScreen paneli
    private GameObject bossInstance; // Sahnedeki boss instance'�
    private bool bossSpawned = false; // Boss'un spawn edilip edilmedi�ini takip eder
    private Health playerHealth; // Player'�n health component'i

    void Start()
    {
        // Oyuncunun Health bile�enini bul
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            playerHealth = player.GetComponent<Health>();
        }

        if (playerHealth == null)
        {
            Debug.LogWarning("Player Health component not found. WinScreen logic may not work correctly.");
        }
    }

    void Update()
    {
        if (!bossSpawned) return; // E�er boss hen�z spawn edilmemi�se, kontrol yapma

        // E�er oyuncu hayatta de�ilse, hi�bir �ey yapma
        if (playerHealth != null && !playerHealth.isPlayerAlive)
        {
            return;
        }

        // E�er sahnede boss yoksa ve daha �nce spawn edilmi�se
        if (bossInstance == null)
        {
            OpenWinScreen();
        }
    }

    public void BossSpawned(GameObject spawnedBoss)
    {
        bossInstance = spawnedBoss; // Spawn edilen boss referans� al�n�yor
        bossSpawned = true; // Boss'un spawn edildi�ini i�aretle
    }

    private void OpenWinScreen()
    {
        if (winScreen != null)
        {
            StartCoroutine(OpenWinScreenWithDelay()); // Coroutine ba�lat
        }
        else
        {
            Debug.LogWarning("WinScreen paneli atanmad�!");
        }
    }

    private IEnumerator OpenWinScreenWithDelay()
    {
        yield return new WaitForSeconds(2f); // 2 saniye bekle
        winScreen.SetActive(true); // WinScreen'i aktif et
        Time.timeScale = 0f; // Oyunu durdur
        Debug.Log("WinScreen opened!");
        enabled = false; // Script'i devre d��� b�rak
    }
}
