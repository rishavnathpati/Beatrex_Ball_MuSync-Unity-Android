using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Player : MonoBehaviour
{

    public float jumpForce;
    public float value;
    public bool gameHasStarted;
    public bool glitchModeOn;
    public new AudioSource[] audio;
    public Text scoreText;
    public GameObject jumpEffect;
    public GameObject blastEffect;
    public OrbFillBar OrbFillBar;
    public GameObject glitchCam, normalCam;

    int score;
    int scoreHundreadthCount = -1;
    int orbCount = 0;
    int audioNumber;
    int scoreValue;
    bool isDragging = false;
    bool playerHasNotCollidedWithSpike = true;
    bool tellHighScore;
    bool isBoosted;

    Vector2 touchPos, playerPos, dragPos;
    Rigidbody2D rb;


    // Start is called before the first frame update
    void Start()
    {
        score = 0;
        orbCount = 0;
        playerHasNotCollidedWithSpike = true;
        tellHighScore = true;
        isBoosted = false;
        Time.timeScale = 1f;
        gameHasStarted = true;
        value = 0.9f;
        scoreValue = 1;
        glitchModeOn = false;

        rb = GetComponent<Rigidbody2D>();
        PlayerPrefs.SetInt("score", score);

        //StartGame();
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

    /*private void StartGame()
    {
        if (UnityEngine.Random.Range(0, 2) == 1)
            PlayAudio(16);
        else
            PlayAudio(12);

        audioNumber = UnityEngine.Random.Range(21, 26);
        PlayAudio(audioNumber);
        Visualizer.instance.GetAudioSource(audio[audioNumber]);
        if (Input.GetMouseButtonDown(0))
        {
            OrbFillBar.SetPowerUpBar(orbCount);
            rb.useGravity = true;
            gameHasStarted = true;
        }
        
    }*/

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (gameHasStarted)
        {
            if (collision.CompareTag("Stair"))
            {
                if (rb.velocity.y <= 0f)
                {
                    rb.velocity = new Vector2(0, jumpForce);
                    DestroyAndMakeStair(collision);
                    StairSpawner.instance.InitColour(value);
                    JumpEffect();
                    IncreaseScore();
                }
            }

            if (collision.CompareTag("SpikeyStair") && !isBoosted && !glitchModeOn)
            {
                playerHasNotCollidedWithSpike = false;
                PlayAudio(18);
                PauseAudio(audioNumber);
                Invoke("GameOver", 1.5f);
            }

            if (collision.CompareTag("Orb"))
            {
                orbCount++;
                Destroy(Instantiate(blastEffect, transform.position, Quaternion.identity), 1f);
                OrbFillBar.SetPowerUpBar(orbCount);
                Destroy(collision.gameObject);

                if (orbCount % 10 == 0)
                {
                    orbCount = 0;
                    OrbFillBar.SetPowerUpBar(orbCount);
                    GiveBoostToPlayer();
                }
            }

            if (collision.CompareTag("Vortex"))
            {
                Destroy(collision.gameObject);
                PlayAudio(27);
                GlitchModeOn();
                StairSpawner.instance.SpecialStairs(transform.position.y);
            }
        }

    }

    private void GiveBoostToPlayer()
    {
        isBoosted = true;
        IncreaseVelocity();
        InvokeRepeating("ShakePlayer", 0f, 0.5f);
        Invoke("CancelInvoke1", 1.5f);
        for (int i = 0; i < 10; i++)
            IncreaseScore();
        scoreText.text = score.ToString();
    }

    void IncreaseVelocity()
    {
        rb.velocity = new Vector2(0f, 100f);
    }

    void ShakePlayer()
    {
        transform.position = new Vector2((UnityEngine.Random.Range(-2f, 2f)), transform.position.y);
    }

    void CancelInvoke1()
    {
        isBoosted = false;
        CancelInvoke("ShakePlayer");
    }

    private void PlayAudio(int soundNumber)
    {
        audio[soundNumber].Play();
    }

    private void IncreaseScore()
    {
        score += scoreValue;
        jumpForce += 0.07f;
        StairSpawner.instance.stairGap += .02f;
        scoreText.text = score.ToString();
        if (score % 60 == 0)
        {
            StairSpawner.instance.SetStairWidth(0.3f);
            Time.timeScale = 1.2f;
            if (UnityEngine.Random.Range(0, 2) == 1)
                PlayAudio(11);
            else
                PlayAudio(15);
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

        StairSpawner.instance.SetScore(score);
    }

    private void JumpEffect()
    {
        Destroy(Instantiate(jumpEffect, transform.position, Quaternion.identity), 0.5f);
    }

    private void DestroyAndMakeStair(Collider2D collision)
    {
        Destroy(collision.gameObject, 2f);
        StairSpawner.instance.makeStair();
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

            if (transform.position.x < -5.5f)
            {
                transform.position = new Vector2(-5.5f, transform.position.y);
            }
            if (transform.position.x > 5.5f)
            {
                transform.position = new Vector2(5.5f, transform.position.y);
            }
        }
    }

    void addGravity()
    {
        rb.velocity = new Vector2(0, rb.velocity.y - .4f);
    }

    void CheckPlayerPos()
    {
        if (playerHasNotCollidedWithSpike == true)
        {
            if (transform.position.y < Camera.main.transform.position.y - 15)
            {
                playerHasNotCollidedWithSpike = false;
                PlayAudio(18);
                PauseAudio(audioNumber);
                Invoke("GameOver", 1.5f);
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

    public void GlitchModeOn()
    {
        value = 0.25f;
        glitchCam.SetActive(true);
        normalCam.SetActive(false);
        glitchModeOn = true;
        scoreValue = 2;
        Invoke("GlitchModeOff", 10f);
    }

    public void GlitchModeOff()
    {
        StairSpawner.instance.SpecialStairs(transform.position.y);
        glitchCam.SetActive(false);
        normalCam.SetActive(true);
        value = 0.9f;
        glitchModeOn = false;
        scoreValue = 1;
    }
}
