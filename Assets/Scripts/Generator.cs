using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Generator : MonoBehaviour
{
    public GameObject[] Tiles = new GameObject[16];
    public GameObject TileSprite1;
    public GameObject TileSpriteEnd;
    public GameObject TileSpriteStart;
    public GameObject TileSpriteDip;
    public GameObject Grass;
    public GameObject TileSpriteBump;
    public BoxCollider2D wavespot;
    public GameObject dipbox;
    private int tick = 0;
    private bool holetile = false;
    // Start is called before the first frame update
    public float time;
    public float timer;
    void Start()
    {
        
        for (int i = 1; i < 480; i++)
        {
            if(tick == 16)
            {
                tick = 0;
            }
            Vector3 pos = transform.position;
            pos.x = pos.x - (.05f) * i;
            Instantiate(Tiles[tick], pos, Quaternion.identity);
            tick += 1;
        }
        
        StartCoroutine(Test());
        //Generate();
        time = .9f;
        timer = time;
    }

    IEnumerator Test()
    {
        yield return new WaitForSeconds(.01f);
        if (tick == 16)
        {
            tick = 0;
        }
        Instantiate(Tiles[tick], transform.position, Quaternion.identity);
        tick += 1;
        StartCoroutine(Test());
    }

    
    private void OnTriggerExit2D(Collider2D collision)
    {
        /*
        if (collision.gameObject.tag != "wave")
        {
            Generate();
        }
        */
    }


    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;
        if (Input.GetKeyDown(KeyCode.S))
            if (timer < 0) // This prevents player from spamming the wave
            {
                {
                    Vector3 grassPos = transform.position;
                    grassPos.y += -.375f;
                    grassPos.z += -1;
                    Instantiate(TileSpriteDip, grassPos, Quaternion.identity);
                    Instantiate(dipbox, grassPos, Quaternion.identity);
                    timer = time;
                }
            }

        if (Input.GetKeyDown(KeyCode.W))
            if (timer < 0) // This prevents player from spamming the wave
            {
                {
                    Vector3 grassPos = transform.position;
                    grassPos.y += .375f;
                    grassPos.z += -1;
                    Instantiate(TileSpriteBump, grassPos, Quaternion.identity);
                    Instantiate(dipbox, grassPos, Quaternion.identity);
                    timer = time;
                }
            }
    }
    /*
    void Generate()
    {
        if (!holetile)
        {
            if (Random.Range(0, 10) == 11)
            {
                Instantiate(TileSpriteEnd, transform.position, Quaternion.identity);
                StartCoroutine(hole());
                holetile = true;

            }
            else
            {
                if(tick == 17)
                {
                    tick = 0;
                }
                Instantiate(Tiles[tick], transform.position, Quaternion.identity);
                tick += 1;
                Vector3 grassPos = transform.position;
                grassPos.y += -.375f;
                grassPos.z += -1;
                if (Random.Range(0, 5) == 1) { }
                    //Instantiate(TileSpriteDip, grassPos, Quaternion.identity);
            }
            
        }
    }

    IEnumerator hole()
    {
        yield return new WaitForSeconds(2f);
        Instantiate(TileSpriteStart, transform.position, Quaternion.identity);
        holetile = false;
    }
}
*/
}