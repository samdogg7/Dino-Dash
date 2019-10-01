using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Tutorial: https://www.youtube.com/watch?v=zit45k6CUMk
public class ParallaxBackground : MonoBehaviour
{
    public float parallaxEffect;
    public GameObject cam;
    public float constantSpeed = 10;

    private Vector2 startPositionVector;
    private float length;
    private float startPos;

    void Start()
    {
        startPositionVector = transform.position;
        startPos = transform.position.x;
        length = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    void FixedUpdate()
    {
        Vector2 testPos = Vector2.Lerp(new Vector2(startPositionVector.x + 10, startPositionVector.y), new Vector2(startPositionVector.x - 10, startPositionVector.y), (Mathf.Sin(constantSpeed * Time.time) + 1.0f) / 2.0f);

        float temp = testPos.x * (1 - parallaxEffect);
        float distance = testPos.x * parallaxEffect;

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