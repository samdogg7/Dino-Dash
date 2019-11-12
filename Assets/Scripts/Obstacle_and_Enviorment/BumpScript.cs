using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BumpScript : MonoBehaviour
{
    private Rigidbody2D body;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Dino"))
        {
            body = collision.gameObject.GetComponent<Rigidbody2D>();
            body.velocity = new Vector2(70f, 15f);
        }
    }
}
