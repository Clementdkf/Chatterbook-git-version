using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MusicToggle : MonoBehaviour
{
    private AudioSource audioSource;
    //public GameObject toggleObject;
    // Start is called before the first frame updates
    void Start()
    {
        //audioSource = toggleObject.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    public void ToggleMusic(bool isOn) //Function for music toggle
    {
        if (BackgroundMusic.Instance != null)
        {
            BackgroundMusic.Instance.ToggleMusic(isOn);
        }
    }
}
