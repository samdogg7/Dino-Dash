using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdAnimation : Animator
{
    public Sprite[] birdSprites;
    public Sprite[] wingSprites;
    public float framesPerSecond = 20;

    private SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        AnimateBird();
    }

    public void AnimateBird()
    {
        Animate(birdSprites, spriteRenderer, birdSprites.Length / framesPerSecond);
    }

    public void AnimateWing()
    {
        Animate(wingSprites, spriteRenderer, wingSprites.Length / framesPerSecond);
    }

    public void StopAnimation(Sprite sprite)
    {
        StopAllCoroutines();
        spriteRenderer.sprite = sprite;
    }
}
