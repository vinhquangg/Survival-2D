using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build.Content;
using UnityEngine;
using UnityEngine.UIElements;

public class PoolingManager : MonoBehaviour
{
    public static PoolingManager Instance { get; private set; }

    [Header("Pool Settings")]
    public GameObject BotPrefab;
    public int initialPoolSize = 10;
    public bool autoExpand = true;
    private int activeBotCount = 0;


    private Queue <GameObject> botQueue = new Queue<GameObject>();

    private void Awake()
    {
        Debug.Log($"[DEBUG] BotPrefab asset path: {UnityEditor.AssetDatabase.GetAssetPath(BotPrefab)}");
        if(Instance != null && Instance != this)
        {
            Debug.LogWarning("[PoolingManager] Đã có một instance tồn tại, sẽ hủy bỏ instance mới.");
            Destroy(gameObject);
            return;
        }
        Instance = this;
        Debug.Log("[PoolingManager] Đã khởi tạo Instance");
        PreloadInfo();
    }
    private void PreloadInfo()
    {
        for (int i = 0; i < initialPoolSize; i++)
        {
            CreateAndAddBot();
        }
    }

    private void CreateAndAddBot()
    {
        Debug.Log($"[PoolingManager] BotPrefab hiện tại là: {(BotPrefab != null ? BotPrefab.name : "null")}");
        if (BotPrefab == null)
        {
            Debug.LogError("[PoolingManager] chưa gán prefab bot");
            return;
        }
        GameObject bot = Instantiate(BotPrefab);
        bot.SetActive(false);
        botQueue.Enqueue(bot);
    }

    public GameObject GetAvailableBot()
    {
        if (botQueue.Count > 0)
        {
            GameObject bot = botQueue.Dequeue();
            bot.SetActive(true);
            activeBotCount++;
            ResetBotState(bot);
            return bot;
        }
        else if (autoExpand) 
        {
            int previousCount = botQueue.Count;
            CreateAndAddBot();

            if (botQueue.Count > previousCount)
            {
                return GetAvailableBot();
            }
            else
            {
                Debug.LogError("[PoolingManager] Không thể tạo bot mới, BotPrefab có thể bị null.");
            }
        }
        return null;
    }

    public void RecycleBot(GameObject bot)
    {
        if (bot == null)
        {
            Debug.Log("[PoolingManager] Attempted to recycle a null bot.");
            return;
        }
        bot.SetActive(false);
        botQueue.Enqueue(bot);
        activeBotCount--;
        Debug.Log($"[Pool] Recycle Called For: {bot.name}");
        Debug.Log($"[Pool] Recycled bot — Active: {activeBotCount} | Inactive: {botQueue.Count}");
    }

    private void ResetBotState(GameObject bot)
    {
        BotControlling2D botControlling2D = bot.GetComponent<BotControlling2D>();
        if (botControlling2D != null)
        {
            botControlling2D.ResetBot();
        }
    }
}
