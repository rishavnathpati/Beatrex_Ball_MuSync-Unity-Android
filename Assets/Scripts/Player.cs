using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{

    public float jumpForce; //decides how much force will be applied on the player
    public float colorValue; //decides what wil be the value of V in HSV 
    public float timeScaleValue; //decides the pace of the game
    public bool glitchModeOn; //for checking is glitch mode is on/off
    public new AudioSource[] audio; //an array of all the music and audio being used
    public Text scoreText; //to show the current score in Canvas UI
    public Text highScore; //to show the current high score in Canvas UI
    public Text scoreText2; //to show the current score in Game Over panel UI
    public OrbFillBar orbFillBar; //to get access to the Orb filling bar
    public GameObject jumpEffect; //the effect when player hits the platform
    public GameObject blastEffect; //the effect when player hits the Orbs
    public GameObject glitchCam, normalCam; //the two camera modes for glitch and normal mode respectively
    public GameObject GameOverPanel; //Game over panel 
    public GameObject UIPanel; //standard UI panel
    public GameObject respawnButton; //to get access to the respawn button
    private int score; //to keep count of the score
    private int scoreHundreadthCount = -1; // to keep count of the number of hundreadths reached
    private int orbCount = 0; //to keep count of the number of orbs collected
    private int audioTrackNumber; //to keep the audio track number
    private int scoreValueIncrements; //to control the increment of score in various modes
    private bool isDragging = false; //to check if the player is dragging on the screen or not
    private bool playerHasCollidedWithSpike; //to check is player has collided with the spikey platform or not
    private bool tellHighScore; //to enable the UI to tell the highscore only once during gameplay
    private bool isBoosted; //to check when the player is boosted
    private Vector2 touchPos, playerPos, dragPos; //for storing various player positions
    private Rigidbody2D rb;
    public static Player instance;
    public static bool playerOutOfScreen;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    private void Start()
    {
        score = 0;
        orbCount = 0;
        colorValue = 0.9f;
        scoreValueIncrements = 1;
        scoreHundreadthCount = -1;
        isBoosted = false;
        Time.timeScale = 1f;
        timeScaleValue = 1f;
        tellHighScore = true;
        glitchModeOn = false;
        MenuManager.gamePaused = false;
        playerHasCollidedWithSpike = false;
        playerOutOfScreen = false;

        highScore.text = PlayerPrefs.GetInt("highScore").ToString();
        rb = GetComponent<Rigidbody2D>();

        if (UnityEngine.Random.Range(0, 2) == 1)
        {
            PlayAudio(16);
        }
        else
        {
            PlayAudio(12);
        }

        if (MenuManager.loFiIsTrue) //Determining whether player has selected Lofi or EDM
        {
            audioTrackNumber = Random.Range(26, 31);
        }
        else if(MenuManager.isEDMTrue)
        {
            audioTrackNumber = Random.Range(21, 26);
        }

        PlayAudio(audioTrackNumber);
        Visualizer.instance.GetAudioSource(audio[audioTrackNumber]);
        orbFillBar.SetPowerUpBar(orbCount);
    }

    private void Update()
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
                StairSpawner.instance.InitColour(colorValue);
                IncreaseScore();
            }
        }

        if (collision.CompareTag("SpikeyStair") && !isBoosted && !glitchModeOn)
        {
            playerHasCollidedWithSpike = true;
            PlayAudio(28);
            PlayAudio(18);
            PauseAudio(audioTrackNumber);
            GameOver();
        }

        if (collision.CompareTag("Orb"))
        {
            orbCount++;
            Destroy(Instantiate(blastEffect, transform.position, Quaternion.identity), 1f);
            orbFillBar.SetPowerUpBar(orbCount);
            Destroy(collision.gameObject);

            if (orbCount % 10 == 0)
            {
                orbCount = 0;
                orbFillBar.SetPowerUpBar(orbCount);
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
        {
            IncreaseScore();
        }
    }

    private void IncreaseVelocity()
    {
        rb.velocity = new Vector2(0f, 100f);
    }

    private void ShakePlayer()
    {
        transform.position = new Vector2((UnityEngine.Random.Range(-2f, 2f)), transform.position.y);
    }

    private void CancelInvoke1()
    {
        isBoosted = false;
        CancelInvoke("ShakePlayer");
    }

    private void IncreaseScore()
    {
        score += scoreValueIncrements;
        scoreText.text = score.ToString();
        scoreText2.text = score.ToString();

        if (score % 60 == 0)
        {
            timeScaleValue += 0.08f;
            Time.timeScale = timeScaleValue;
            if (UnityEngine.Random.Range(0, 2) == 1)
            {
                PlayAudio(11);
            }
            else
            {
                PlayAudio(15);
            }
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

    private void MovePlayer()
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

    private void AddGravity()
    {
        if (!MenuManager.gamePaused)
        {
            rb.velocity = new Vector2(0, rb.velocity.y - .4f);
        }
    }

    private void CheckPlayerPos()
    {
        if (!playerHasCollidedWithSpike)
        {
            if (transform.position.y < Camera.main.transform.position.y - 15)
            {
                playerHasCollidedWithSpike = true;
                PlayAudio(18);
                PauseAudio(audioTrackNumber);
                playerOutOfScreen = true;
                Invoke("GameOver", 1.5f);
            }
        }
    }

    public void RespwanPos()
    {
        transform.position = new Vector2(0, transform.position.y + 5f);
        Time.timeScale = timeScaleValue;
        audio[audioTrackNumber].Play();
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
        if (playerOutOfScreen)
        {
            respawnButton.SetActive(false);
        }

        UIPanel.SetActive(false);
    }

    public void GlitchModeOn()
    {
        colorValue = 0.25f;
        glitchCam.SetActive(true);
        normalCam.SetActive(false);
        glitchModeOn = true;
        scoreValueIncrements = 2;
        Invoke("GlitchModeOff", 10f);
    }

    public void GlitchModeOff()
    {
        StairSpawner.instance.SpecialStairs(transform.position.y);
        glitchCam.SetActive(false);
        normalCam.SetActive(true);
        colorValue = 0.9f;
        glitchModeOn = false;
        scoreValueIncrements = 1;
    }
}