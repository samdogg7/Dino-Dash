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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject collidedObject = collision.gameObject;

        if(collidedObject.CompareTag("Dino"))
        {
            gameManager.isAlive = false;
            rb.velocity = Vector2.zero;
            Destroy(gameObject);
        } else
        {
            //If we want to add explosion animation, do here...
            Destroy(gameObject);
        }
    }
}
