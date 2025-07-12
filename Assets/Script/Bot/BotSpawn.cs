using System.Collections;
using UnityEngine;

public class BotSpawn : MonoBehaviour
{
    public Transform[] spawnPoints;
    public float spawnDelay = 2f;

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
        Vector2 spawnPos = GenerateSpawnInsideCamera();
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

    private Vector2 GenerateSpawnInsideCamera()
    {
        Camera mainCamera = Camera.main;

        float cameraHeight = mainCamera.orthographicSize * 2;
        float cameraWidth = cameraHeight * mainCamera.aspect;

        Vector3 center = mainCamera.transform.position;

        float minX = center.x - cameraWidth / 2f;
        float maxX = center.x + cameraWidth / 2f;
        float minY = center.y - cameraHeight / 2f;
        float maxY = center.y + cameraHeight / 2f;

        Vector2 pos = new Vector2(Random.Range(minX, maxX), Random.Range(minY, maxY));
        Debug.Log($"[Spawner] Vị trí spawn trong camera: {pos}");
        return pos;
    }
}
