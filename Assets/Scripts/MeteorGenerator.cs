using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteorGenerator : MonoBehaviour
{

    public float meteorTime = 1;
    public float range = 10;

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
            Vector3 spawnPosition = transform.position;
            spawnPosition += new Vector3(Random.Range(-range, range), 0, 0);
            Instantiate(meteorPre, spawnPosition, Quaternion.identity);


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
