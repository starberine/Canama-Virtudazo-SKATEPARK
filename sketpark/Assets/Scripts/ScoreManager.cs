using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class ScoreManager : MonoBehaviour
{
    public int totalRamen = 5; 
    private int collectedRamen = 0; 
    public TMP_Text scoreText;
    public TMP_Text timerText;
    public GameObject winPanel;
    public GameObject losePanel;

    private float timeRemaining = 60f; 
    private bool gameOver = false;

    void Start()
    {
        UpdateScoreUI();
        UpdateTimerUI();
        winPanel.SetActive(false);
        losePanel.SetActive(false);
    }

    void Update()
    {
        if (!gameOver)
        {
            HandleTimer();
        }
    }

    void HandleTimer()
    {
        if (timeRemaining > 0)
        {
            timeRemaining -= Time.deltaTime;
            UpdateTimerUI();
        }
        else
        {
            LoseGame();
        }
    }

    void UpdateScoreUI()
    {
        scoreText.text = $"{collectedRamen}/{totalRamen} ramen collected";
    }

    void UpdateTimerUI()
    {
        timerText.text = $"Time: {Mathf.Ceil(timeRemaining)}s";
    }

    public void DeliverRamen()
    {
        if (!gameOver)
        {
            collectedRamen++;
            UpdateScoreUI();
            
            if (collectedRamen >= totalRamen)
            {
                WinGame();
            }
        }
    }

    void WinGame()
    {
        gameOver = true;
        Time.timeScale = 0;
        winPanel.SetActive(true);
    }

    void LoseGame()
    {
        gameOver = true;
        Time.timeScale = 0;
        losePanel.SetActive(true);
    }

    public void RetryGame()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void BackToMainMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("MainMenu");
    }
}
