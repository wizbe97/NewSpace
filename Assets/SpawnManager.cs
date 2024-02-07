using UnityEngine;
using System.Collections;

public class SpawnManager : MonoBehaviour
{
    public GameObject enemyPrefab;
    public Transform[] spawnPoints;
    public float timeBetweenWaves = 5f;
    public float initialSpawnRate = 2f;
    public float maxSpawnRate = 0.5f;
    public float spawnRateDecrease = 0.1f;
    public int maxEnemiesPerWave = 10;

    private int currentWave = 0;
    private int enemiesAlive = 0;

    void Start()
    {
        StartCoroutine(SpawnWaves());
    }

    IEnumerator SpawnWaves()
    {
        yield return new WaitForSeconds(timeBetweenWaves);

        while (true)
        {
            currentWave++;
            Debug.Log("Wave " + currentWave);

            int enemiesToSpawn = GetTriangularNumber(currentWave + 1); // Start at 4 instead of 1
            enemiesToSpawn = Mathf.Min(enemiesToSpawn, maxEnemiesPerWave);
            float spawnRateAdjusted = Mathf.Max(initialSpawnRate - (spawnRateDecrease * (currentWave - 1)), maxSpawnRate);

            for (int i = 0; i < enemiesToSpawn; i++)
            {
                SpawnEnemy();
                yield return new WaitForSeconds(spawnRateAdjusted);
            }

            // Wait until all enemies from the previous wave are defeated
            while (enemiesAlive > 0)
            {
                yield return null;
            }

            yield return new WaitForSeconds(timeBetweenWaves);
        }
    }

    void SpawnEnemy()
    {
        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
        GameObject enemy = Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);
        enemy.GetComponent<EnemyHealth>().OnDeath += () => { enemiesAlive--; };
        enemiesAlive++;
    }

    int GetTriangularNumber(int n)
    {
        // Formula for triangular number: Tn = (n * (n + 1)) / 2
        return ((n - 1) * n) / 2; // Start at 4 instead of 1
    }
}
