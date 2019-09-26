using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DinoAnimator : Animator
{
    public Sprite[] idleSprites;
    public Sprite[] runningSprites;
    public Sprite[] crouchSprites;
    public float framesPerSecond = 20;

    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void IdleAnimation()
    {
        Animate(idleSprites, spriteRenderer, idleSprites.Length / framesPerSecond);
    }

    public void RunningAnimation()
    {
        Animate(runningSprites, spriteRenderer, runningSprites.Length / framesPerSecond);
    }

    public void CrouchAnimation()
    {
        Animate(crouchSprites, spriteRenderer, runningSprites.Length / framesPerSecond);
    }
}