using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeOutSprite : MonoBehaviour
{
    public float fadeTime = 10f;
    public float fadeRate = 1f;
    private float fadeAmount;
    private SpriteRenderer spriteRenderer;
    private Color fadeColor;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        fadeColor = spriteRenderer.color;
        fadeAmount = 1f / fadeTime;
        StartCoroutine(FadeOut());
    }

    private IEnumerator FadeOut()
    {
        yield return new WaitForSeconds(fadeRate);

        fadeTime = fadeTime - fadeRate;

        fadeColor.a = fadeColor.a - fadeAmount;

        spriteRenderer.color = fadeColor;

        if (fadeTime >= 0)
        {
            StartCoroutine(FadeOut());
        }
    }
}
