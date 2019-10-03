using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteorGenerator : MonoBehaviour
{

    public float meteorTime = 1;
    public float range = 10;
    public GameObject targetObj;
    public GameObject meteorPre;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(MeteorGenerate());
    }


    IEnumerator MeteorGenerate() {
        bool check = true;

        while (check == true){
            yield return new WaitForSeconds(meteorTime);
            GameObject clone;
            Vector3 spawnPosition = transform.position;
            spawnPosition += new Vector3(Random.Range(-range, range), 0, 0);

            clone = Instantiate(meteorPre, spawnPosition, Quaternion.identity);

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
        }

    }

    void OnDrawGizmos()
    {

        Gizmos.DrawLine(transform.position - new Vector3(range, 0, 0), transform.position + new Vector3(range, 0, 0));
        Gizmos.DrawLine(transform.position - new Vector3(range, 1, 0), transform.position - new Vector3(range, -1, 0));
        Gizmos.DrawLine(transform.position + new Vector3(range, 1, 0), transform.position + new Vector3(range, -1, 0));

    }
}
