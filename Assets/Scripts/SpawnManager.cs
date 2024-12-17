using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject[] enemyPrefabs; // D��man t�rleri (4 farkl� prefab)
    public Transform player; // Oyuncunun transform'u
    public float spawnRadius = 20f; // Spawn yap�lacak maksimum yar��ap
    public float minSpawnDistance = 5f; // Oyuncunun minimum uzakl���
    public float spawnInterval = 2f; // D��manlar�n spawnlanma s�kl��� (saniye cinsinden)

    private bool spawning = true; // Spawn i�lemi aktif mi?

    void Start()
    {
        // Spawn i�lemine ba�la
        StartCoroutine(SpawnEnemies());
    }

    IEnumerator SpawnEnemies()
    {
        while (spawning)
        {
            // Rastgele bir d��man t�r�n� se�
            GameObject enemyToSpawn = enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];

            // Oyuncudan uygun uzakl�kta rastgele bir spawn pozisyonu al
            Vector3 spawnPosition = GetRandomSpawnPosition();

            // D��man� instantiate et
            Instantiate(enemyToSpawn, spawnPosition, Quaternion.identity);

            // Spawn aral���n� bekle
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    Vector3 GetRandomSpawnPosition()
    {
        Vector3 randomPosition;

        // Rastgele bir pozisyon se� ve oyuncunun minimum mesafesini kontrol et
        do
        {
            Vector2 randomCircle = Random.insideUnitCircle * spawnRadius; // 2D rastgele pozisyon
            randomPosition = new Vector3(randomCircle.x, 0, randomCircle.y); // 3D pozisyon
            randomPosition += player.position; // Pozisyonu oyuncu merkezli yap
        } while (Vector3.Distance(player.position, randomPosition) < minSpawnDistance);

        return randomPosition;
    }
}
