using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteorGenerator : MonoBehaviour
{

    public float meteorTime = 1;
    public float range = 10;
    public Vector2 target;
    public Vector3 target3;

    public GameObject meteorPre;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(MeteorGenerate());
    }


    IEnumerator MeteorGenerate() {
        bool check = true;

        while (check == true){

            GameObject clone;
            GameObject targetObj = GameObject.Find("meteorTarget" );
            Debug.Log(targetObj.transform.position);

            yield return new WaitForSeconds(meteorTime);
            Vector3 spawnPosition = transform.position;
            spawnPosition += new Vector3(Random.Range(-range, range), 0, 0);

            clone = Instantiate(meteorPre, spawnPosition, Quaternion.identity);
            //LookRotation(targetObj.transform.position - spawnPosition, Vector3.forward)
            //clone.transform.Rotate(0, 0, 90, Space.World);


            Vector3 targ = targetObj.transform.position;
            targ.z = 0f;

            Vector3 objectPos = clone.transform.position;
            targ.x = targ.x - objectPos.x;
            targ.y = targ.y - objectPos.y;

            float angle = Mathf.Atan2(targ.y, targ.x) * Mathf.Rad2Deg;
            clone.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
            clone.transform.Rotate(0, 0, 90, Space.World);



            Rigidbody2D rb = clone.GetComponent<Rigidbody2D>();
            Vector2 meteorVector = targetObj.GetComponent<Rigidbody2D>().position - rb.position;
            rb.AddForce(meteorVector * 10f);
            
            //clone.transform.rotation = Quaternion.LookRotation(targetObj.transform.position);
           // Debug.Log(clone.transform.rotation);
        }

    }
    // Update is called once per frame
    void Update()
    {
        
    }

    void OnDrawGizmos()
    {

        Gizmos.DrawLine(transform.position - new Vector3(range, 0, 0), transform.position + new Vector3(range, 0, 0));
        Gizmos.DrawLine(transform.position - new Vector3(range, 1, 0), transform.position - new Vector3(range, -1, 0));
        Gizmos.DrawLine(transform.position + new Vector3(range, 1, 0), transform.position + new Vector3(range, -1, 0));

    }
}
