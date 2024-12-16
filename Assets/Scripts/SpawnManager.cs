using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject[] enemyPrefabs; // Düþman türleri (4 farklý prefab)
    public Transform player; // Oyuncunun transform'u
    public float spawnRadius = 20f; // Spawn yapýlacak maksimum yarýçap
    public float minSpawnDistance = 5f; // Oyuncunun minimum uzaklýðý
    public float spawnInterval = 2f; // Düþmanlarýn spawnlanma sýklýðý (saniye cinsinden)

    private bool spawning = true; // Spawn iþlemi aktif mi?

    void Start()
    {
        // Spawn iþlemine baþla
        StartCoroutine(SpawnEnemies());
    }

    IEnumerator SpawnEnemies()
    {
        while (spawning)
        {
            // Rastgele bir düþman türünü seç
            GameObject enemyToSpawn = enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];

            // Oyuncudan uygun uzaklýkta rastgele bir spawn pozisyonu al
            Vector3 spawnPosition = GetRandomSpawnPosition();

            // Düþmaný instantiate et
            Instantiate(enemyToSpawn, spawnPosition, Quaternion.identity);

            // Spawn aralýðýný bekle
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    Vector3 GetRandomSpawnPosition()
    {
        Vector3 randomPosition;

        // Rastgele bir pozisyon seç ve oyuncunun minimum mesafesini kontrol et
        do
        {
            Vector2 randomCircle = Random.insideUnitCircle * spawnRadius; // 2D rastgele pozisyon
            randomPosition = new Vector3(randomCircle.x, 0, randomCircle.y); // 3D pozisyon
            randomPosition += player.position; // Pozisyonu oyuncu merkezli yap
        } while (Vector3.Distance(player.position, randomPosition) < minSpawnDistance);

        return randomPosition;
    }
}
