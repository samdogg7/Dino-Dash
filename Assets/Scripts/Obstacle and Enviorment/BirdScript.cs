using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdScript : MonoBehaviour
{
    public ParticleSystem featherParticles;
    private Rigidbody2D RB;

    void Start()
    {
        RB = GetComponent<Rigidbody2D>();
        RB.velocity = new Vector2(-GameManager.instance.tileMovementSpeed + 3f, 0f);
        StartCoroutine(trash());
    }

    IEnumerator trash()
    {
        yield return new WaitForSeconds(30f);
        Destroy(gameObject);
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