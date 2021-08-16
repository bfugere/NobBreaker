using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using TMPro;

public class GameSession : MonoBehaviour
{
    // Parameters
    [Range(0.5f, 5f)] [SerializeField] float gameSpeed = 1f;
    [SerializeField] int pointsPerBlockDestroyed = 25;
    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] TextMeshProUGUI livesText;
    [SerializeField] TextMeshProUGUI highScoreText;

    // State Variables
    [SerializeField] int currentScore = 0;
    [SerializeField] int currentLives = 3;

    // Before Start
    private void Awake()
    {
        // Singleton Pattern
        int gameStatusCount = FindObjectsOfType<GameSession>().Length;

        if (gameStatusCount > 1)
        {
            gameObject.SetActive(false);
            Destroy(gameObject);
        }
        else
            DontDestroyOnLoad(gameObject);
    }

    // Cached Reference
    Ball ball;

    // Start is called before the first frame update
    private void Start()
    {
        ball = FindObjectOfType<Ball>();
        scoreText.text = "Score: " + currentScore.ToString();
        livesText.text = "Lives: " + currentLives.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        Time.timeScale = gameSpeed;
    }

    public void AddToScore()
    {
        currentScore += pointsPerBlockDestroyed;
        scoreText.text = "Score: " + currentScore.ToString();
    }

    // Start Power Ups
    public void AddToLives()
    {
        currentLives++;
        livesText.text = "Lives: " + currentLives.ToString();
    }
    // End Power Ups

    public void CheckLives()
    {
        if (currentLives <= 0)
            GameOver();
        else
        {
            currentLives--;
            livesText.text = "Lives: " + currentLives.ToString();
            RelockToPaddle();
        }
    }

    public void RelockToPaddle()
    {
        ball.LockBallToPaddle();
        ball.LaunchOnMouseClick();
    }

    public void GameOver()
    {
        SceneManager.LoadScene("Game Over");

        int highScore = PlayerPrefs.GetInt("HIGHSCORE");
        if (currentScore > highScore)
        {
            PlayerPrefs.SetInt("HIGHSCORE", currentScore);
            highScoreText.text = "*New* Highscore: " + currentScore;
        }
        else
            highScoreText.text = "Highscore: " + highScore + "\n" + "Can you beat it?";
    }

    public void ResetGame()
    {
        Destroy(gameObject);
    }
}
