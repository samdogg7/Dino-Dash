using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animator : MonoBehaviour
{
    public void Animate(Sprite[] sprites, SpriteRenderer spriteRenderer, float framesPerSecond)
    {
        StopAllCoroutines();
        StartCoroutine(AnimateLoop(sprites, spriteRenderer, framesPerSecond));
    }

    private IEnumerator AnimateLoop(Sprite[] sprites, SpriteRenderer spriteRenderer, float framesPerSecond)
    {
        while (true)
        {
            foreach (Sprite sprite in sprites)
            {
                spriteRenderer.sprite = sprite;
                yield return new WaitForSeconds(1 / framesPerSecond);
            }
        }
    }
}
