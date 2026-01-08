using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab; // prefab de l'ennemi à spawner
    [SerializeField] private float spawnInterval = 5f; // intervalle entre les spawns en secondes
    [SerializeField] private int maxEnemies = 10; // nombre maximum d'ennemis actifs
    [SerializeField] private Transform spawnPoint; // point de spawn (optionnel, sinon utilise la position du spawner)

    private float lastSpawnTime;
    private int currentEnemyCount = 0;

    void Start()
    {
        lastSpawnTime = Time.time;
    }

    void Update()
    {
        if (currentEnemyCount < maxEnemies && Time.time - lastSpawnTime > spawnInterval)
        {
            SpawnEnemy();
            lastSpawnTime = Time.time;
        }
    }

    private void SpawnEnemy()
    {
        if (enemyPrefab == null) return;

        Vector3 spawnPos = spawnPoint != null ? spawnPoint.position : transform.position;
        GameObject enemy = Instantiate(enemyPrefab, spawnPos, Quaternion.identity);
        currentEnemyCount++;

        // Assurer que l'ennemi a un Health si nécessaire
        if (!enemy.TryGetComponent<Health>(out var health))
        {
            health = enemy.AddComponent<Health>();
            health.maxHealth = 100; // valeur par défaut
        }

        // Écouter la mort pour décrémenter le compteur
        health.OnDeath.AddListener(() => OnEnemyDeath());
    }

    private void OnEnemyDeath()
    {
        currentEnemyCount--;
    }
}
