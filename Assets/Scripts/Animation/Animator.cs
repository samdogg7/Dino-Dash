using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Animator : MonoBehaviour
{
    //This is a helper method that will cancel the existing animation and start the next.
    public void Animate(Sprite[] sprites, SpriteRenderer spriteRenderer, float framesPerSecond, float delay = 0, bool repeating = true)
    {
        StopAllCoroutines();
        StartCoroutine(AnimateLoop(sprites, spriteRenderer, framesPerSecond, delay, repeating));
    }

    //Animation loops at the speed of the frames per second
    private IEnumerator AnimateLoop(Sprite[] sprites, SpriteRenderer spriteRenderer, float framesPerSecond, float delay, bool repeating)
    {
        yield return new WaitForSeconds(delay);
        while (repeating)
        {
            foreach (Sprite sprite in sprites)
            {
                spriteRenderer.sprite = sprite;
                yield return new WaitForSeconds(1 / framesPerSecond);
            }
        }
    }
}
