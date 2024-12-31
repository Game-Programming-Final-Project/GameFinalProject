using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SpawnManager : MonoBehaviour
{
    public GameObject[] enemyPrefabs; 
    public GameObject bossPrefab; 
    public Transform player; 
    public float spawnRadius = 20f; 
    public float minSpawnDistance = 5f; 
    public float spawnInterval = 2f;
    public float waveDuration = 10f;

    public Text waveTimerText; 
    public GameObject marketPanel; 
    public TextMeshProUGUI currentWaveText;
    public bool spawning = true; 
    public int currentWave = 1; 
    public int totalWaves = 3; 
    private float waveTimeRemaining; 

    // Müzik ile ilgili alanlar
    public Slider volumeSlider;
    public AudioClip backgroundMusic; 
    public AudioClip bossMusic; 
    private AudioSource audioSource; 

    void Start()
    {
        marketPanel.SetActive(false);
        StartNewWave(); 

        
        audioSource = GetComponent<AudioSource>();
        if (audioSource != null && backgroundMusic != null)
        {
            audioSource.clip = backgroundMusic;
            audioSource.loop = true;
            audioSource.Play();
        }
        if (volumeSlider != null)
        {
            volumeSlider.value = audioSource.volume;
            volumeSlider.onValueChanged.AddListener(UpdateVolume);
        }
        else
        {
            Debug.LogWarning("Volume slider atanmadý.");
        }
    }

    void Update()
    {
        if (waveTimeRemaining > 0)
        {
            waveTimeRemaining -= Time.deltaTime; 
            waveTimerText.text = $"Time: {Mathf.Ceil(waveTimeRemaining)}"; 
            currentWaveText.text = "Wave:" + currentWave;
        }
        else if (spawning) 
        {
            EndWave();
        }
    }

    public void StartNewWave()
    {
        spawning = true;
        waveTimeRemaining = waveDuration;
        if (currentWave == totalWaves) 
        {
            SpawnBoss(); 
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
            
            int maxEnemyIndex = Mathf.Min(currentWave, enemyPrefabs.Length);

           
            GameObject enemyToSpawn = enemyPrefabs[Random.Range(0, maxEnemyIndex)];

            Vector3 spawnPosition = GetRandomSpawnPosition();
            Instantiate(enemyToSpawn, spawnPosition, Quaternion.identity);

            yield return new WaitForSeconds(spawnInterval);
        }
    }

    public void SpawnBoss()
    {
        spawning = false; 
        Vector3 bossSpawnPosition = player.position - new Vector3(0, 0, -8); 
        GameObject spawnedBoss = Instantiate(bossPrefab, bossSpawnPosition, Quaternion.identity); 

        Debug.Log("Boss spawned!");

        
        if (audioSource != null && bossMusic != null)
        {
            audioSource.Stop(); 
            audioSource.clip = bossMusic;
            audioSource.loop = true;
            audioSource.Play();
        }

        
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
        spawnInterval -= spawnInterval * (5 / 10);
        Time.timeScale = 0f;
        spawning = false;

        
        GameObject[] remainingEnemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in remainingEnemies)
        {
            Destroy(enemy);
        }

        
        marketPanel.SetActive(true);

        
        if (currentWave >= totalWaves)
        {
            Debug.Log("All waves completed!");
        }
        else
        {
            currentWave++;
        }
    }

    public void CloseMarketPanel()
    {
        marketPanel.SetActive(false);
        StartNewWave(); 
        Time.timeScale = 1f;
    }

    Vector3 GetRandomSpawnPosition()
    {
        Vector3 randomPosition;
        do
        {
            Vector2 randomCircle = Random.insideUnitCircle * spawnRadius; 
            randomPosition = new Vector3(randomCircle.x, 0, randomCircle.y) + player.position; 
        } while (Vector3.Distance(player.position, randomPosition) < minSpawnDistance);

        return randomPosition;
    }
    public void UpdateVolume(float value)
    {
        if (audioSource != null)
        {
            audioSource.volume = value;
        }
    }
}
