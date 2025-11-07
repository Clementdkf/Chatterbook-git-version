using UnityEngine;

public class SettingsManager : MonoBehaviour
{
    public static SettingsManager Instance { get; private set; }
    public float CurrentTextSize { get; private set; } = 36f; // Default

    public float CurrentVolume { get; private set; } = 1f; // Default   

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject); // Persist across scenes/pages
            CurrentTextSize = PlayerPrefs.GetFloat("textSize", 36f);
            CurrentVolume = PlayerPrefs.GetFloat("volume", 1f);
        }
    }

    public void SetTextSize(float newSize)
    {
        CurrentTextSize = newSize;
        PlayerPrefs.SetFloat("textSize", newSize);
        PlayerPrefs.Save();
        // Optional: Trigger an event here for all active texts to update immediately
    }

    public void SetVolume(float newVolume)
    {
        CurrentVolume = newVolume;
        PlayerPrefs.SetFloat("volume", newVolume);
        PlayerPrefs.Save();
        AudioListener.volume = newVolume;
    }
}
