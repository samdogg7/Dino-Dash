using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class TextPulse : MonoBehaviour
{
    private TextMeshProUGUI myText;
    public int startText;
    public int minSize;
    public int maxSize;
    public int introBuild;
    public int pongSpeed;
   
    private float timer = 0f;

    // Start is called before the first frame update
    void Start()
    {
        myText = GetComponent<TextMeshProUGUI>();
        myText.fontSize = startText;
}

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        if (myText.fontSize < minSize)
        {
            /*{ if (timer > waitTime)
                {
                    timer -= waitTime;
                    myText.fontSize += 1;
                }
            }
            */
            int newSize1 = (int)(Mathf.PingPong(Time.time * introBuild, minSize + 1));
            myText.fontSize = newSize1;


        }
        else
        {
            int newSize = (int)(Mathf.PingPong(Time.time * pongSpeed, maxSize - minSize));
            myText.fontSize = newSize + minSize;
        }
    }
}

