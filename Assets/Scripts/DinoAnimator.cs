using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DinoAnimator : Animator
{
    public Sprite[] idleSprites;
    public Sprite[] runningSprites;
    public float framesPerSecond = 20;
    public DinoColor dinoColor;

    private SpriteRenderer spriteRenderer;

    //Get the sprite render
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    //Helper method
    public void IdleAnimation()
    {
        Animate(idleSprites, spriteRenderer, idleSprites.Length / framesPerSecond);
    }
    //Helper method
    public void RunningAnimation()
    {
        Animate(runningSprites, spriteRenderer, runningSprites.Length / framesPerSecond);
    }
}