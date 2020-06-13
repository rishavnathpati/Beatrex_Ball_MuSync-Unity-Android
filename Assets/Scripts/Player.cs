using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{

    public float jumpForce;
    public float value;
    public float timeScaleValue;
    public bool glitchModeOn;
    public new AudioSource[] audio;
    public Text scoreText;
    public Text highScore;
    public Text scoreText2;
    public OrbFillBar OrbFillBar;
    public GameObject jumpEffect;
    public GameObject blastEffect;
    public GameObject glitchCam, normalCam;
    public GameObject GameOverPanel;
    public GameObject UIPanel;

    int score;
    int scoreHundreadthCount = -1;
    int orbCount = 0;
    int audioNumber;
    int scoreValue;
    int lifeCount;
    bool isDragging = false;
    bool playerHasCollidedWithSpike;
    bool tellHighScore;
    bool isBoosted;

    Vector2 touchPos, playerPos, dragPos;
    Rigidbody2D rb;
    public static Player instance;

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    void Start()
    {
        score = 0;
        orbCount = 0;
        value = 0.9f;
        scoreValue = 1;
        isBoosted = false;
        Time.timeScale = 1f;
        timeScaleValue = 1f;
        tellHighScore = true;
        glitchModeOn = false;
        MenuManager.gamePaused = false;
        playerHasCollidedWithSpike = false;
        highScore.text = PlayerPrefs.GetInt("highScore").ToString();

        rb = GetComponent<Rigidbody2D>();

        if (UnityEngine.Random.Range(0, 2) == 1)
            PlayAudio(16);
        else
            PlayAudio(12);
        audioNumber = UnityEngine.Random.Range(21, 26);
        PlayAudio(audioNumber);
        Visualizer.instance.GetAudioSource(audio[audioNumber]);
        OrbFillBar.SetPowerUpBar(orbCount);
    }

    void Update()
    {
        AddGravity();
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
                JumpEffect();
                rb.velocity = new Vector2(0, jumpForce);
                Destroy(collision.gameObject, 1.5f);
                StairSpawner.instance.InitColour(value);
                IncreaseScore();
            }
        }

        if (collision.CompareTag("SpikeyStair") && !isBoosted && !glitchModeOn)
        {
            playerHasCollidedWithSpike = true;
            PlayAudio(28);
            PlayAudio(18);
            PauseAudio(audioNumber);
            GameOver();
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

    private void GiveBoostToPlayer()
    {
        isBoosted = true;
        IncreaseVelocity();
        InvokeRepeating("ShakePlayer", 0f, 0.5f);
        Invoke("CancelInvoke1", 1.5f);
        for (int i = 0; i < 10; i++)
            IncreaseScore();
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

    private void IncreaseScore()
    {
        score += scoreValue;
        jumpForce += 0.05f;
        scoreText.text = score.ToString();
        scoreText2.text = score.ToString();

        if (score % 60 == 0)
        {
            timeScaleValue += 0.08f;
            Time.timeScale = timeScaleValue;
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
                highScore.text = PlayerPrefs.GetInt("highScore").ToString();

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

        StairSpawner.instance.GetScore(score);
    }

    private void JumpEffect()
    {
        Destroy(Instantiate(jumpEffect, transform.position, Quaternion.identity), 0.5f);
    }

    private void GetInput()
    {
        if (Input.GetMouseButtonDown(0) && !MenuManager.gamePaused)
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

    void AddGravity()
    {
        if (!MenuManager.gamePaused)
            rb.velocity = new Vector2(0, rb.velocity.y - .4f);
    }

    void CheckPlayerPos()
    {
        if (!playerHasCollidedWithSpike)
        {
            if (transform.position.y < Camera.main.transform.position.y - 15)
            {
                playerHasCollidedWithSpike = true;
                PlayAudio(18);
                PauseAudio(audioNumber);
                Invoke("GameOver", 1.5f);
            }
        }
    }

    public void RespwanPos()
    {
        transform.position = new Vector2(0, transform.position.y + 15f);
        Time.timeScale = timeScaleValue;
        audio[audioNumber].Play();
        playerHasCollidedWithSpike = false;
    }

    private void PlayAudio(int soundNumber)
    {
        audio[soundNumber].Play();
    }

    private void PauseAudio(int audioNumber)
    {
        audio[audioNumber].Pause();
    }


    private void GameOver()
    {
        Time.timeScale = 0;
        MenuManager.gamePaused = true;
        GameOverPanel.SetActive(true);
        UIPanel.SetActive(false);
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
