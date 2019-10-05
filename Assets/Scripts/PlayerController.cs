using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GenerateScript generate;
    public float movementSpeed = 10f;
    public float jumpVelocity = 100f;
    public int dinoHunger = 100;
    private bool jumping = false;

    private DinoAnimator animator;
    private Rigidbody2D rb;
    private AudioSource audioSource;

    //Gather required components, and if the player selected a dino in the main menu, set up the sprites, and finally invoke the hunger loss
    private void Start()
    {
        generate.SpawnWave();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<DinoAnimator>();
        audioSource = GetComponent<AudioSource>();
        if(Settings.instance != null)
        {
            animator.runningSprites = Settings.instance.GetRunningSprites();
        }
        animator.RunningAnimation();

        InvokeRepeating("HungerEnumerator", 0f, 0.5f);
    }

    void HungerEnumerator()
    {
        dinoHunger -= 1;
    }

    //Check if dino isAlive
    void Update()
    {
        if(dinoHunger >= 0)
        {
            UIManager.instance.UpdateHunger(dinoHunger);
        } else
        {
            GameManager.instance.isAlive = false;
        }
    }

    //Handle player touch input
    private void FixedUpdate()
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
    }

    //Handles dino movement
    private void Move(bool moveForward)
    {
        Vector2 movement = new Vector2(0, rb.velocity.y);

        //Move forward horizontally 
        if (moveForward)
        {
            movement.x = movementSpeed;
        }
        else //Move backward horizontally
        {
            movement.x = -movementSpeed;
        }

        //If the player runs over a bump, they will hit a trigger that sets jumping to be true
        if (jumping)
        {
            movement.y = Mathf.Sqrt(2 * jumpVelocity * Mathf.Abs(Physics2D.gravity.y));
            jumping = false;
        }

        //Add gravity to the players y, otherwise the dino will float vertically indefintely
        movement.y += Physics2D.gravity.y * Time.deltaTime;

        //Set the velocity to the players movement
        rb.velocity = movement;
    }

    //Check for collisions with obstacles, and the ground
    private void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject collisionObject = collision.gameObject;

        if (collisionObject.CompareTag("Bird"))
        {
            //If the bird is red, it is cooked. Gives a bigger hunger boost
            if(collisionObject.GetComponent<SpriteRenderer>().color == Color.red)
            {
                audioSource.Play();
                Destroy(collision.gameObject);
                GameManager.instance.score += 1;
                dinoHunger += 25;
            }
            else //If you eat a normal bird, you gain a smaller amount of hunger
            {
                audioSource.Play();
                Destroy(collision.gameObject);
                dinoHunger += 5;
            }
        }
    }

    //Check if the player is running up a bump, or has fallen back into the fire
    private void OnTriggerEnter2D(Collider2D other)
    {
        //If the player hits the fire on screen left, the dino dies
        if (other.CompareTag("fire"))
        {
            dinoHunger = 0;
            //play charring sound also here
            audioSource.Play();
        }
        //If the player triggers a bump, jump!
        if (other.CompareTag("Bump"))
        {
            jumping = true;
        }
    }
}
