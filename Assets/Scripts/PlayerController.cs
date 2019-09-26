using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float jumpVelocity = 100f;

    private Vector2 initialPosition;
    private DinoAnimator animator;
    private Rigidbody2D rb;
    private BoxCollider2D col;

    private bool isCrouching = false;
    private bool isGrounded = true;

    private float crouchStartTime = 0.0f;
    private float crouchHoldTime = 0.0f;
    private float crouchDelay = 0.025f;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<BoxCollider2D>();
        animator = GetComponent<DinoAnimator>();
        animator.RunningAnimation();
    }

    void Update()
    {
        if (Input.touchCount > 0)
        {
            var touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                initialPosition = touch.position;
                crouchStartTime = Time.time - crouchHoldTime;
            }
            else if (touch.phase == TouchPhase.Stationary && !isCrouching)
            {
                crouchHoldTime = Time.time;
                if((crouchHoldTime - crouchStartTime) > crouchDelay)
                {
                    //change col to fix the crouch sprite here... TODO
                    isCrouching = true;
                    animator.CrouchAnimation();
                }
            }
            else if (touch.phase == TouchPhase.Ended)
            {
                animator.RunningAnimation();
                isCrouching = false;
                Vector2 direction = touch.position - initialPosition;
                if (direction.y > 0 && isGrounded)
                {
                    rb.velocity = Vector2.up * jumpVelocity;
                    isGrounded = false;
                    //SPAWN BUMP HERE IF WE DO WAVE
                }
                else if (direction.y < 0)
                {
                    //SPAWN DIP HERE IF WE DO WAVE
                }

                //Reset crouch timers
                crouchStartTime = 0.0f;
                crouchHoldTime = 0.0f;
            }
        }
    }

    //Prevent double jump from occuring
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }
}
