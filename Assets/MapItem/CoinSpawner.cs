using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinSpawner : MonoBehaviour
{
    public GameObject coinPrefab;         // 要生成的 Coin 預置體
    public Transform coinSpot;             // coinSpot 父物件
    public float spawnInterval = 1f;       // 每隔幾秒生成

    private Transform[] spawnPoints;       // 生成點陣列
    private Dictionary<Transform, GameObject> activeCoins = new Dictionary<Transform, GameObject>(); // 每個點上的coin
    private int currentCoinCount = 0;       // 現在場上有幾個 coin
    public int maxCoinCount = 3;            // 同時場上最多3個coin

    void Start()
    {
        // 把 coinSpot 底下的所有子物件存起來
        spawnPoints = new Transform[coinSpot.childCount];
        for (int i = 0; i < coinSpot.childCount; i++)
        {
            spawnPoints[i] = coinSpot.GetChild(i);
        }

        // 開始每隔一段時間生成
        InvokeRepeating("SpawnCoin", 0f, spawnInterval);
    }

    void SpawnCoin()
    {
        // 如果場上coin已經超過最大數量，不生成
        if (currentCoinCount >= maxCoinCount) return;

        if (spawnPoints.Length == 0) return;

        int randomIndex = Random.Range(0, spawnPoints.Length);
        Transform spawnPoint = spawnPoints[randomIndex];

        if (activeCoins.ContainsKey(spawnPoint) && activeCoins[spawnPoint] != null)
        {
            // 這個生成點上已經有 coin，不生成
            return;
        }

        GameObject newCoin = Instantiate(coinPrefab, spawnPoint.position, Quaternion.identity);

        activeCoins[spawnPoint] = newCoin;
        currentCoinCount++;  // coin數量+1

        // 告訴coin自己是哪個生成器生的
        coinController coinScript = newCoin.GetComponent<coinController>();
        if (coinScript != null)
        {
            coinScript.SetSpawner(this, spawnPoint);
        }
    }

    // Coin吃掉自己後呼叫這個
    public void ClearSpawnPoint(Transform spawnPoint)
    {
        if (activeCoins.ContainsKey(spawnPoint))
        {
            activeCoins[spawnPoint] = null;
        }
        currentCoinCount--;  // coin數量-1
        if (currentCoinCount < 0) currentCoinCount = 0; // 保險，防止負數
    }
}
