using System.Collections;
using UnityEngine;

public class BossManager : MonoBehaviour
{
    public GameObject winScreen; // WinScreen paneli
    private GameObject bossInstance; // Sahnedeki boss instance'ý
    private bool bossSpawned = false; // Boss'un spawn edilip edilmediðini takip eder
    private Health playerHealth; // Player'ýn health component'i

    void Start()
    {
        // Oyuncunun Health bileþenini bul
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
        if (!bossSpawned) return; // Eðer boss henüz spawn edilmemiþse, kontrol yapma

        // Eðer oyuncu hayatta deðilse, hiçbir þey yapma
        if (playerHealth != null && !playerHealth.isPlayerAlive)
        {
            return;
        }

        // Eðer sahnede boss yoksa ve daha önce spawn edilmiþse
        if (bossInstance == null)
        {
            OpenWinScreen();
        }
    }

    public void BossSpawned(GameObject spawnedBoss)
    {
        bossInstance = spawnedBoss; // Spawn edilen boss referansý alýnýyor
        bossSpawned = true; // Boss'un spawn edildiðini iþaretle
    }

    private void OpenWinScreen()
    {
        if (winScreen != null)
        {
            StartCoroutine(OpenWinScreenWithDelay()); // Coroutine baþlat
        }
        else
        {
            Debug.LogWarning("WinScreen paneli atanmadý!");
        }
    }

    private IEnumerator OpenWinScreenWithDelay()
    {
        yield return new WaitForSeconds(2f); // 2 saniye bekle
        winScreen.SetActive(true); // WinScreen'i aktif et
        Time.timeScale = 0f; // Oyunu durdur
        Debug.Log("WinScreen opened!");
        enabled = false; // Script'i devre dýþý býrak
    }
}
