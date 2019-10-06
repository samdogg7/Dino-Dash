using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireAnimation : Animator
{

    public Sprite[] sprites;
    public float framesPerSecond = 20;

    private SpriteRenderer spriteRenderer;
    // Start is called before the first frame update

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        AnimateFire();

    }

    public void AnimateFire()
    {

        Animate(sprites, spriteRenderer, sprites.Length / framesPerSecond);
    }


}
