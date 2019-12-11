using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MarchingBytes;

public class MeteorGenerator : MonoBehaviour
{   
    public GameObject targetObj;
<<<<<<< HEAD:Assets/Scripts/Obstacle and Enviorment/MeteorGenerator.cs
    public GameObject meteorPre;
    public float meteorTime;
    public float range;
    public float minSize;
    public float maxSize;
    public float trackPercentage;
=======
    public GameObject meteorPre;
    public ObjectPool objectPool;
    public float meteorTime = 1;
    public float range = 10;
    public float minSize = 0.5f;
    public float maxSize = 0.75f;
    private Transform spawnTransform;
>>>>>>> master:Assets/Scripts/Obstacle_and_Enviorment/MeteorGenerator.cs

    void Start()
    {
        spawnTransform = transform;
        StartCoroutine(MeteorGenerate());
    }

    IEnumerator MeteorGenerate() {
<<<<<<< HEAD:Assets/Scripts/Obstacle and Enviorment/MeteorGenerator.cs
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
=======
        if(EasyObjectPool.instance != null)
        {
            GameObject clone = Instantiate(meteorPre, transform.position + new Vector3(Random.Range(-range, range), 0, 0), Quaternion.identity);
            //GameObject clone = objectPool.CreateFromPoolAction(spawnPosition);
>>>>>>> master:Assets/Scripts/Obstacle_and_Enviorment/MeteorGenerator.cs

            float randomScale = Random.Range(minSize, maxSize);
            clone.transform.localScale = new Vector3(randomScale, randomScale, randomScale);

            Vector3 targ = targetObj.transform.position;
            targ.z = 0f;

<<<<<<< HEAD:Assets/Scripts/Obstacle and Enviorment/MeteorGenerator.cs
        float angle = Mathf.Atan2(targ.y, targ.x) * Mathf.Rad2Deg;
        clone.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
        clone.transform.Rotate(0, 0, 90, Space.World);
    
        Rigidbody2D rb = clone.GetComponent<Rigidbody2D>();

        Vector2 meteorVector = new Vector2(0,0);
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
=======
            Vector3 objectPos = clone.transform.position;
            targ.x = targ.x - objectPos.x;
            targ.y = targ.y - objectPos.y;

            float angle = Mathf.Atan2(targ.y, targ.x) * Mathf.Rad2Deg;
            clone.transform.rotation = Quaternion.identity;
            clone.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
            clone.transform.Rotate(0, 0, 90, Space.World);

            Rigidbody2D rb = clone.GetComponent<Rigidbody2D>();
            rb.velocity = Vector3.zero;
            Vector2 meteorVector = (Vector2)targetObj.transform.position - rb.position;
            meteorVector.Normalize();
            rb.velocity = meteorVector * (GameManager.instance.tileMovementSpeed);
        }
>>>>>>> master:Assets/Scripts/Obstacle_and_Enviorment/MeteorGenerator.cs


        yield return new WaitForSeconds(meteorTime);
        StartCoroutine(MeteorGenerate());
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position - new Vector3(range, 0, 0), transform.position + new Vector3(range, 0, 0));
        Gizmos.DrawLine(transform.position - new Vector3(range, 1, 0), transform.position - new Vector3(range, -1, 0));
        Gizmos.DrawLine(transform.position + new Vector3(range, 1, 0), transform.position + new Vector3(range, -1, 0));
    }
}
