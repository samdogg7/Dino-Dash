using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveBoxScript : MonoBehaviour
{
    private Vector3 velocity = new Vector3(-3.2f, 0f, 0f);
    // Start is called before the first frame update
    void Start()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        collision.gameObject.layer = 8;
        SpriteRenderer col = collision.gameObject.GetComponent<SpriteRenderer>();
        col.color = new Color(1f, 1f, 1f, 0f);
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        collision.gameObject.layer = 9;
        SpriteRenderer col = collision.gameObject.GetComponent<SpriteRenderer>();
        col.color = new Color(1f, 1f, 1f, 1f);

    }
    // Update is called once per frame
    void Update()
    {
        transform.position += velocity * Time.deltaTime;
    }
}
