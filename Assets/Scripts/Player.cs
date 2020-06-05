using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    Rigidbody2D rb;
    float jumpForce;
    public float gravity = 1f;
    public float jumpMultiplier = 30f;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        addGravity();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Stair"))
        {
            if(rb.velocity.y<=0f)
            {
                jumpForce = gravity * jumpMultiplier;
                rb.velocity = new Vector2(0, jumpForce);
            }
                
        }
    }

    void addGravity()
    {
        rb.velocity = new Vector2(0, rb.velocity.y - (gravity * gravity));
    }
}
