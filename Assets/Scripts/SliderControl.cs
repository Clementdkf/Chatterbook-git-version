using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;
using TMPro;

public class SliderControl : MonoBehaviour
{
    [Header("Volume")]
    public Volume volume;
    private ColorAdjustments colorAdjustments;

    [Header("Text")]
    public TextMeshProUGUI[] textElements;
    [Header("UI Sliders")]
    public Slider volumeSlider;
    public Slider brightnessSlider;
    public Slider textsizeSlider;
    [Header("Brightness Overlays")]

    public Image blackOverlay; // Reference to the black overlay image
    public Image whiteOverlay; // Reference to the white overlay image
    [Header("Overlay Settings")]
    [Range(0f, 1f)] public float maxOverlayAlpha = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        volumeSlider.value = PlayerPrefs.GetFloat("Volume", 1f); // Load saved volume or default to 1
        volumeSlider.onValueChanged.AddListener(OnVolumeChanged);
        brightnessSlider.onValueChanged.AddListener(SetBrightness);
        textsizeSlider.onValueChanged.AddListener(UpdateFontSizes);
        UpdateFontSizes(textsizeSlider.value);
        if (volume.profile.TryGet(out colorAdjustments))
        {
            colorAdjustments.postExposure.value = 0f; // Default brightness
        }
    }
    public void OnVolumeChanged(float value)
    {
        AudioListener.volume = value; // Adjust the global audio volume based on slider value
        PlayerPrefs.SetFloat("Volume", value); // Save the volume setting
        Debug.Log("Volume changed to: " + value); // Log the volume change for debugging
    }

    public void SetBrightness(float value)
    {
        if (colorAdjustments != null)
        {
            colorAdjustments.postExposure.value = value; // Map slider value to a suitable range
        }
        else
        {
            Debug.LogWarning("ColorAdjustments not found in the volume profile.");
        }
        Debug.Log("Post Exposure value: " + colorAdjustments.postExposure.value);
    }

    public void UpdateFontSizes(float newSize)
    {
        foreach (TextMeshProUGUI textElement in textElements)
        {
            textElement.fontSize = newSize;
        }
        Debug.Log("Text size updated to: " + newSize);
    }
}
