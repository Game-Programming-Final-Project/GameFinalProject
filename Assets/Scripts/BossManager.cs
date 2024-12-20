using System.Collections;
using UnityEngine;

public class BossManager : MonoBehaviour
{
    public GameObject winScreen; // WinScreen paneli
    private GameObject bossInstance; // Sahnedeki boss instance'�
    private bool bossSpawned = false; // Boss'un spawn edilip edilmedi�ini takip eder

    void Update()
    {
        if (!bossSpawned) return; // E�er boss hen�z spawn edilmemi�se, kontrol yapma

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

    // Gecikme s�resiyle WinScreen a�ma i�lemi
    private IEnumerator OpenWinScreenWithDelay()
    {
        yield return new WaitForSeconds(2f); // 2 saniye bekle

        winScreen.SetActive(true); // WinScreen'i aktif et
        Time.timeScale = 0f; // Oyunu durdur
        Debug.Log("WinScreen opened!");

        // Script'i devre d��� b�rak
        enabled = false;
    }

}
