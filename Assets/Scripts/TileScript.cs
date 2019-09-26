using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileScript : MonoBehaviour
{
    private Rigidbody2D Tile;

    // Start is called before the first frame update
    void Start()
    {
        Tile = GetComponent<Rigidbody2D>();
        Tile.velocity = new Vector2(-1.6f, 0f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
