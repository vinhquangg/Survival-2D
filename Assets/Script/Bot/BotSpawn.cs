using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WaveConfig
{
    public string waveName = "Wave";
    public float waveDuration = 30f;
    public float spawnInterval = 1f;
    public int enemiesPerWave = 20;
    public float difficultyMultiplier = 1f;
}

public class BotSpawn : MonoBehaviour
{
    public enum SpawnMode { Area, Edge }

    [Header("Spawn Settings")]
    public string botPoolTag = "Bot";
    public SpawnMode spawnMode = SpawnMode.Edge;
    public float edgeSpawnChance = 0.7f;
    public float spawnMargin = 1f;
    
    [Header("Wave Settings")]
    public WaveConfig[] waveConfigs;
    public float globalDifficultyIncrease = 0.1f;
    public int maxWaves = 10;
    
    [Header("Camera Settings")]
    public Camera targetCamera;
    public bool useCameraBounds = true;
    
    // Private variables
    private int currentWave = 0;
    private float waveTimer = 0f;
    private float nextSpawnTime = 0f;
    private int enemiesSpawned = 0;
    private bool isWaveActive = false;
    private List<GameObject> activeEnemies = new List<GameObject>();
    private WaveConfig currentWaveConfig;
    
    // Events
    public System.Action<int> OnWaveStart;
    public System.Action<int> OnWaveComplete;
    public System.Action OnAllWavesComplete;
    public System.Action<float> OnWaveTimeUpdate;
    
    private void Start()
    {
        InitializeSpawnSystem();
    }
    
    private void Update()
    {
        if (!isWaveActive) return;
        
        UpdateWaveTimer();
        HandleSpawning();
        CheckWaveEnd();
    }
    
    private void InitializeSpawnSystem()
    {
        if (PoolingManager.Instance == null)
        {
            Debug.LogError("[BotSpawn] PoolingManager.Instance is null!");
            return;
        }
        
        if (targetCamera == null)
            targetCamera = Camera.main;
            
        if (waveConfigs == null || waveConfigs.Length == 0)
        {
            CreateDefaultWaveConfigs();
        }
        
        StartNextWave();
    }
    
    private void CreateDefaultWaveConfigs()
    {
        waveConfigs = new WaveConfig[maxWaves];
        for (int i = 0; i < maxWaves; i++)
        {
            waveConfigs[i] = new WaveConfig
            {
                waveName = $"Wave {i + 1}",
                waveDuration = 30f + (i * 5f),
                spawnInterval = Mathf.Max(0.5f, 1.5f - (i * 0.1f)),
                enemiesPerWave = 15 + (i * 5),
                difficultyMultiplier = 1f + (i * globalDifficultyIncrease)
            };
        }
    }
    
    private void UpdateWaveTimer()
    {
        waveTimer += Time.deltaTime;
        OnWaveTimeUpdate?.Invoke(waveTimer);
    }
    
    private void HandleSpawning()
    {
        if (Time.time >= nextSpawnTime && enemiesSpawned < currentWaveConfig.enemiesPerWave)
        {
            SpawnEnemy();
            nextSpawnTime = Time.time + currentWaveConfig.spawnInterval;
        }
    }
    
    private void CheckWaveEnd()
    {
        if (waveTimer >= currentWaveConfig.waveDuration)
        {
            EndWave();
        }
    }
    
    private void StartNextWave()
    {
        if (currentWave >= maxWaves)
        {
            Debug.Log("[BotSpawn] All waves completed!");
            OnAllWavesComplete?.Invoke();
            return;
        }
        
        currentWave++;
        currentWaveConfig = waveConfigs[currentWave - 1];
        waveTimer = 0f;
        enemiesSpawned = 0;
        isWaveActive = true;
        
        Debug.Log($"[BotSpawn] Starting Wave {currentWave}: {currentWaveConfig.waveName}");
        OnWaveStart?.Invoke(currentWave);
        
        nextSpawnTime = Time.time + currentWaveConfig.spawnInterval;
    }
    
    private void EndWave()
    {
        isWaveActive = false;
        Debug.Log($"[BotSpawn] Wave {currentWave} completed!");
        
        OnWaveComplete?.Invoke(currentWave);
        
        StartCoroutine(WaitForNextWave());
    }
    
    private IEnumerator WaitForNextWave()
    {
        yield return new WaitForSeconds(3f);
        StartNextWave();
    }
    
    private void SpawnEnemy()
    {
        if (!PoolingManager.Instance.HasAvailable(botPoolTag))
        {
            Debug.LogWarning("[BotSpawn] No enemies available in pool!");
            return;
        }
        
        Vector2 spawnPos = GetSpawnPosition();
        GameObject enemy = PoolingManager.Instance.GetFromPool(botPoolTag, spawnPos, Quaternion.identity);
        
        if (enemy != null)
        {
            ApplyDifficultyScaling(enemy);
            activeEnemies.Add(enemy);
            enemiesSpawned++;
        }
    }
    
    private Vector2 GetSpawnPosition()
    {
        if (spawnMode == SpawnMode.Edge || Random.Range(0f, 1f) < edgeSpawnChance)
        {
            return GetRandomEdgePosition();
        }
        else
        {
            return GetRandomAreaPosition();
        }
    }
    
    private Vector2 GetRandomEdgePosition()
    {
        Rect cameraRect = GetCameraRect();
        
        int edge = Random.Range(0, 4); // 0: Top, 1: Right, 2: Bottom, 3: Left
        
        float x, y;
        switch (edge)
        {
            case 0: // Top
                x = Random.Range(cameraRect.xMin + spawnMargin, cameraRect.xMax - spawnMargin);
                y = cameraRect.yMax + spawnMargin;
                break;
            case 1: // Right
                x = cameraRect.xMax + spawnMargin;
                y = Random.Range(cameraRect.yMin + spawnMargin, cameraRect.yMax - spawnMargin);
                break;
            case 2: // Bottom
                x = Random.Range(cameraRect.xMin + spawnMargin, cameraRect.xMax - spawnMargin);
                y = cameraRect.yMin - spawnMargin;
                break;
            case 3: // Left
                x = cameraRect.xMin - spawnMargin;
                y = Random.Range(cameraRect.yMin + spawnMargin, cameraRect.yMax - spawnMargin);
                break;
            default:
                x = cameraRect.xMin - spawnMargin;
                y = cameraRect.yMax + spawnMargin;
                break;
        }
        
        return new Vector2(x, y);
    }
    
    private Vector2 GetRandomAreaPosition()
    {
        if (useCameraBounds && targetCamera != null)
        {
            Rect cameraRect = GetCameraRect();
            float x = Random.Range(cameraRect.xMin + spawnMargin, cameraRect.xMax - spawnMargin);
            float y = Random.Range(cameraRect.yMin + spawnMargin, cameraRect.yMax - spawnMargin);
            return new Vector2(x, y);
        }
        else
        {
            // Fallback to simple area spawning
            float x = Random.Range(-10f, 10f);
            float y = Random.Range(-10f, 10f);
            return new Vector2(x, y);
        }
    }
    
    private Rect GetCameraRect()
    {
        if (targetCamera == null) return new Rect(-10, -10, 20, 20);
        
        float height = targetCamera.orthographicSize * 2;
        float width = height * targetCamera.aspect;
        Vector3 center = targetCamera.transform.position;
        
        return new Rect(center.x - width / 2, center.y - height / 2, width, height);
    }
    
    private void ApplyDifficultyScaling(GameObject enemy)
    {
        float difficulty = currentWaveConfig.difficultyMultiplier;
        
        // Apply to BotControlling2D
        BotControlling2D botController = enemy.GetComponent<BotControlling2D>();
        if (botController != null)
        {
            botController.maxHealth = Mathf.RoundToInt(botController.maxHealth * difficulty);
        }
        
        // Apply to MoveToPlayerBehavior
        MoveToPlayerBehavior moveBehavior = enemy.GetComponent<MoveToPlayerBehavior>();
        if (moveBehavior != null)
        {
            moveBehavior.moveSpeed *= difficulty;
        }
    }
    
    public void OnEnemyDestroyed(GameObject enemy)
    {
        if (activeEnemies.Contains(enemy))
        {
            activeEnemies.Remove(enemy);
        }
    }
    
    // Public methods
    public int GetCurrentWave() => currentWave;
    public float GetWaveProgress() => waveTimer / currentWaveConfig.waveDuration;
    public int GetActiveEnemyCount() => activeEnemies.Count;
    public bool IsWaveActive() => isWaveActive;
    public string GetCurrentWaveName() => currentWaveConfig?.waveName ?? "Unknown Wave";
    
    // Control methods
    public void StartSpawning()
    {
        if (!isWaveActive)
        {
            StartNextWave();
        }
    }
    
    public void StopSpawning()
    {
        isWaveActive = false;
    }
    
    public void SkipToNextWave()
    {
        if (isWaveActive)
        {
            EndWave();
        }
    }
    
    public void ResetWaves()
    {
        currentWave = 0;
        waveTimer = 0f;
        enemiesSpawned = 0;
        isWaveActive = false;
        activeEnemies.Clear();
    }
    
    // Debug methods
    public void DebugSpawnPositions(int count = 5)
    {
        Debug.Log($"[BotSpawn] Testing {count} spawn positions:");
        for (int i = 0; i < count; i++)
        {
            Vector2 pos = GetSpawnPosition();
            Debug.Log($"[BotSpawn] Spawn {i + 1}: {pos}");
        }
    }
    
    private void OnDrawGizmosSelected()
    {
        if (targetCamera == null) return;
        
        Rect cameraRect = GetCameraRect();
        
        // Draw camera bounds
        Gizmos.color = Color.yellow;
        Vector3 topLeft = new Vector3(cameraRect.xMin, cameraRect.yMax, 0);
        Vector3 topRight = new Vector3(cameraRect.xMax, cameraRect.yMax, 0);
        Vector3 bottomLeft = new Vector3(cameraRect.xMin, cameraRect.yMin, 0);
        Vector3 bottomRight = new Vector3(cameraRect.xMax, cameraRect.yMin, 0);
        
        Gizmos.DrawLine(topLeft, topRight);
        Gizmos.DrawLine(topRight, bottomRight);
        Gizmos.DrawLine(bottomRight, bottomLeft);
        Gizmos.DrawLine(bottomLeft, topLeft);
        
        // Draw spawn margin
        Gizmos.color = Color.red;
        Vector3 marginTopLeft = new Vector3(cameraRect.xMin - spawnMargin, cameraRect.yMax + spawnMargin, 0);
        Vector3 marginTopRight = new Vector3(cameraRect.xMax + spawnMargin, cameraRect.yMax + spawnMargin, 0);
        Vector3 marginBottomLeft = new Vector3(cameraRect.xMin - spawnMargin, cameraRect.yMin - spawnMargin, 0);
        Vector3 marginBottomRight = new Vector3(cameraRect.xMax + spawnMargin, cameraRect.yMin - spawnMargin, 0);
        
        Gizmos.DrawLine(marginTopLeft, marginTopRight);
        Gizmos.DrawLine(marginTopRight, marginBottomRight);
        Gizmos.DrawLine(marginBottomRight, marginBottomLeft);
        Gizmos.DrawLine(marginBottomLeft, marginTopLeft);
    }
}
