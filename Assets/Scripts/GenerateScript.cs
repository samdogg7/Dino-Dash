using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateScript : MonoBehaviour
{
    //Defining public variables
    public GameObject Tile;
    public GameObject DownTile;
    public GameObject UpTile;

    private Vector2 TempVec;
    // Start is called before the first frame update
    void Start()
    {
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
        Generate();
    }

    void Generate()
    {
        Instantiate(Tile, transform.position, Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            TempVec = transform.position;
            TempVec.y += .475f;
            Instantiate(UpTile, TempVec, Quaternion.identity);
        }
    }
}
