using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GenerateScript generate;
    public float movementSpeed = 3f;
    public float jumpVelocity = 100f;

    private DinoAnimator animator;
    private Rigidbody2D rb;
    private AudioSource audioSource;
    private bool jumping = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<DinoAnimator>();
        audioSource = GetComponent<AudioSource>();
        animator.RunningAnimation();
    }

    void Update()
    {
        //Touch input
        if (Input.touchCount > 0)
        {
            foreach (Touch touch in Input.touches)
            {
                if (touch.position.x < Screen.width / 2)
                {
                    //Touch left side of screen
                    Move(true);
                    animator.framesPerSecond = 30f;
                }
                else if (touch.position.x > Screen.width / 2)
                {
                    //Touch right side of screen
                    generate.SpawnWave();
                }
            }
        } else
        {
            animator.framesPerSecond = 20f;
            Move(false);
        }

        //Keyboard input
        //if (Input.GetKey(KeyCode.D) && transform.position.x >= -7f && transform.position.x <= 7f)
        //{
        //    rb.velocity = new Vector2(movementSpeed, rb.velocity.y);
        //}
        //if (Input.GetKeyDown(KeyCode.W))
        //{
        //    generate.SpawnWave();
        //}
    }

    private void Move(bool moveForward)
    {
        Vector2 movement = new Vector2(0,rb.velocity.y);

        if (moveForward)
        {
            movement.x = movementSpeed;
        }
        else
        {
            movement.x = -movementSpeed;
        }

        rb.velocity = movement;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Bird") && collision.gameObject.GetComponent<BirdScript>())
        {
            audioSource.Play();
        }

        if (collision.gameObject.CompareTag("cookedBird"))
        {
            //gain more health
            audioSource.Play();
            Destroy(collision.gameObject);
        }
        if (collision.gameObject.CompareTag("Bird"))
        {
            //gain less health
            audioSource.Play();
            Destroy(collision.gameObject);
            GameManager.instance.score += 1;
        }

        if (collision.gameObject.CompareTag("fire"))
        {
            //lose health
            //play charring sound also here
            audioSource.Play();
            
        }

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Bump")
        {
            Debug.Log("Jump");
            rb.AddForce(Vector2.up * jumpVelocity);
        }
    }
}
