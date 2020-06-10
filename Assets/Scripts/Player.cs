using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Player : MonoBehaviour
{

    public float jumpForce;
    public new AudioSource[] audio;
    public Text scoreText;
    public GameObject jumpEffect;
    public OrbFillBar OrbFillBar;

    int score;
    int scoreHundreadthCount = -1;
    int orbCount = 0;
    int audioNumber;
    bool isDragging = false;
    bool playerUp = true;
    bool tellHighScore;

    Vector2 touchPos, playerPos, dragPos;
    Rigidbody2D rb;


    // Start is called before the first frame update
    void Start()
    {
        score = 0;
        orbCount = 0;
        playerUp = true;
        tellHighScore = true;

        rb = GetComponent<Rigidbody2D>();
        PlayerPrefs.SetInt("score", score);

        if (UnityEngine.Random.Range(0, 2) == 1)
            PlayAudio(16);
        else
            PlayAudio(12);

        audioNumber = UnityEngine.Random.Range(21, 26);
        PlayAudio(audioNumber);
        Visualizer.instance.GetAudioSource(audio[audioNumber]);

        OrbFillBar.SetPowerUpBar(orbCount);
    }

    // Update is called once per frame
    void Update()
    {
        addGravity();
        GetInput();
        MovePlayer();
        CheckPlayerPos();
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Stair"))
        {
            if (rb.velocity.y <= 0f)
            {
                //jumpForce = gravity * jumpMultiplier;
                rb.velocity = new Vector2(0, jumpForce);
                DestroyAndMakeStair(collision);
                StairSpawner.stairSpawner.InitColour();
                JumpEffect();
                IncreaseScore();
            }
        }

        if (collision.CompareTag("Orb"))
        {
            orbCount++;
            OrbFillBar.SetPowerUpBar(orbCount);
            Destroy(collision.gameObject);
            if (orbCount % 10 == 0)
            {
                orbCount = 0;
                OrbFillBar.SetPowerUpBar(orbCount);
                GiveBoostToPlayer();
            }

        }
    }

    private void GiveBoostToPlayer()
    {
        //InvokeRepeating("IncreaseVelocity", 0, 1.5f);
        IncreaseVelocity();
        InvokeRepeating("ShakePlayer", 0f, 0.5f);
        Invoke("CancelInvoke1", 1.5f);
        score += 10;
        scoreText.text = score.ToString();
    }

    void IncreaseVelocity()
    {
        rb.velocity = new Vector2(0f, 100f);
    }

    void ShakePlayer()
    {
        transform.position = new Vector2((UnityEngine.Random.Range(-2f, 2f)), transform.position.y);
        //transform.position = new Vector2(1f, transform.position.y);
    }

    void CancelInvoke1()
    {
        CancelInvoke("ShakePlayer");
        //CancelInvoke("IncreaseVelocity");
    }

    private void PlayAudio(int soundNumber)
    {
        audio[soundNumber].Play();
    }

    private void IncreaseScore()
    {
        score++;
        jumpForce += 0.1f;
        scoreText.text = score.ToString();
        if (score % 60 == 0)
        {
            if (UnityEngine.Random.Range(0, 2) == 1)
                PlayAudio(11);
            else
                PlayAudio(15);
            StairSpawner.stairSpawner.stairGap += 1;
        }
        if (score % 100 == 0)
        {
            scoreHundreadthCount++;
            PlayAudio(scoreHundreadthCount);
        }

        if (PlayerPrefs.HasKey("highScore"))
        {
            if (score > PlayerPrefs.GetInt("highScore"))
            {
                PlayerPrefs.SetInt("highScore", score);
                if (tellHighScore)
                {
                    PlayAudio(UnityEngine.Random.Range(13, 15));
                    tellHighScore = false;
                }

            }
        }
        else
        {
            PlayerPrefs.SetInt("highScore", score);
        }
    }

    private void JumpEffect()
    {
        Destroy(Instantiate(jumpEffect, transform.position, Quaternion.identity), 0.5f);
    }

    private void DestroyAndMakeStair(Collider2D collision)
    {
        Destroy(collision.gameObject, 2f);
        //collision.gameObject.SetActive(false);
        StairSpawner.stairSpawner.makeStair();
    }

    private void GetInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("Mouse button Down");
            isDragging = true;
            touchPos = Camera.main.ScreenToWorldPoint(new Vector2(Input.mousePosition.x, Input.mousePosition.y));
            playerPos = transform.position;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            Debug.Log("Mouse button Up");
            isDragging = false;
        }
    }

    void MovePlayer()
    {
        if (isDragging)
        {
            dragPos = Camera.main.ScreenToWorldPoint(new Vector2(Input.mousePosition.x, Input.mousePosition.y));
            transform.position = new Vector2(playerPos.x + (dragPos.x - touchPos.x), transform.position.y);

            if (transform.position.x < -5f)
            {
                transform.position = new Vector2(-5f, transform.position.y);
            }
            if (transform.position.x > 5f)
            {
                transform.position = new Vector2(5f, transform.position.y);
            }
        }
    }

    void addGravity()
    {
        rb.velocity = new Vector2(0, rb.velocity.y - 0.5f);
    }

    void CheckPlayerPos()
    {
        if (playerUp == true)
        {
            if (transform.position.y < Camera.main.transform.position.y - 15)
            {
                playerUp = false;
                PlayAudio(18);
                PauseAudio(audioNumber);
                Invoke("GameOver", 2f);
            }
        }


    }

    private void PauseAudio(int audioNumber)
    {
        audio[audioNumber].Pause();
    }

    private void GameOver()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
