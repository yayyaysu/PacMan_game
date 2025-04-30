using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float moveSpeed = 3f;
    public float detectionRange = 5f;
    public float stuckCheckTime = 1f; // 檢查是否卡住的時間
    public float detourDistance = 2f; // 繞障時移動的距離
    public float wanderChangeTime = 2f; // 徘徊時改變方向的間隔

    private Transform player;
    private Rigidbody2D rb;
    private bool isChasing = false;
    private Vector2 lastPosition; // 上一個位置，用於檢查是否卡住
    private float stuckTimer = 0f; // 卡住計時器
    private bool isDetouring = false; // 是否正在繞障
    private Vector2 detourDirection; // 繞障方向
    private Vector2 wanderDirection; // 徘徊方向
    private float wanderTimer = 0f; // 徘徊計時器

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        rb = GetComponent<Rigidbody2D>();
        lastPosition = transform.position;
        // 初始隨機方向
        wanderDirection = Random.insideUnitCircle.normalized;
    }

    void FixedUpdate()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        // 決定是否追蹤玩家
        if (distanceToPlayer <= detectionRange)
        {
            isChasing = true;
        }
        else
        {
            isChasing = false;
        }

        // 檢查是否卡住（追擊或徘徊時）
        if (!isDetouring)
        {
            float distanceMoved = Vector2.Distance(transform.position, lastPosition);
            if (distanceMoved < 0.1f)
            {
                stuckTimer += Time.fixedDeltaTime;
                if (stuckTimer >= stuckCheckTime)
                {
                    StartDetour(); // 開始繞障
                }
            }
            else
            {
                stuckTimer = 0f; // 重置卡住計時器
            }
            lastPosition = transform.position;
        }

        // 移動邏輯
        if (isChasing)
        {
            if (isDetouring)
            {
                // 執行繞障移動
                rb.linearVelocity = detourDirection * moveSpeed;
                stuckTimer += Time.fixedDeltaTime;
                if (stuckTimer >= stuckCheckTime)
                {
                    isDetouring = false; // 繞障結束，重新直線追擊
                    stuckTimer = 0f;
                }
            }
            else
            {
                // 直線追擊玩家
                Vector2 direction = (player.position - transform.position).normalized;
                rb.linearVelocity = direction * moveSpeed;
            }
        }
        else
        {
            // 隨機徘徊
            wanderTimer += Time.fixedDeltaTime;
            if (wanderTimer >= wanderChangeTime)
            {
                wanderDirection = Random.insideUnitCircle.normalized; // 隨機選擇新方向
                wanderTimer = 0f;
            }

            if (isDetouring)
            {
                // 執行繞障移動
                rb.linearVelocity = detourDirection * moveSpeed;
                stuckTimer += Time.fixedDeltaTime;
                if (stuckTimer >= stuckCheckTime)
                {
                    isDetouring = false; // 繞障結束，繼續徘徊
                    stuckTimer = 0f;
                }
            }
            else
            {
                // 按照徘徊方向移動
                rb.linearVelocity = wanderDirection * moveSpeed;
            }
        }
    }

    void StartDetour()
    {
        isDetouring = true;
        stuckTimer = 0f;

        if (isChasing)
        {
            // 追擊時的繞障方向
            Vector2 toPlayer = (player.position - transform.position).normalized;
            detourDirection = new Vector2(-toPlayer.y, toPlayer.x); // 順時針90度
            if (Random.value > 0.5f)
            {
                detourDirection = -detourDirection; // 逆時針90度
            }
        }
        else
        {
            // 徘徊時的繞障方向（基於當前徘徊方向）
            detourDirection = new Vector2(-wanderDirection.y, wanderDirection.x); // 順時針90度
            if (Random.value > 0.5f)
            {
                detourDirection = -detourDirection; // 逆時針90度
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }
}