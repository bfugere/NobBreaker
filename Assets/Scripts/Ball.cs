using UnityEngine;

public class Ball : MonoBehaviour
{
    // Configuration Parameters
    [SerializeField] Paddle paddle1;
    [SerializeField] float xPush = 2f;
    [SerializeField] float yPush = 10f;
    [SerializeField] float randomFactor = 0.4f;
    [SerializeField] AudioClip[] ballSounds;
    

    // State
    Vector2 paddleToBallVector;
    public bool hasStarted = false;

    // Cached Component References
    AudioSource myAudioSource;
    Rigidbody2D myRidgidBody2D;

    // Start is called before the first frame update
    void Start()
    {
        paddleToBallVector = transform.position - paddle1.transform.position;
        myAudioSource = GetComponent<AudioSource>();
        myRidgidBody2D = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (hasStarted != true)
        {
            LockBallToPaddle();
            LaunchOnMouseClick();
        }
    }

    public void LockBallToPaddle()
    {
        Vector2 paddlePos = new Vector2(paddle1.transform.position.x, paddle1.transform.position.y);
        transform.position = paddlePos + paddleToBallVector;
    }

    public void LaunchOnMouseClick()
    {
        if (Input.GetMouseButton(0) || Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space))
        {
            hasStarted = true;
            myRidgidBody2D.velocity = new Vector2(xPush, yPush);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Random Factor
        Vector2 velocityTweak = new Vector2
            (Random.Range(-0.4f, randomFactor), 
             Random.Range(-0.4f, randomFactor));

        if (hasStarted == true)
        {
            //  Plays a single chosen audio clip.
            //  GetComponent<AudioSource>().Play();

            myRidgidBody2D.velocity = myRidgidBody2D.velocity.normalized * yPush;
            myRidgidBody2D.velocity += velocityTweak;
            // print(myRidgidBody2D.velocity.magnitude);

            // Plays a random audio clip based on ballSounds array.
            AudioClip clip = ballSounds[UnityEngine.Random.Range(0, ballSounds.Length)];
            myAudioSource.PlayOneShot(clip);
            myRidgidBody2D.velocity += velocityTweak;
        }
    }   
}