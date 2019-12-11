using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MarchingBytes;

public class MeteorGenerator : MonoBehaviour
{

    public GameObject targetObj;
    public GameObject meteorPre;
    public float meteorTime;
    public float range;
    public float minSize;
    public float maxSize;
    public float trackPercentage;

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
        clone.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
        clone.transform.Rotate(0, 0, 90, Space.World);

        Rigidbody2D rb = clone.GetComponent<Rigidbody2D>();

        Vector2 meteorVector = new Vector2(0, 0);
        if (!goDown)
        {
            meteorVector = (Vector2)targetObj.transform.position - rb.position;
            Debug.Log("should track");
        }
        else
        {
            meteorVector.y = -1;
            Debug.Log("should go down");
        }

        meteorVector.Normalize();
        rb.velocity = meteorVector * (GameManager.instance.tileMovementSpeed);


        yield return new WaitForSeconds(meteorTime);
        StartCoroutine(MeteorGenerate());
    }
}
