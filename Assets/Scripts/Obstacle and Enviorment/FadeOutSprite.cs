using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeOutSprite : MonoBehaviour
{
    public float fadeTime = 10f;
    public float fadeRate = 0.25f;
    private float fadeAmount;
    private SpriteRenderer spriteRenderer;
    private Color fadeColor;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        fadeColor = spriteRenderer.color;
        fadeAmount = fadeRate/fadeTime;
        StartCoroutine(FadeOut());
    }

    private IEnumerator FadeOut()
    {
        yield return new WaitForSeconds(fadeRate);

        fadeColor.a = fadeColor.a - fadeAmount;

        spriteRenderer.color = fadeColor;

        if (fadeColor.a > 0)
        {
            StartCoroutine(FadeOut());
        }
    }
}
