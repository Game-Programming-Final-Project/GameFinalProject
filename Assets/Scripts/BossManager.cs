using System.Collections;
using UnityEngine;

public class BossManager : MonoBehaviour
{
    public GameObject winScreen; 
    private GameObject bossInstance; 
    private bool bossSpawned = false; 
    private Health playerHealth; 

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
        if (!bossSpawned) return; 

        
        if (playerHealth != null && !playerHealth.isPlayerAlive)
        {
            return;
        }

        
        if (bossInstance == null)
        {
            OpenWinScreen();
        }
    }

    public void BossSpawned(GameObject spawnedBoss)
    {
        bossInstance = spawnedBoss; 
        bossSpawned = true; 
    }

    private void OpenWinScreen()
    {
        if (winScreen != null)
        {
            StartCoroutine(OpenWinScreenWithDelay()); 
        }
        else
        {
            Debug.LogWarning("WinScreen paneli atanmadý!");
        }
    }

    private IEnumerator OpenWinScreenWithDelay()
    {
        yield return new WaitForSeconds(2f); 
        winScreen.SetActive(true); 
        Time.timeScale = 0f; 
        Debug.Log("WinScreen opened!");
        enabled = false; 
    }
}
