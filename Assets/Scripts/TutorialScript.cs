using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialScript : PlayerController
{
    // Update is called once per frame
    private void Start()
    {
        SetupDino();
        movementSpeed = 2;
        jumpVelocity = 5;
    }

    void FixedUpdate()
    {
        if (Input.touchCount > 0 && transform.position.x < 8f)
        {
            Time.timeScale = 1f;
            foreach (Touch touch in Input.touches)
            {
                if ((touch.position.x < Screen.width / 2))
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
        }
        else
        {
            Time.timeScale = 0.5f;
            animator.framesPerSecond = 20f;
            Move(false);
        }
    }
}
