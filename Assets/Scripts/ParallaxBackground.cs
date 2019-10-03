using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Tutorial for looping background (not parallax effect): https://learn.unity.com/tutorial/live-session-making-a-flappy-bird-style-game#5c7f8528edbc2a002053b6a7
public class ParallaxBackground : MonoBehaviour
{
    public float parallaxEffect;

    private float constantSpeed = 0.1f;
    private float backgroundHorizontalLength;        //Length of the background horizontally

    private void Awake()
    {
        backgroundHorizontalLength = GetComponent<BoxCollider2D>().size.x;
    }

    void Start()
    {
        if(GameManager.instance != null)
        {
            constantSpeed = GameManager.instance.tileMovementSpeed;
        }

        float horizontalSpeed = -(constantSpeed / parallaxEffect);
        GetComponent<Rigidbody2D>().velocity = new Vector2(horizontalSpeed, 0);
    }

    private void Update()
    {
        if (transform.position.x < -backgroundHorizontalLength)
        {
            LoopPosition();
        }
    }

    private void LoopPosition()
    {
        //This is how far to the right we will move our background object, in this case, twice its length. This will position it directly to the right of the currently visible background object.
        Vector2 groundOffSet = new Vector2(backgroundHorizontalLength * 2f, 0);

        //Move this object from it's position offscreen, behind the player, to the new position off-camera in front of the player.
        transform.position = (Vector2)transform.position + groundOffSet;
    }
}