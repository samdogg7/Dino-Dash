using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    private GameManager gameManager;
    private Rigidbody2D rb;

    void Start()
    {
        if(GameManager.instance != null)
        {
            gameManager = GameManager.instance;
        }
        rb = GetComponent<Rigidbody2D>();
    }
  

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject collidedObject = collision.gameObject;
        if (collidedObject.CompareTag("Dino"))
        {
            //player lose health
            //play charring sound
            gameManager.isAlive = false;
            rb.velocity = Vector2.zero;
            
        }
        else if (collidedObject.CompareTag("Bird"))
        {

            collidedObject.GetComponent<SpriteRenderer>().color = Color.red;
            collidedObject.tag = "cookedBird";
            //play a charring sound effect here
        }
        Destroy(gameObject);
    }

}
