using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectedDinoShadow : MonoBehaviour
{
    public Sprite shadow;
    public Sprite selectedShadow;
    public SpriteRenderer shadowSpriteRender;
    private float selectedScale = 1.25f;

    //Helper function to indicate which dino is selected
    public void UpdateShadow(bool selected)
    {
        if(selected)
        {
            if(transform.localScale.x < selectedScale)
            {
                shadowSpriteRender.sprite = selectedShadow;
                transform.localScale = transform.localScale * selectedScale;
            }
        }
        else
        {
            shadowSpriteRender.sprite = shadow;
            transform.localScale = new Vector3(1,1,1);
        }
    }
}
