using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SpawnManager : MonoBehaviour
{
    public GameObject[] enemyPrefabs; // D��man t�rleri
    public GameObject bossPrefab; // Boss d��man prefab'�
    public Transform player; // Oyuncunun transform'u
    public float spawnRadius = 20f; // Spawn yap�lacak maksimum yar��ap
    public float minSpawnDistance = 5f; // Oyuncunun minimum uzakl���
    public float spawnInterval = 2f; // D��man spawn s�kl���
    public float waveDuration = 10f; // Her wave'in s�resi

    public Text waveTimerText; // UI'deki wave timer
    public GameObject marketPanel; // Market paneli
    public TextMeshProUGUI currentWaveText;
    public bool spawning = true; // Spawn i�lemi aktif mi?
    public int currentWave = 1; // �u anki wave
    public int totalWaves = 3; // Toplam wave say�s�
    private float waveTimeRemaining; // Wave i�inde kalan s�re
    public AudioSource backgroundMusic;
    public AudioSource bossMusic;
    void Start()
    {
        marketPanel.SetActive(false); // Market panelini gizle
         if (backgroundMusic != null)
        {
            backgroundMusic.Play();
        }
        StartNewWave(); // �lk wave ba�las�n
    }

    void Update()
    {
        
        if (waveTimeRemaining > 0)
        {
            waveTimeRemaining -= Time.deltaTime; // S�reyi azalt
            waveTimerText.text = $"Time: {Mathf.Ceil(waveTimeRemaining)}"; // UI'yi g�ncelle
            currentWaveText.text = "Wave:"+currentWave;
        }
        else if (spawning) // Wave s�resi bitti�inde
        {
            EndWave();
        }
    }

    public void StartNewWave()
    {
        spawning = true;
        waveTimeRemaining = waveDuration; // Yeni wave i�in s�reyi ba�lat
        if (currentWave == totalWaves) // E�er son wave'deysek
        {   
            SpawnBoss();
            backgroundMusic.Stop();
            bossMusic.Play();
           // Boss d��man� spawn et
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
            // currentWave'i prefab say�s�yla s�n�rland�r
            int maxEnemyIndex = Mathf.Min(currentWave, enemyPrefabs.Length);

            // Wave'e g�re rastgele bir d��man se�
            GameObject enemyToSpawn = enemyPrefabs[Random.Range(0, maxEnemyIndex)];

            Vector3 spawnPosition = GetRandomSpawnPosition();
            Instantiate(enemyToSpawn, spawnPosition, Quaternion.identity);

            yield return new WaitForSeconds(spawnInterval);
        }
    }


    public void SpawnBoss()
{
    spawning = false; // Boss spawn edildiği için normal spawn işlemi durduruluyor
    Vector3 bossSpawnPosition = player.position - new Vector3(0, 0, -8); // Oyuncudan uzakta bir pozisyon belirle
    GameObject spawnedBoss = Instantiate(bossPrefab, bossSpawnPosition, Quaternion.identity); // Boss'u spawn et

    Debug.Log("Boss spawned!");
   


    // BossManager'a boss'un spawn edildiğini bildir
    BossManager bossManager = FindObjectOfType<BossManager>();
    if (bossManager != null)
    {
        bossManager.BossSpawned(spawnedBoss);
    }
    else
    {
        Debug.LogWarning("BossManager bulunamadı! Boss öldüğünde WinScreen açılmayacak.");
    }
}


    void EndWave()
    {
        spawnInterval -= spawnInterval * (5/10);
        Time.timeScale = 0f;
        spawning = false;

        // Sahnedeki d��manlar� temizle
        GameObject[] remainingEnemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in remainingEnemies)
        {
            Destroy(enemy);
        }

        // Market panelini a�
        marketPanel.SetActive(true);

        // E�er t�m wave'ler bittiyse, oyun sonu i�lemleri
        if (currentWave >= totalWaves)
        {
            Debug.Log("All waves completed!");
            // Oyunun biti� mant���n� buraya ekleyebilirsiniz
        }
        else
        {
            currentWave++;
        }
    }

    public void CloseMarketPanel()
    {
        marketPanel.SetActive(false); // Market panelini kapat
        StartNewWave(); // Yeni wave ba�lat
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
