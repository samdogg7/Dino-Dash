using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnviorment : MonoBehaviour
{
    public GameObject[] props;
    public float spawnRate;
    private float[] propHeights;

    // Start is called before the first frame update
    void Start()
    {
        propHeights = new float[props.Length];

        for(int i=0; i<props.Length; i++)
        {
            RectTransform rect = (RectTransform)props[i].transform;
            propHeights[i] = rect.rect.y;
        }
        StartCoroutine("SpawnProp");
    }

    private IEnumerator SpawnProp()
    {
        yield return new WaitForSeconds(spawnRate + Random.Range(0f,1f));
        int randIndex = Random.Range(0, props.Length);
        GameObject prop = props[randIndex];
        Vector2 pos = (Vector2)transform.position;
        pos.y = pos.y - propHeights[randIndex];
        Instantiate(prop, pos, Quaternion.identity);
        StartCoroutine("SpawnProp");
    }
}
