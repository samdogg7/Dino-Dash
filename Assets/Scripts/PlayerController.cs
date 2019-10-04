using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GenerateScript generate;
    public float movementSpeed = 3f;
    public float jumpVelocity = 100f;
    private float dinoHunger = 100f;
    private bool jumping = false;

    private DinoAnimator animator;
    private Rigidbody2D rb;
    private AudioSource audioSource;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<DinoAnimator>();
        audioSource = GetComponent<AudioSource>();
        animator.runningSprites = Settings.instance.GetRunningSprites();
        animator.RunningAnimation();

        StartCoroutine(hungerEnumerator());
    }

    IEnumerator hungerEnumerator()
    {
        yield return new WaitForSeconds(0.1f);
        dinoHunger -= 1f;
        StartCoroutine(hungerEnumerator());
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

        Settings.instance
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
        GameObject collisionObject = collision.gameObject;

        if (collisionObject.CompareTag("Bird"))
        {
            if(collisionObject.GetComponent<Obstacle>().cooked)
            {
                audioSource.Play();
                Destroy(collision.gameObject);
                GameManager.instance.score += 1;
                dinoHunger += 25f;
            }
            else
            {
                audioSource.Play();
                Destroy(collision.gameObject);
                dinoHunger -= 5f;
            }

        }

        if (collisionObject.CompareTag("fire"))
        {
            dinoHunger -= 15f;
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
