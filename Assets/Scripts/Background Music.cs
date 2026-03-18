using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMusic : MonoBehaviour
{
    // Start is called before the first frame update
    public static BackgroundMusic Instance;
    private AudioSource audioSource;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            audioSource = GetComponent<AudioSource>();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    //toggle the background music on/off from the settings panel
    public void ToggleMusic(bool isOn)
    {
        if (audioSource != null)
        {
            audioSource.enabled = isOn;
        }
    }
}
