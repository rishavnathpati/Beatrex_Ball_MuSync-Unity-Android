using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Player : MonoBehaviour
{

    public float jumpForce;
    public new AudioSource[] audio;
    public Text scoreText;
    public GameObject jumpEffect;

    int score, scoreCount = -1;
    bool isDragging = false,playerUp=true;

    Vector2 touchPos, playerPos, dragPos;
    Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        score = 0;
        playerUp = true;
        rb = GetComponent<Rigidbody2D>();
        PlaySounds(16);
        PlaySounds(21);
        
    }

    private void Sleep(int sleepTime)
    {
        System.Threading.Thread.Sleep(sleepTime);
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
    }

    private void PlaySounds(int soundNumber)
    {
        audio[soundNumber].Play();
    }

    private void IncreaseScore()
    {
        score++;
        scoreText.text = score.ToString();
        if (score % 50 == 0)
        {
            if (UnityEngine.Random.Range(0, 1) == 1)
                PlaySounds(11);
            else
                PlaySounds(15);
        }
        if (score % 100 == 0)
        {
            scoreCount++;
            PlaySounds(scoreCount);
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

            if (transform.position.x < -4.6f)
            {
                transform.position = new Vector2(-4.6f, transform.position.y);
            }
            if (transform.position.x > 4.6f)
            {
                transform.position = new Vector2(4.6f, transform.position.y);
            }
        }
    }

    void addGravity()
    {
        rb.velocity = new Vector2(0, rb.velocity.y - 0.5f);
    }

    void CheckPlayerPos()
    {
        if(playerUp==true)
        {
            if (transform.position.y < Camera.main.transform.position.y - 15)
            {
                playerUp = false;
                PlaySounds(18);
                Invoke("GameOver", 2f);
            }
        }
        

    }

    private void GameOver()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
