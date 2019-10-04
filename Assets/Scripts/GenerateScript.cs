using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateScript : MonoBehaviour
{
    //Defining public variables
    public GameObject Tile;
    public GameObject DownTile;
    public GameObject UpTile;
    public GameObject BumpBox;
    public GameObject Bird;

    private bool wait = false;
    private bool WaveState = false;
    private Vector2 TempVec;
    private float waitTime;
    private float height;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(BirdGenerator());
        TempVec = transform.position;
        //TempVec.x -= .02f;
        for (int i = 0; i< 30; i++)
        {
            
            TempVec.x -= .9f;
            Instantiate(Tile, TempVec, Quaternion.identity);
        }
        Generate();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(!WaveState || collision.gameObject.layer == 10)
            Generate();
    }

    void Generate()
    {
        WaveState = false;
        Instantiate(Tile, transform.position, Quaternion.identity);
    }

    IEnumerator WaveDelay()
    {
        yield return new WaitForSeconds(2f);
        wait = false;
    }

    public void SpawnWave()
    {
        if (!wait)
        {
            TempVec = transform.position;
            TempVec.y += .49f;
            TempVec.x += 2.375f;
            Instantiate(UpTile, TempVec, Quaternion.identity);
            //TempVec.x += 0f;
            Instantiate(BumpBox, TempVec, Quaternion.identity);
            WaveState = true;
            wait = true;
            StartCoroutine(WaveDelay());
        }
    }

    IEnumerator BirdGenerator()
    {
        waitTime = Random.Range(5f, 8f);
        height = Random.Range(0f, 10f)/2;
        yield return new WaitForSeconds(waitTime);
        Instantiate(Bird, transform.position + new Vector3(0f, height, 1f), Quaternion.identity);
        StartCoroutine(BirdGenerator());
    }
}
