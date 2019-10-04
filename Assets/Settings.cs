using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Settings : MonoBehaviour
{

    public Button musicOff;
    public Button musicOn;
    public Button soundEffectsOn;
    public Button soundEffectsOff;

    // Start is called before the first frame update
    void Start()
    {
        musicOff.onClick.AddListener(MusicOffButtonClicked);
    }

    void MusicOffButtonClicked()
    {
       
    }
}
