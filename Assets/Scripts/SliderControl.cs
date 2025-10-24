using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;
using TMPro;

public class SliderControl : MonoBehaviour
{
    public Slider volumeSlider;
    public Slider brightnessSlider;
    public Slider textsizeSlider;
    public TextMeshProUGUI[] textElements;
    public Volume volume;

    private ColorAdjustments colorAdjustments;

    void Start()
    {
        // Load saved values or use defaults
        float savedVolume = PlayerPrefs.GetFloat("volume", 1f);
        float savedBrightness = PlayerPrefs.GetFloat("brightness", 0f);
        float savedTextSize = PlayerPrefs.GetFloat("textSize", 36f);

        // Apply to sliders
        volumeSlider.value = savedVolume;
        brightnessSlider.value = savedBrightness;
        textsizeSlider.value = savedTextSize;

        // Apply to system
        AudioListener.volume = savedVolume;

        if (volume.profile.TryGet(out colorAdjustments))
        {
            colorAdjustments.postExposure.value = savedBrightness;
        }

        if (textElements != null)
        {
            UpdateFontSizes(savedTextSize);
        }

        // Add listeners
        volumeSlider.onValueChanged.AddListener(OnVolumeChanged);
        brightnessSlider.onValueChanged.AddListener(SetBrightness);
        textsizeSlider.onValueChanged.AddListener(UpdateFontSizes);

        Debug.Log("Volume: " + savedVolume + ", Brightness: " + savedBrightness + ", Text Size: " + savedTextSize);
    }

    public void OnVolumeChanged(float value)
    {
        AudioListener.volume = value;
        PlayerPrefs.SetFloat("volume", value);
        PlayerPrefs.Save();
    }

    public void SetBrightness(float value)
    {
        if (colorAdjustments != null)
        {
            colorAdjustments.postExposure.value = value;
            PlayerPrefs.SetFloat("brightness", value);
            PlayerPrefs.Save();
        }
    }

    public void UpdateFontSizes(float newSize)
    {
        foreach (var text in textElements)
        {
            text.fontSize = newSize;
        }
        PlayerPrefs.SetFloat("textSize", newSize);
        PlayerPrefs.Save();
    }
}