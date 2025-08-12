using System.Collections.Generic;
using UnityEngine;

public class PoolingManager : MonoBehaviour
{
    public static PoolingManager Instance { get; private set; }

    [System.Serializable]
    public class Pool
    {
        public string tag;
        public GameObject prefab;
        public int size = 10;
        public Transform parent; // Để gom object cùng loại vào 1 group trong Hierarchy
    }

    public List<Pool> pools;
    private Dictionary<string, Queue<GameObject>> poolDictionary;
    private Dictionary<string, int> activeCount;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        poolDictionary = new Dictionary<string, Queue<GameObject>>();
        activeCount = new Dictionary<string, int>();

        foreach (Pool pool in pools)
        {
            Queue<GameObject> objectPool = new Queue<GameObject>();
            for (int i = 0; i < pool.size; i++)
            {
                GameObject obj = Instantiate(pool.prefab, pool.parent);
                obj.SetActive(false);
                objectPool.Enqueue(obj);
            }
            poolDictionary.Add(pool.tag, objectPool);
            activeCount.Add(pool.tag, 0);
        }
    }

    /// <summary>
    /// Lấy object từ pool theo tag.
    /// </summary>
    public GameObject GetFromPool(string tag, Vector3 position, Quaternion rotation)
    {
        if (!poolDictionary.ContainsKey(tag))
        {
            Debug.LogWarning($"[PoolingManager] Pool với tag {tag} không tồn tại.");
            return null;
        }

        if (poolDictionary[tag].Count == 0)
        {
            Debug.LogWarning($"[PoolingManager] Pool {tag} đã hết object.");
            return null;
        }

        GameObject obj = poolDictionary[tag].Dequeue();
        obj.SetActive(true);
        obj.transform.position = position;
        obj.transform.rotation = rotation;
        activeCount[tag]++;
        return obj;
    }

    /// <summary>
    /// Trả object về pool.
    /// </summary>
    public void ReturnToPool(string tag, GameObject obj)
    {
        if (!poolDictionary.ContainsKey(tag))
        {
            Debug.LogWarning($"[PoolingManager] Pool với tag {tag} không tồn tại.");
            Destroy(obj);
            return;
        }
        obj.SetActive(false);
        poolDictionary[tag].Enqueue(obj);
        activeCount[tag]--;
    }

    /// <summary>
    /// Kiểm tra còn object khả dụng trong pool.
    /// </summary>
    public bool HasAvailable(string tag)
    {
        return poolDictionary.ContainsKey(tag) && poolDictionary[tag].Count > 0;
    }

    /// <summary>
    /// Đếm số object đang active theo tag.
    /// </summary>
    public int GetActiveCount(string tag)
    {
        return activeCount.ContainsKey(tag) ? activeCount[tag] : 0;
    }
}
