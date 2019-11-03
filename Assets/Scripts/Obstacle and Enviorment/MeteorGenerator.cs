using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MarchingBytes;

public class MeteorGenerator : MonoBehaviour
{

    
    public GameObject targetObj;
    public GameObject meteorPre;
    public string poolName = "AsteroidPool";
    public float meteorTime = 1;
    public float range = 10;
    public float minSize = 0.5f;
    public float maxSize = 0.75f;
    private List<GameObject> asteroidList = new List<GameObject>();

    void Start()
    {
        StartCoroutine(MeteorGenerate());
    }


    public GameObject CreateFromPoolAction(Vector3 spawnLocation)
    {
        GameObject go = EasyObjectPool.instance.GetObjectFromPool(poolName, spawnLocation, Quaternion.identity);
        if (go)
        {
            asteroidList.Add(go);
        }

        return go;
    }

    public void ReturnToPoolAction(GameObject asteroid)
    {
        EasyObjectPool.instance.ReturnObjectToPool(asteroid);
        asteroidList.Remove(asteroid);
    }

    IEnumerator MeteorGenerate() {
        GameObject clone;
        Vector3 spawnPosition = transform.position;
        spawnPosition += new Vector3(Random.Range(-range, range), 0, 0);

        clone = CreateFromPoolAction(spawnPosition);
        clone.GetComponent<Obstacle>().meteorGenerator = this;
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

        Vector2 meteorVector = (Vector2)targetObj.transform.position - rb.position;
        meteorVector.Normalize();
        rb.velocity = meteorVector * (GameManager.instance.tileMovementSpeed);

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
