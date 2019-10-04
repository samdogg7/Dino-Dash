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
    public GameObject UnderTile;

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
            Instantiate(UnderTile, TempVec - new Vector2(0f, .914f), Quaternion.identity);
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
        TempVec = transform.position;
        TempVec.y -= .914f;
        Instantiate(UnderTile, TempVec, Quaternion.identity);

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
            Instantiate(UnderTile, TempVec + new Vector2(2.4f, -1.4f), Quaternion.identity);
            Instantiate(UnderTile, TempVec + new Vector2(2.181f, -1.4f), Quaternion.identity);
            Instantiate(UnderTile, TempVec+ new Vector2(1.267f, -1.4f), Quaternion.identity);
            Instantiate(UnderTile, TempVec + new Vector2(.353f,-1.4f), Quaternion.identity);
            Instantiate(UnderTile, TempVec + new Vector2(-.561f, -1.4f), Quaternion.identity);
            Instantiate(UnderTile, TempVec + new Vector2(-1.475f, -1.4f), Quaternion.identity);
            WaveState = true;
            wait = true;
            StartCoroutine(WaveDelay());
        }
    }

    IEnumerator BirdGenerator()
    {
        waitTime = Random.Range(5f, 8f);
        height = Random.Range(3f, 10f)/2;
        yield return new WaitForSeconds(waitTime);
        Instantiate(Bird, transform.position + new Vector3(0f, height, 1f), Quaternion.identity);
        StartCoroutine(BirdGenerator());
    }
}
