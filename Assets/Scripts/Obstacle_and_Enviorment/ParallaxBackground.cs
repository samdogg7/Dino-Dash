using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Attach this script to each layer of the background. Make parallax effect larger for background elements and smaller for foreground

//Tutorial for looping background (not parallax effect): https://learn.unity.com/tutorial/live-session-making-a-flappy-bird-style-game#5c7f8528edbc2a002053b6a7
public class ParallaxBackground : MonoBehaviour
{
    //This float is important for creating the parallax effect. The layers closer to the foreground are smaller numbers
    //than the background layers, making them move more quickly when dividing the constant speed; 
    public float parallaxEffect;
    //Constant movement speed of layers
    private float constantSpeed = 0.1f;
    private float backgroundHorizontalLength;        //Length of the background horizontally
    //Find the width of layer, so we know when to loop it back to its starting position
    private void Awake()
    {
        backgroundHorizontalLength = GetComponent<BoxCollider2D>().size.x;
    }

    void Start()
    {
        //Use the tile movement speed as a baseline speed for background
        if (GameManager.instance != null)
        {
            constantSpeed = GameManager.instance.tileMovementSpeed;
        }
        //Apply the horizontal speed (-) velocity
        float horizontalSpeed = -(constantSpeed / parallaxEffect);
        GetComponent<Rigidbody2D>().velocity = new Vector2(horizontalSpeed, 0);
    }

    //If the background layer reaches its width, reset it's position
    private void Update()
    {
        if (transform.position.x <= -backgroundHorizontalLength)
        {
            LoopPosition();
        }
    }

    //Loop the layer back to the starting position
    private void LoopPosition()
    {
        //This is how far to the right we will move our background object, in this case, twice its length. This will position it directly to the right of the currently visible background object.
        Vector2 groundOffSet = new Vector2(backgroundHorizontalLength * 2f, 0);

        //Move this object from it's position offscreen, behind the player, to the new position off-camera in front of the player.
        transform.position = (Vector2)transform.position + groundOffSet;
    }
}