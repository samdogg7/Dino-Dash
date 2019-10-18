using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This script gives the Tiles a velocity and destroys them when they leave the screen.
public class TileScript : MonoBehaviour
{
    void Start()
    {
        GetComponent<Rigidbody2D>().velocity = new Vector2(-GameManager.instance.tileMovementSpeed, 0f);
        Destroy(gameObject, 15f);
    }
}
