using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;
    public int score = 0;
    public TextMeshProUGUI scoreText;
    //public GameObject gameOverPanel; // �� GameOver �e��
    //public GameObject playAgainButton; // �� �s�[�G�� Play Again ���s

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
        Time.timeScale = 1f; // �Ȱ��C��
        Debug.Log("�C�������A������ playAgain �����I");
        SceneManager.LoadScene("playAgain"); // ������ PlayAgain scene
        //gameOverPanel.SetActive(true); // ��ܹC�������e��
        //playAgainButton.SetActive(true); // ��� Play again ���s
    }
}
