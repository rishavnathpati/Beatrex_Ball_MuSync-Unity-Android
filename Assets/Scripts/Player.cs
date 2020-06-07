using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{

    Rigidbody2D rb;
    float jumpForce;

    public float gravity = 1f;
    public float jumpMultiplier = 30f;

    bool isDragging = false;
    Vector2 touchPos, playerPos, dragPos;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
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
                jumpForce = gravity * jumpMultiplier;
                rb.velocity = new Vector2(0, jumpForce);
                DestroyAndMakeStair(collision);
                StairSpawner.stairSpawner.InitColour();
            }

        }
    }

    private void DestroyAndMakeStair(Collider2D collision)
    {
        Destroy(collision.gameObject,2f);
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
        rb.velocity = new Vector2(0, rb.velocity.y - (gravity * gravity));
    }

    void CheckPlayerPos()
    {
        if (transform.position.y < Camera.main.transform.position.y - 15)
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
