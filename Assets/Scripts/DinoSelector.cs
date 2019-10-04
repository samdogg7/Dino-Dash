using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DinoSelector : MonoBehaviour
{
    public GameObject[] dinos;

    void Start()
    {
        foreach (GameObject dino in dinos)
        {
            dino.GetComponent<DinoAnimator>().IdleAnimation();
        }
        UpdateCharacter(dinos[Random.Range(0, dinos.Length)]);
    }

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

    private void UpdateCharacter(GameObject character)
    {
        foreach (GameObject dino in dinos)
        {
            if(dino != character)
            {
                dino.GetComponent<DinoAnimator>().IdleAnimation();
            } else
            {
                dino.GetComponent<DinoAnimator>().RunningAnimation();
            }
        }

        Settings.instance.dinoColor = character.GetComponent<DinoAnimator>().dinoColor;
    }
}
