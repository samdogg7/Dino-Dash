using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectedDinoShadow : MonoBehaviour
{
    public Sprite shadow;
    public Sprite selectedShadow;
    public SpriteRenderer shadowSpriteRender;

    //Helper function to indicate which dino is selected
    public void UpdateShadow(bool selected)
    {
        if(selected)
        {
            shadowSpriteRender.sprite = selectedShadow;
            transform.localScale = transform.localScale * 1.25f;
        }
        else
        {
            shadowSpriteRender.sprite = shadow;
            transform.localScale = new Vector3(1,1,1);
        }
    }
}
