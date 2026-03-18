using UnityEngine;

public class SettingsManager : MonoBehaviour
{
    public static SettingsManager Instance { get; private set; }
    public float CurrentTextScale { get; private set; } = 1f; // Default = 100%

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
            DontDestroyOnLoad(this.gameObject);
            CurrentTextScale = PlayerPrefs.GetFloat("textScale", 1f);
            CurrentVolume = PlayerPrefs.GetFloat("volume", 1f);
        }
    }

    //saving the newly set text scale to playerPrefs
    public void SetTextScale(float newScale)
    {
        CurrentTextScale = newScale;
        PlayerPrefs.SetFloat("textScale", newScale);
        PlayerPrefs.Save();
        // Optional: trigger event to update all texts
    }

    //saving the newly set volume to playerPrefs
    public void SetVolume(float newVolume)
    {
        CurrentVolume = newVolume;
        PlayerPrefs.SetFloat("volume", newVolume);
        PlayerPrefs.Save();
        AudioListener.volume = newVolume;
    }
}