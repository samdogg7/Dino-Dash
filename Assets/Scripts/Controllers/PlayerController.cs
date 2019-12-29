using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//This Script Controls the Dinosaur and his movements/interactions. This allows
//the dinosaur to be controlled by touch interaction.
public class PlayerController : MonoBehaviour
{
    public Camera mainCam;
    public GenerateScript generate;
    public float movementSpeed = 3f;
    public float jumpVelocity = 14f;
    public int dinoHunger = 100;
    public float pulseTime = 0.5f;
    public float lowHunger = 33f;
    public bool hungerEnabled = true;

    private CameraShake cameraShake;
    protected DinoAnimator animator;
    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rb;
    private bool jumping = false;
    private bool pulsing = false;
    private string spriteName;
    private int startingHunger;
    private int numberOfBirdsConsumed = 0;

    //Gather required components, and if the player selected a dino in the main menu, set up the sprites, and finally invoke the hunger loss
    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<DinoAnimator>();

        if (Settings.instance != null)
        {
            animator.runningSprites = Settings.instance.GetRunningSprites();
        }

        animator.RunningAnimation();

        startingHunger = dinoHunger;

        rb = GetComponent<Rigidbody2D>();
        cameraShake = mainCam.GetComponent<CameraShake>();

        InvokeRepeating("HungerEnumerator", 0f, 0.25f);
    }

    public void HungerEnumerator()
    {
        UIManager.instance.UpdateHunger(dinoHunger, startingHunger);
        
        if(hungerEnabled)
        {
            dinoHunger -= 1;
        }
    }

    //Check if dino isAlive
    private void Update()
    {
        if(dinoHunger <= 0)
        {
            GameManager.instance.isAlive = false;
        }

        if(dinoHunger <= lowHunger && !pulsing)
        {
            pulsing = true;
            StartCoroutine(PulseDinoColor());
        }
    }

    private IEnumerator PulseDinoColor()
    {
        spriteRenderer.material.color = new Color(1f,1f,1f);
        yield return new WaitForSeconds(pulseTime);
        spriteRenderer.material.color = new Color(.5f, .5f, .5f);
        yield return new WaitForSeconds(pulseTime);

        if (dinoHunger <= lowHunger)
        {
            StartCoroutine(PulseDinoColor());
        } else
        {
            pulsing = false;
            spriteRenderer.material.color = new Color(1f, 1f, 1f);
        }
    }

    //Handle player touch input
    private void FixedUpdate()
    {
        if (Input.touchCount > 0 && transform.position.x < 8f)
        {
            foreach (Touch touch in Input.touches)
            {
                if ((touch.position.x < Screen.width / 2))
                {
                    //Touch left side of screen
                    Move(true);
                    animator.framesPerSecond = 30f;
                }
                else if (touch.position.x > Screen.width / 2 && touch.position.y < (3 * Screen.height / 4))
                {
                    CameraShake.instance.Shake(.1f, .1f);
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
    public void Move(bool moveForward)
    {
        Vector2 movement = new Vector2(0, rb.velocity.y);

        //Move forward horizontally 
        if (moveForward && transform.position.x < 10)
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
            numberOfBirdsConsumed++;
            //If the bird is red, it is cooked. Gives a bigger hunger boost
            if (collisionObject.GetComponent<SpriteRenderer>().color == Color.red)
            {
                if (Settings.instance != null && Settings.instance.soundEffects)
                {
                    AudioManager.instance.PlayMunchSound();
                }
                GameManager.instance.score += 1;
                if (dinoHunger + 35 > 100)
                {
                    dinoHunger = 100;
                }
                else
                {
                    dinoHunger += 35;
                }              
            }
            else //If you eat a normal bird, you gain a smaller amount of hunger
            {
                if (Settings.instance != null && Settings.instance.soundEffects)
                {
                    AudioManager.instance.PlayMunchSound();
                }
                if (dinoHunger + 10 > 100)
                {
                    dinoHunger = 100;
                }
                else
                {
                    dinoHunger += 10;
                }
            }

            BirdScript bird = collisionObject.GetComponent<BirdScript>();
            bird.SpawnFeathers(true);
            //bird.ReturnBirdToPool();
            Destroy(collisionObject);
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
            if(Settings.instance != null && Settings.instance.soundEffects)
            {
                AudioManager.instance.PlayCrispSound();
            }
        }
        //If the player triggers a bump, jump!
        if (other.CompareTag("Bump"))
        {
            jumping = true;
        }
    }

    public int GetNumberOfBirdsConsumed()
    {
        return numberOfBirdsConsumed;
    }
}
