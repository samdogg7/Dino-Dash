using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Animator : MonoBehaviour
{
    //This is a helper method that will cancel the existing animation and start the next.
    public void Animate(Sprite[] sprites, SpriteRenderer spriteRenderer, float framesPerSecond, float delay = 0)
    {
        StopAllCoroutines();
        StartCoroutine(AnimateLoop(sprites, spriteRenderer, framesPerSecond, delay));
    }

    //Animation loops at the speed of the frames per second
    private IEnumerator AnimateLoop(Sprite[] sprites, SpriteRenderer spriteRenderer, float framesPerSecond, float delay)
    {
        yield return new WaitForSeconds(delay);
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
