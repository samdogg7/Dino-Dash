using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Tutorial: https://www.youtube.com/watch?v=zit45k6CUMk
public class ParallaxBackground : MonoBehaviour
{
    public float parallaxEffect;
    public GameObject cam;

    private float length;
    private float startPos;

    void Start()
    {
        startPos = transform.position.x;
        length = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    void FixedUpdate()
    {
        float temp = cam.transform.position.x * (1 - parallaxEffect);
        float distance = cam.transform.position.x * parallaxEffect;

        transform.position = new Vector3(startPos + distance, transform.position.y, transform.position.z);

        if(temp > startPos + length)
        {
            startPos += length;
        } else if (temp < startPos - length)
        {
            startPos -= length;
        }
    }
}
