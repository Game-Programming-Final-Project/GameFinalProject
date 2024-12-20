using System.Collections;
using UnityEngine;

public class BossManager : MonoBehaviour
{
    public GameObject winScreen; // WinScreen paneli
    private GameObject bossInstance; // Sahnedeki boss instance'ý
    private bool bossSpawned = false; // Boss'un spawn edilip edilmediðini takip eder

    void Update()
    {
        if (!bossSpawned) return; // Eðer boss henüz spawn edilmemiþse, kontrol yapma

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

    // Gecikme süresiyle WinScreen açma iþlemi
    private IEnumerator OpenWinScreenWithDelay()
    {
        yield return new WaitForSeconds(2f); // 2 saniye bekle

        winScreen.SetActive(true); // WinScreen'i aktif et
        Time.timeScale = 0f; // Oyunu durdur
        Debug.Log("WinScreen opened!");

        // Script'i devre dýþý býrak
        enabled = false;
    }

}
