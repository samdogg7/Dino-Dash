﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdScript : MonoBehaviour
{
    private Rigidbody2D RB;
    // Start is called before the first frame update
    void Start()
    {
        RB = GetComponent<Rigidbody2D>();
        RB.velocity = new Vector2(-GameManager.instance.tileMovementSpeed + 3f, 0f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}