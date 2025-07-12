using System.Collections;
using UnityEngine;

public class BotSpawn : MonoBehaviour
{
    public Transform[] spawnPoints;
    public float spawnDelay = 2f;

    private float timer = 2f;

    private void Start()
    {
        Debug.Log("[Spawner] Bắt đầu spawn bot");
        StartCoroutine(SpawnLoop());
        Debug.Log($"[Spawner] Số lượng spawn points: {spawnPoints.Length}");
    }

    private IEnumerator SpawnLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnDelay);
            SpawnBotAtRandomPoint();
        }
    }

    private void SpawnBotAtRandomPoint()
    {
        Debug.Log("[Spawner] Bắt đầu spawn bot tại điểm ngẫu nhiên");
        Vector2 spawnPos = GenerateRandomSpawnPoint(-10f, 10f, -5f, 5f);
        Debug.Log($"[Spawner] Vị trí spawn ngẫu nhiên: {spawnPos}");
        GameObject bot = PoolingManager.Instance.GetAvailableBot();

        if (bot != null)
        {
            bot.transform.SetPositionAndRotation(spawnPos, Quaternion.identity);
            Debug.Log("[Spawner] Bot đã được spawn tại vị trí: " + spawnPos);
        }
        else
        {
            Debug.LogWarning("[Spawner] Không thể spawn bot, có thể pool đã hết.");
        }
    }

    private Vector2 GenerateRandomSpawnPoint(float minX, float maxX, float minY, float maxY)
    {
        float randomX = Random.Range(minX, maxX);
        float randomY = Random.Range(minY, maxY);
        Vector2 randomPoint = new Vector2(randomX, randomY);

        Debug.Log($"[Spawner] Tạo điểm spawn ngẫu nhiên: {randomPoint}");
        return randomPoint;
    }
}
