﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WavesScript : MonoBehaviour
{
    
    private Rigidbody2D Tile;
    // Start is called before the first frame update
    void Start()
    {
        Tile = GetComponent<Rigidbody2D>();
        Tile.velocity = new Vector2(-3.2f, 0f);
        StartCoroutine(trash());
    }

    // Update is called once per frame

    void Update()
    {
        
    }

    
    IEnumerator trash()
    {
        yield return new WaitForSeconds(10f);
        Destroy(gameObject);
    }
}