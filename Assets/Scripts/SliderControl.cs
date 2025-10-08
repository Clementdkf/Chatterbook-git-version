using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;

public class SliderControl : MonoBehaviour
{
    [Header("Volume")]
    public Volume volume;
    private ColorAdjustments colorAdjustments;
    [Header("UI Sliders")]
    public Slider volumeSlider;
    public Slider brightnessSlider;
    [Header("Brightness Overlays")]

    public Image blackOverlay; // Reference to the black overlay image
    public Image whiteOverlay; // Reference to the white overlay image
    [Header("Overlay Settings")]
    [Range(0f, 1f)] public float maxOverlayAlpha = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        // Set slider range and default value
        /*brightnessSlider.minValue = 0f;
        brightnessSlider.maxValue = 1f;
        brightnessSlider.value = 0.5f; // Neutral brightness*/

        volumeSlider.value = PlayerPrefs.GetFloat("Volume", 1f); // Load saved volume or default to 1
        volumeSlider.onValueChanged.AddListener(OnVolumeChanged);
        brightnessSlider.onValueChanged.AddListener(SetBrightness);
        if (volume.profile.TryGet(out colorAdjustments))
        {
            colorAdjustments.postExposure.value = 0f; // Default brightness
        }
    }

    // Update is called once per frame
    void Update()
    {

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
        // Calculate blend: 0 = full black, 0.5 = neutral, 1 = full white
        /*float blackAlpha = Mathf.Clamp01(1f - value) * maxOverlayAlpha;
        float whiteAlpha = Mathf.Clamp01(value - 0.5f) * (maxOverlayAlpha * 2f);

        if (blackOverlay != null)
        {
            Color blackColor = blackOverlay.color;
            blackColor.a = blackAlpha;
            blackOverlay.color = blackColor;
        }

        if (whiteOverlay != null)
        {
            Color whiteColor = whiteOverlay.color;
            whiteColor.a = whiteAlpha;
            whiteOverlay.color = whiteColor;
        }*/

    }
}
