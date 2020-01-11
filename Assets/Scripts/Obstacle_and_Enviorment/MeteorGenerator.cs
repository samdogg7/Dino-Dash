using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MarchingBytes;

public class MeteorGenerator : MonoBehaviour
{
    public GameObject targetObj;
    public GameObject meteorPre;
    public float meteorSpawnRate;
    public float range;
    public float minSize;
    public float maxSize;
    public float trackPercentage = 75f;
    public float trackingOffset = 15f;
    public float speedMultiplier = 1.5f;

    void Start()
    {
        StartCoroutine(MeteorGenerate());
    }

    IEnumerator MeteorGenerate()
    {
        int rInt = Random.Range(0, 100);
        bool goDown = true;

        if (rInt < trackPercentage)
        {
            goDown = false;
        }


        GameObject clone;
        Vector3 spawnPosition = transform.position;
        spawnPosition += new Vector3(Random.Range(-range, range), 0, 0);

        clone = Instantiate(meteorPre, spawnPosition, Quaternion.identity);
        float randomScale = Random.Range(minSize, maxSize);
        clone.transform.localScale = new Vector3(randomScale, randomScale, randomScale);

        Vector3 targ = targetObj.transform.position;
        targ.z = 0f;

        Vector3 objectPos = clone.transform.position;
        targ.x = targ.x - objectPos.x;
        targ.y = targ.y - objectPos.y;

        float angle = Mathf.Atan2(targ.y, targ.x) * Mathf.Rad2Deg;
        angle += Random.Range(-trackingOffset, trackingOffset);
        clone.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
        clone.transform.Rotate(0, 0, 90, Space.World);

        Rigidbody2D rb = clone.GetComponent<Rigidbody2D>();

        Vector2 meteorVector = new Vector2(0, 0);
        if (!goDown)
        {
            meteorVector = (Vector2)targetObj.transform.position - rb.position;
        }
        else
        {
            meteorVector = new Vector2(rb.position.x + Random.Range(-90, 90), -90f);
        }
        meteorVector *= Random.Range(1, speedMultiplier);
        meteorVector.Normalize();
        rb.velocity = meteorVector * (GameManager.instance.tileMovementSpeed);

        if (GameManager.instance.score >= 25)
        {
            yield return new WaitForSeconds(meteorSpawnRate / 1.1f);
        }
        else if (GameManager.instance.score >= 50)
        {
            yield return new WaitForSeconds(meteorSpawnRate/1.2f);
        }
        else if (GameManager.instance.score >= 100)
        {
            yield return new WaitForSeconds(meteorSpawnRate/1.3f);
        }
        else if (GameManager.instance.score >= 150)
        {
            yield return new WaitForSeconds(meteorSpawnRate / 1.4f);
        }
        else if (GameManager.instance.score >= 175)
        {
            yield return new WaitForSeconds(meteorSpawnRate / 1.5f);
        }
        else if (GameManager.instance.score >= 200)
        {
            yield return new WaitForSeconds(meteorSpawnRate / 1.6f);
        }
        else
        {
            yield return new WaitForSeconds(meteorSpawnRate);
        }

        StartCoroutine(MeteorGenerate());
    }
}
