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
    private Vector2 movement;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<DinoAnimator>();
        audioSource = GetComponent<AudioSource>();
        animator.RunningAnimation();
        movement = new Vector2(0, rb.velocity.y);
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
                    movement.x = movementSpeed;
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
            movement.x = -movementSpeed;
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

    private void FixedUpdate()
    {
        rb.velocity = movement;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Bird") && collision.gameObject.GetComponent<BirdScript>())
        {
            audioSource.Play();
            Destroy(collision.gameObject);
            GameManager.instance.score += 1;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log(other.name);
        if (other.name == "bumpbox")
        {
            movement.y = jumpVelocity;
        }
    }
}
