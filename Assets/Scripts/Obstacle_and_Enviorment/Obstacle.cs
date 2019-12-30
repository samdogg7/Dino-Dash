using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    public float shakeTime = 0.25f;
    public float shakeMagnitude = 0.1f;
    public GameObject collisionParticles;
    private GameManager gameManager;
    private Rigidbody2D rb;
    private CameraShake cameraShake;

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
            gameManager.isAlive = false;
            rb.velocity = Vector2.zero;   
        }
        else if (collidedObject.CompareTag("Bird"))
        {
            BirdScript birdScript = collidedObject.GetComponent<BirdScript>();
            if(!birdScript.isCooked)
            {
                birdScript.isCooked = true;
                collidedObject.GetComponent<BirdScript>().SpawnFeathers(false);
            } else
            {
                Destroy(collidedObject);
                SpawnColParticles();
            }
            collidedObject.GetComponent<BirdAnimation>().AnimateWing();
        } else
        {
            SpawnColParticles();
        }
        CameraShake.instance.Shake(shakeTime, shakeMagnitude);
        Destroy(gameObject);
    }

    public void SpawnColParticles()
    {
        GameObject colParticles = Instantiate(collisionParticles, transform.position, Quaternion.identity);
        Destroy(colParticles, 1f);
    }

}
