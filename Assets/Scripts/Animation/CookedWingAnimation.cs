using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CookedWingAnimation : Animator
{
    public Sprite[] wingSprites;
    public float framesPerSecond = 20;

    private SpriteRenderer spriteRenderer;
    
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        AnimateWing();
    }

    private void OnEnable()
    {
        AnimateWing();
    }

    public void AnimateWing()
    {
        Animate(wingSprites, spriteRenderer, wingSprites.Length / framesPerSecond);
    }
}
