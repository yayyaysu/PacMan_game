using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameOverManager : MonoBehaviour
{
    public void RestartGame()
    {
        StartCoroutine(DelayedRestart());
        
        // SceneManager.LoadScene("Map"); // 載入地圖場景
    }
    
    private IEnumerator DelayedRestart()
    {
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene("Map"); // 載入地圖場景
    }
}