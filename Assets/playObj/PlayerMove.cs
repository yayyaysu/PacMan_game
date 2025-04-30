using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class PlayerMove : MonoBehaviour
{

    public AudioClip damageClip;
    private AudioSource audioSource;
    public float moveSpeed = 5f;
    public float maxHealth = 100f;
    public float currentHealth;
    public float damageAmount = 10f;
    public float invincibilityTime = 1f;
    public TextMeshProUGUI healthText;

    private Rigidbody2D rb;
    private float invincibilityTimer;
    private bool isInvincible;

    void Start()
    {
        audioSource = GetComponent<AudioSource>(); // 確保AudioSource存在
        rb = GetComponent<Rigidbody2D>();
        currentHealth = maxHealth;
        UpdateHealthUI();
        invincibilityTimer = 0f;
        isInvincible = false;
    }

    void Update()
    {
        // 處理移動
        float moveX = Input.GetAxisRaw("Horizontal"); // A/D鍵
        float moveY = Input.GetAxisRaw("Vertical");   // W/S鍵
        Vector2 movement = new Vector2(moveX, moveY).normalized * moveSpeed;
        rb.linearVelocity = movement; // 修正：設置速度為movement，而非Vector2.zero

        // 處理面向方向
        if (moveX < 0)
        {
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
        else if (moveX > 0)
        {
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }

        // 處理無敵時間
        if (isInvincible)
        {
            invincibilityTimer -= Time.deltaTime;
            if (invincibilityTimer <= 0)
            {
                isInvincible = false;
            }
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy") && !isInvincible)
        {
            TakeDamage(damageAmount);
            StartInvincibility();
        }
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy") && !isInvincible)
        {
            TakeDamage(damageAmount);
            StartInvincibility();
        }
    }

    void TakeDamage(float damage)
    {
        Debug.Log("TakeDamage called with damage: " + damage);
        Debug.Log("damageClip is null: " + (damageClip == null));
        Debug.Log("audioSource is null: " + (audioSource == null));

        currentHealth -= damage;
        if (damageClip != null && audioSource != null)
        {
            Debug.Log("玩家受傷了！");
            audioSource.PlayOneShot(damageClip);
        }
        UpdateHealthUI();

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void UpdateHealthUI()
    {
        healthText.text = "Health: " + Mathf.FloorToInt(currentHealth).ToString();
    }

    void StartInvincibility()
    {
        isInvincible = true;
        invincibilityTimer = invincibilityTime;
    }

    void Die()
    {

        SceneManager.LoadScene("GameOver");
    }
}