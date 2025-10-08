using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MusicToggle : MonoBehaviour
{
    private AudioSource audioSource;
    public GameObject toggleObject;
    // Start is called before the first frame updates
    void Start()
    {
        audioSource = toggleObject.GetComponent<AudioSource>();
        /*Toggle toggle = GetComponent<Toggle>();
        if (toggle == null)
        {
            Debug.LogError("Toggle component not found on this GameObject.");
            return;
        }*/
        //toggle.onValueChanged.AddListener(ToggleMusic);
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void ToggleMusic(bool isOn)
    {
        if (audioSource != null)
        {
            if (isOn == true)
            {
                audioSource.enabled = true;
            }
            else
            {
                audioSource.enabled = false;
            }
        }
    }
}
