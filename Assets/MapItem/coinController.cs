using UnityEngine;

public class coinController : MonoBehaviour
{
    public AudioClip coinClip; // 硬幣音效
    public GameObject coinParticlePrefab; // 加粒子Prefab的變數
    private CoinSpawner spawner;    // 記錄是哪個 spawner 生成的
    private Transform spawnPoint;   // 記錄是在哪個生成點生成的

    public void SetSpawner(CoinSpawner spawner, Transform spawnPoint)
    {
        this.spawner = spawner;
        this.spawnPoint = spawnPoint;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            ScoreManager.instance.AddScore(1);
            // 播放音效
            AudioSource.PlayClipAtPoint(coinClip, transform.position, 3f);
            // 播放特效
            if (coinParticlePrefab != null)
            {
                Instantiate(coinParticlePrefab, transform.position, Quaternion.identity);
            }
            // 通知 Spawner 清除這個生成點
            if (spawner != null)
            {
                spawner.ClearSpawnPoint(spawnPoint);
            }

            Destroy(gameObject);
        }
    }
}
