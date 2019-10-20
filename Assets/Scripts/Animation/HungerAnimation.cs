using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HungerAnimation : Animator
{
    public Sprite[] hungerSprites;
    public float framesPerSecond = 20;
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        Animate(hungerSprites, spriteRenderer, framesPerSecond);
    }
}