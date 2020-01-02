using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DinoAnimator : Animator
{
    public Sprite[] idleSprites;
    public Sprite[] runningSprites;
    public Sprite[] deathSprites;

    public float framesPerSecond = 20;
    public DinoColor dinoColor;
    public bool isRunning = false;

    private SpriteRenderer spriteRenderer;

    //Get the sprite render
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        if(isRunning)
        {
            RunningAnimation();
        }
    }

    //Helper method
    public void IdleAnimation()
    {
        Animate(idleSprites, spriteRenderer, idleSprites.Length / framesPerSecond);
    }
    public void DeathAnimation()
    {
        StopAllCoroutines();
        StartCoroutine(AnimateDeath());
    }

    private IEnumerator AnimateDeath()
    {
        foreach (Sprite sprite in deathSprites)
        {
            spriteRenderer.sprite = sprite;
            yield return new WaitForSeconds(0.05f);
        }

        gameObject.GetComponent<Rigidbody2D>().AddForce(Vector2.up * 500);
        gameObject.GetComponent<Collider2D>().enabled = false;
    }

    //Helper method
    public void RunningAnimation(Sprite[] newSprites = null)
    {
        if(newSprites != null)
        {
            runningSprites = newSprites;
        }
        Animate(runningSprites, spriteRenderer, runningSprites.Length / framesPerSecond);
    }
}