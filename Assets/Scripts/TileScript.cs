using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This script gives the Tiles a velocity and destroys them when they leave the screen.
public class TileScript : MonoBehaviour
{
    private Rigidbody2D Tile;

    // Start is called before the first frame update
    void Start()
    {
        Tile = GetComponent<Rigidbody2D>();
        Tile.velocity = new Vector2(-GameManager.instance.tileMovementSpeed, 0f);
        StartCoroutine(trash());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator trash()
    {
        yield return new WaitForSeconds(15f);
        Destroy(gameObject);
    }
}
