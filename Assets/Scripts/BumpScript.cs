using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BumpScript : MonoBehaviour
{
    public Rigidbody2D body;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        body = collision.gameObject.GetComponent<Rigidbody2D>();
        body.velocity = new Vector2(0f, 10f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
