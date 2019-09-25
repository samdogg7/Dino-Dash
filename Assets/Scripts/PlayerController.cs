using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Rigidbody2D rb;

    private Vector2 initialPosition;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (Input.touchCount > 0)
        {
            var touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                initialPosition = touch.position;
            }
            else if (touch.phase == TouchPhase.Ended)
            {
                Vector2 direction = touch.position - initialPosition;
                if (direction.y > 0)
                {
                    print("Spawn bump");
                }
                else if (direction.y < 0)
                {
                    print("Spawn dip");
                }
            }
        }
    }
}
