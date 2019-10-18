using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinGameobject : MonoBehaviour
{
    public int rotationPerSecond = 30;

    private void Start()
    {
        if (Random.value >= 0.5)
        {
            rotationPerSecond = -rotationPerSecond;
        }
    }

    void Update()
    {
        transform.Rotate(0, 0, rotationPerSecond * Time.deltaTime);
    }
}
