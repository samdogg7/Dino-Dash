using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Generator : MonoBehaviour
{

    public GameObject TileSprite1;
    public GameObject TileSpriteEnd;
    public GameObject TileSpriteStart;
    public GameObject Grass;
    private bool holetile = false;
    // Start is called before the first frame update
    void Start()
    {
        Generate();
        
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Generate();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Generate()
    {
        if (!holetile)
        {
            if (Random.Range(0, 10) == 1)
            {
                Instantiate(TileSpriteEnd, transform.position, Quaternion.identity);
                StartCoroutine(hole());
                holetile = true;

            }
            else
            {
                Instantiate(TileSprite1, transform.position, Quaternion.identity);
                Vector3 grassPos = transform.position;
                grassPos.y += .5f;
                grassPos.z += 1;
                if (Random.Range(0, 5) == 1)
                    Instantiate(Grass, grassPos, Quaternion.identity);
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
