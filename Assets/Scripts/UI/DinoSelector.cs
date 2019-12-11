using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DinoSelector : MonoBehaviour
{
    //Array of selectable dinos
    public GameObject[] dinos;
    public SpriteRenderer[] dinoShadows;

    //Set all of the dinos to the idle animation, and randomly select a dino
    void Start()
    {
        foreach (GameObject dino in dinos)
        {
            if (dino.GetComponent<DinoAnimator>().dinoColor != Settings.instance.dinoColor)
            {
                dino.GetComponent<DinoAnimator>().IdleAnimation();
            }
            else
            {
                UpdateCharacter(dino);
            }
        }
    }

    //Check for user input selecting the dino
    void Update()
    {
        if ((Input.touchCount > 0) && (Input.GetTouch(0).phase == TouchPhase.Began))
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position), Vector2.zero);
            if (hit.collider != null && hit.collider.CompareTag("Dino"))
            {
                print(hit.collider.gameObject.name);
                UpdateCharacter(hit.collider.gameObject);
            }
        }
    }

    //Changes the selected dino to the running animation to help highlight which dino is selected. It will also update the settings instance so that this transfers to other scenes
    private void UpdateCharacter(GameObject character)
    {
        foreach (GameObject dino in dinos)
        {
            if(dino != character)
            {
                dino.GetComponent<DinoAnimator>().IdleAnimation();
                dino.GetComponent<SelectedDinoShadow>().UpdateShadow(false);
            }
            else
            {
                dino.GetComponent<DinoAnimator>().RunningAnimation();
                dino.GetComponent<SelectedDinoShadow>().UpdateShadow(true);
            }
        }
        
        Settings.instance.dinoColor = character.GetComponent<DinoAnimator>().dinoColor;
    }
}
