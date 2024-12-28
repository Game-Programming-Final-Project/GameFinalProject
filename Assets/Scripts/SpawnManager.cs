using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SpawnManager : MonoBehaviour
{
    public GameObject[] enemyPrefabs; // Düþman türleri
    public GameObject bossPrefab; // Boss düþman prefab'ý
    public Transform player; // Oyuncunun transform'u
    public float spawnRadius = 20f; // Spawn yapýlacak maksimum yarýçap
    public float minSpawnDistance = 5f; // Oyuncunun minimum uzaklýðý
    public float spawnInterval = 2f; // Düþman spawn sýklýðý
    public float waveDuration = 30f; // Her wave'in süresi

    public Text waveTimerText; // UI'deki wave timer
    public GameObject marketPanel; // Market paneli
    public TextMeshProUGUI currentWaveText;
    public bool spawning = true; // Spawn iþlemi aktif mi?
    public int currentWave = 1; // Þu anki wave
    public int totalWaves = 3; // Toplam wave sayýsý
    private float waveTimeRemaining; // Wave içinde kalan süre

    void Start()
    {
        marketPanel.SetActive(false); // Market panelini gizle
        StartNewWave(); // Ýlk wave baþlasýn
    }

    void Update()
    {
        
        if (waveTimeRemaining > 0)
        {
            waveTimeRemaining -= Time.deltaTime; // Süreyi azalt
            waveTimerText.text = $"Time: {Mathf.Ceil(waveTimeRemaining)}"; // UI'yi güncelle
            currentWaveText.text = "Wave:"+currentWave;
        }
        else if (spawning) // Wave süresi bittiðinde
        {
            EndWave();
        }
    }

    public void StartNewWave()
    {
        spawning = true;
        waveTimeRemaining = waveDuration; // Yeni wave için süreyi baþlat
        if (currentWave == totalWaves) // Eðer son wave'deysek
        {
            SpawnBoss(); // Boss düþmaný spawn et
        }
        else
        {
            StartCoroutine(SpawnEnemies());
        }
    }

    IEnumerator SpawnEnemies()
    {
        while (spawning)
        {
            // currentWave'i prefab sayýsýyla sýnýrlandýr
            int maxEnemyIndex = Mathf.Min(currentWave, enemyPrefabs.Length);

            // Wave'e göre rastgele bir düþman seç
            GameObject enemyToSpawn = enemyPrefabs[Random.Range(0, maxEnemyIndex)];

            Vector3 spawnPosition = GetRandomSpawnPosition();
            Instantiate(enemyToSpawn, spawnPosition, Quaternion.identity);

            yield return new WaitForSeconds(spawnInterval);
        }
    }


    public void SpawnBoss()
    {
        spawning = false; // Boss spawn edildiði için normal spawn iþlemi durduruluyor
        Vector3 bossSpawnPosition = GetRandomSpawnPosition(); // Oyuncudan uzakta bir pozisyon belirle
        GameObject spawnedBoss = Instantiate(bossPrefab, bossSpawnPosition, Quaternion.identity); // Boss'u spawn et

        Debug.Log("Boss spawned!");

        // BossManager'a boss'un spawn edildiðini bildir
        BossManager bossManager = FindObjectOfType<BossManager>();
        if (bossManager != null)
        {
            bossManager.BossSpawned(spawnedBoss);
        }
        else
        {
            Debug.LogWarning("BossManager bulunamadý! Boss öldüðünde WinScreen açýlmayacak.");
        }
    }


    void EndWave()
    {
        spawnInterval -= spawnInterval * (5/10);
        Time.timeScale = 0f;
        spawning = false;

        // Sahnedeki düþmanlarý temizle
        GameObject[] remainingEnemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in remainingEnemies)
        {
            Destroy(enemy);
        }

        // Market panelini aç
        marketPanel.SetActive(true);

        // Eðer tüm wave'ler bittiyse, oyun sonu iþlemleri
        if (currentWave >= totalWaves)
        {
            Debug.Log("All waves completed!");
            // Oyunun bitiþ mantýðýný buraya ekleyebilirsiniz
        }
        else
        {
            currentWave++;
        }
    }

    public void CloseMarketPanel()
    {
        marketPanel.SetActive(false); // Market panelini kapat
        StartNewWave(); // Yeni wave baþlat
        Time.timeScale = 1f;
    }

    Vector3 GetRandomSpawnPosition()
    {
        Vector3 randomPosition;
        do
        {
            Vector2 randomCircle = Random.insideUnitCircle * spawnRadius; // Rastgele 2D pozisyon
            randomPosition = new Vector3(randomCircle.x, 0, randomCircle.y) + player.position; // 3D pozisyon
        } while (Vector3.Distance(player.position, randomPosition) < minSpawnDistance);

        return randomPosition;
    }
}
