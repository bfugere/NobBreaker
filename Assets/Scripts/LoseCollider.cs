using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoseCollider : MonoBehaviour
{
    [SerializeField] AudioClip loseLifeSound;
    [SerializeField] float audioClipVolume = 0.2f;

    // Cached References
    Ball ball;
    GameSession gameSession;

    // Start is called before the first frame update
    void Start()
    {
        ball = FindObjectOfType<Ball>();
        gameSession = FindObjectOfType<GameSession>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //   SceneManager.LoadScene("Game Over");
        AudioSource.PlayClipAtPoint(loseLifeSound, Camera.main.transform.position, audioClipVolume);
        ball.hasStarted = false;
        gameSession.CheckLives();
    }
}
