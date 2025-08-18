using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    public GameObject normalEnemyPrefab;
    public GameObject dasherEnemyPrefab;
    public GameObject shooterEnemyPrefab;
    public Vector3 playerPosition;
    public float spawnRadius = 8f;
    public float timeBetweenWaves = 3f;
    public int minEnemiesPerWave = 12;
    public int maxEnemiesPerWave = 16;

    [Header("Spawn Weights (Higher = More Common)")]
    public int normalWeight = 8;
    public int dasherWeight = 2;
    public int shooterWeight = 2;

    private List<GameObject> currentEnemies = new List<GameObject>();
    private bool spawning = false;
    private Coroutine waveRoutine;


    //subscribe events
    private void OnEnable()
    {
        PlayerPositionNotifier.OnPlayerPositionChanged += UpdatePlayerPosition;
    }

    private void OnDisable()
    {
        PlayerPositionNotifier.OnPlayerPositionChanged -= UpdatePlayerPosition;
    }

    private void UpdatePlayerPosition(Vector3 newPosition)
    {
        playerPosition = newPosition;
    }

    void Start()
    {
        // Optionally start automatically
        // StartWaves();
    }

    public void StartWaves()
    {
        if (!spawning)
        {
            spawning = true;
            waveRoutine = StartCoroutine(WaveLoop());
        }
    }

    public void StopWaves()
    {
        spawning = false;
        if (waveRoutine != null)
            StopCoroutine(waveRoutine);
    }

    public void KillAllEnemies()
    {
        foreach (var enemy in currentEnemies)
        {
            if (enemy != null)
                Destroy(enemy);
        }
        currentEnemies.Clear();
    }

    private IEnumerator WaveLoop()
    {
        while (spawning)
        {
            int enemyCount = Random.Range(minEnemiesPerWave, maxEnemiesPerWave + 1);
            SpawnWave(enemyCount);

            // Wait until all enemies are dead
            yield return new WaitUntil(() => currentEnemies.TrueForAll(e => e == null));

            currentEnemies.Clear();

            // Wait before next wave
            yield return new WaitForSeconds(timeBetweenWaves);
        }
    }

    private void SpawnWave(int count)
    {
        if (playerPosition == null) return;

        float angleStep = 360f / count;
        for (int i = 0; i < count; i++)
        {
            float angle = i * angleStep * Mathf.Deg2Rad;
            Vector3 spawnPos = playerPosition + new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0) * spawnRadius;
            GameObject prefab = GetRandomEnemyPrefab();
            GameObject enemy = Instantiate(prefab, spawnPos, Quaternion.identity);

            // Assign player reference to any script that needs it
            var enemyController = enemy.GetComponent<EnemyController>();
            if (enemyController != null)
                enemyController.playerPosition = playerPosition;

            var enemyDasher = enemy.GetComponent<EnemyDasher>();
            if (enemyDasher != null)
                enemyDasher.playerPosition = playerPosition;

            var enemyShooter = enemy.GetComponent<EnemyShooter>();
            if (enemyShooter != null)
                enemyShooter.playerPosition = playerPosition;

            currentEnemies.Add(enemy);
        }
    }

    private GameObject GetRandomEnemyPrefab()
    {
        int totalWeight = normalWeight + dasherWeight + shooterWeight;
        int rand = Random.Range(0, totalWeight);
        if (rand < normalWeight)
            return normalEnemyPrefab;
        else if (rand < normalWeight + dasherWeight)
            return dasherEnemyPrefab;
        else
            return shooterEnemyPrefab;
    }
}