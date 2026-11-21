using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    [Header("Item & Spawn Settings")]
    [SerializeField] private GameObject collectiblePrefab; // ลาก Prefab ของไอเทม (ที่มี CollectibleItem.cs) มาใส่
    [SerializeField] private List<Transform> spawnPoints = new List<Transform>(); // ลากจุดเกิดทั้งหมดมาใส่
    [SerializeField] private int spawnCount = 5; // จำนวนไอเทมที่จะสุ่มเกิดในด่านนี้
    [SerializeField] private float heightOffset = 0.5f; // เผื่อปรับความสูงให้ของลอยอยู่เหนือพื้นพอดี

    void Start()
    {
        SpawnRandomItems();
    }

    public void SpawnRandomItems()
    {
        if (collectiblePrefab == null || spawnPoints.Count == 0)
        {
            Debug.LogWarning("ยังไม่ได้ใส่ Collectible Prefab หรือ Spawn Points ใน ItemSpawner!");
            return;
        }

        // สร้างลิสต์สำเนาเพื่อไม่ให้สุ่มซ้ำจุดเดิม
        List<Transform> availablePoints = new List<Transform>(spawnPoints);

        int actualSpawnCount = Mathf.Min(spawnCount, availablePoints.Count);

        for (int i = 0; i < actualSpawnCount; i++)
        {
            int randomIndex = Random.Range(0, availablePoints.Count);
            Transform chosenPoint = availablePoints[randomIndex];

            // ป้องกันกรณีจุดเกิดถูกลบหรือว่างเปล่า
            if (chosenPoint != null)
            {
                Vector3 spawnPos = chosenPoint.position + Vector3.up * heightOffset;
                Instantiate(collectiblePrefab, spawnPos, Quaternion.identity);
            }

            // เอาออกจากลิสต์ชั่วคราว เพื่อไม่ให้ชิ้นต่อไปสุ่มทับจุดนี้
            availablePoints.RemoveAt(randomIndex);
        }
    }
}