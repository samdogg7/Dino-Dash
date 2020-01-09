using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BirdState
{
    Alive, Cooked, Burnt
}

public class BirdScript : MonoBehaviour
{
    public ParticleSystem featherParticles;
    public Sprite burntSprite;
    public int aliveEnergy = 30, cookedEnergy = 70, burntEnergy = 15;
    private BirdState birdState = BirdState.Alive;
    private BirdAnimation birdAnimation;
    private Rigidbody2D rb;

    void Start()
    {
        birdAnimation = GetComponent<BirdAnimation>();
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = new Vector2(-GameManager.instance.tileMovementSpeed + 3f, 0f);
        Destroy(gameObject, 22f);
    }

    public void NextBirdState()
    {
        if(birdState == BirdState.Alive)
        {
            birdState = BirdState.Cooked;
            birdAnimation.AnimateWing();
        }
        else
        {
            birdState = BirdState.Burnt;
            rb.gravityScale = 1f;
            birdAnimation.StopAnimation(burntSprite);
        }
    }

    public int GetEnergy()
    {
        if(birdState == BirdState.Alive)
        {
            return aliveEnergy;
        } else if(birdState == BirdState.Cooked) {
            return cookedEnergy;
        } else
        {
            return burntEnergy;
        }
    }

    public void SpawnFeathers(bool isDino)
    {
        if (isDino)
        {
            print("Need to add feathers");
        } else
        {
            featherParticles.Play();
        }
    }
}