using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Paddle : MonoBehaviour
{
    // Configuration Parameters
    [SerializeField] float screenWidthInUnits = 16f;
    [SerializeField] float minX = 1f;
    [SerializeField] float maxX = 15f;
    [SerializeField] bool isAutoPlayEnabled;
    [SerializeField] AudioClip extraLifeSFX;
    [SerializeField] AudioClip megaPaddleSFX;
    [SerializeField] AudioClip extraBallSFX;
    [SerializeField] float audioClipVolume = 0.3f;

    // Cached References
    Ball theBall;
    GameSession gameSession;

    // Start is called before the first frame update
    void Start()
    {
        theBall = FindObjectOfType<Ball>();
        gameSession = FindObjectOfType<GameSession>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isAutoPlayEnabled)
            MoveWithMouse();
        else if (Input.GetAxis("Mouse X") < 0 || Input.GetAxis("Mouse X") > 0)
            MoveWithMouse();
        else
            MoveWithArrows();
    }

    void MoveWithMouse()
    {
        Vector2 paddlePos = new Vector2(transform.position.x, transform.position.y);
        paddlePos.x = Mathf.Clamp(GetXPos(), minX, maxX);
        transform.position = paddlePos;
    }

    void MoveWithArrows()
    {
        float speed = 15.0f;
        if (Input.GetKey(KeyCode.LeftArrow) && transform.position.x >= minX)
            transform.position += new Vector3(-speed * Time.deltaTime, 0, 0);
        else if (Input.GetKey(KeyCode.RightArrow) && transform.position.x <= maxX)
            transform.position += new Vector3(speed * Time.deltaTime, 0, 0);
    }

    private float GetXPos()
    {
        if (IsAutoPlayEnabled())
            return theBall.transform.position.x;
        else
        return Input.mousePosition.x / Screen.width * screenWidthInUnits;
    }

    public bool IsAutoPlayEnabled()
    {
        return isAutoPlayEnabled;
    }

    // Start PowerUps
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("extraLife"))
        {
            gameSession.AddToLives();
            AudioSource.PlayClipAtPoint(extraLifeSFX, Camera.main.transform.position, audioClipVolume);
            Destroy(other.gameObject);
        }
        else if (other.CompareTag("megaPaddle"))
        {
            StartCoroutine(GrowPaddle());
            AudioSource.PlayClipAtPoint(megaPaddleSFX, Camera.main.transform.position, audioClipVolume);
            Destroy(other.gameObject);
        }
        else if (other.CompareTag("extraBall"))
        {
            AddExtraBall();
            AudioSource.PlayClipAtPoint(extraBallSFX, Camera.main.transform.position, audioClipVolume);
            Destroy(other.gameObject);
        }
    }

    public IEnumerator GrowPaddle()
    {
        int duration = 10;
        float size = 2;

        //Vector2 newPaddleSize = paddle.transform.localScale;
        transform.localScale = new Vector2(size, 1);

        yield return new WaitForSeconds(duration);

        //Vector2 oldPaddleSize = paddle.transform.localScale;
        transform.localScale = new Vector2(1, 1);
    }

    public void AddExtraBall()
    {
        Instantiate(theBall);
    }
    // End PowerUps
}