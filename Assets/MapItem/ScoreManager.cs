using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;
    public int score = 0;
    public TextMeshProUGUI scoreText;
    //public GameObject gameOverPanel; // 放 GameOver 畫面
    //public GameObject playAgainButton; // ← 新加：放 Play Again 按鈕

    private void Awake()
    {
        instance = this;
    }

    public void AddScore(int amount)
    {
        score += amount;
        scoreText.text = "score: " + score.ToString();

        if (score >= 10)
        {
            EndGame();
        }
    }

    private void EndGame()
    {
        Time.timeScale = 1f; // 暫停遊戲
        Debug.Log("遊戲結束，切換到 playAgain 場景！");
        SceneManager.LoadScene("playAgain"); // 切換到 PlayAgain scene
        //gameOverPanel.SetActive(true); // 顯示遊戲結束畫面
        //playAgainButton.SetActive(true); // 顯示 Play again 按鈕
    }
}
