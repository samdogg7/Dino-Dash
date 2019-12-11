using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MarchingBytes;

//This script generates the Tiles, Birds and Waves to create the platform and
//food for the player to interact with.
public class GenerateScript : MonoBehaviour
{
    //Defining public variables
    public GameObject Tile;
    public GameObject UpTile;
    public GameObject BumpBox;
    public GameObject Bird;
    public GameObject UnderTile;
    public ObjectPool birdPool;
    public GameObject birdPrefab;

    private bool wait = false;
    private bool WaveState = false;
    private Vector2 TempVec;
    private float waitTime;
    private float height;

    // Start is called before the first frame update
    void Start()
    {
        //Generates a Bird right at the start
        height = Random.Range(5f, 10f) / 2;
        StartCoroutine(BirdGenerator());

        //Instantiating the intial flooring of the game
        TempVec = transform.position;
        for (int i = 0; i< 30; i++)
        {
            TempVec.x -= .9f;
            Instantiate(Tile, TempVec, Quaternion.identity);
            Instantiate(UnderTile, TempVec - new Vector2(0f, .914f), Quaternion.identity);
        }
        //Beginning tile generation
        Generate();
    }

    //Generates Tiles based on hitbox off the side of the screen
    private void OnTriggerExit2D(Collider2D collision)
    {
        if(!WaveState || collision.gameObject.layer == 10)
            Generate();
    }

    //Creates two tiles a top and a bottom to create the endless runner floor
    void Generate()
    {
        WaveState = false;
        Instantiate(Tile, transform.position, Quaternion.identity);
        TempVec = transform.position;
        TempVec.y -= .914f;
        Instantiate(UnderTile, TempVec, Quaternion.identity);

    }

    //2 Second Delay
    IEnumerator WaveDelay()
    {
        yield return new WaitForSeconds(2f);
        wait = false;
    }

    //Spawns a wave
    public void SpawnWave()
    {
        if (!wait)
        {
            TempVec = transform.position;
            TempVec.y += .49f;
            TempVec.x += 2.3f;
            Instantiate(UpTile, TempVec, Quaternion.identity);
            Instantiate(BumpBox, TempVec, Quaternion.identity);
            //Fills in area underneath the wave
            Instantiate(UnderTile, TempVec + new Vector2(2.4f, -1.4f), Quaternion.identity);
            Instantiate(UnderTile, TempVec + new Vector2(2.181f, -1.4f), Quaternion.identity);
            Instantiate(UnderTile, TempVec+ new Vector2(1.267f, -1.4f), Quaternion.identity);
            Instantiate(UnderTile, TempVec + new Vector2(.353f,-1.4f), Quaternion.identity);
            Instantiate(UnderTile, TempVec + new Vector2(-.561f, -1.4f), Quaternion.identity);
            Instantiate(UnderTile, TempVec + new Vector2(-1.475f, -1.4f), Quaternion.identity);
            Instantiate(UnderTile, TempVec + new Vector2(-2.389f, -1.4f), Quaternion.identity);
            WaveState = true;
            wait = true;
            StartCoroutine(WaveDelay());
        }
    }

    //Generates Birds at random heights and intervals
    IEnumerator BirdGenerator()
    {
        waitTime = Random.Range(5f, 8f);
        height = Random.Range(5f, 10f) / 2;
        //GameObject birdClone = birdPool.CreateFromPoolAction(transform.position + new Vector3(0f, height, 1f));
        GameObject birdClone = Instantiate(birdPrefab, transform.position + new Vector3(0f, height, 1f), Quaternion.identity);
        birdClone.GetComponent<BirdScript>().objectPool = birdPool;

        yield return new WaitForSeconds(waitTime);
        StartCoroutine(BirdGenerator());
    }
}
