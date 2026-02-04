using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using TMPro;
using Unity.VisualScripting;

public class SliderControl : MonoBehaviour
{
    public string targetTag = "Book Text";
    [Header("UI Sliders")]
    public Slider volumeSlider;
    public Slider brightnessSlider;
    public Slider textsizeSlider;

    [Header("Text Prefab")]
    public GameObject textPrefab;

    [Header("Static Text Elements")]
    public TextMeshProUGUI[] textElements;

    [Header("Post Processing")]
    public Volume volume;

    private ColorAdjustments colorAdjustments;

    // Track all dynamic text instances (e.g., record summaries)
    private List<TextMeshProUGUI> dynamicTextInstances = new List<TextMeshProUGUI>();

    void Start()
    {
        // Load saved values or use defaults
        float savedVolume = SettingsManager.Instance?.CurrentVolume ?? PlayerPrefs.GetFloat("volume", 1f);
        float savedBrightness = PlayerPrefs.GetFloat("brightness", 0f);
        float savedTextSize = SettingsManager.Instance?.CurrentTextSize ?? PlayerPrefs.GetFloat("textSize", 36f);

        // Apply to sliders
        volumeSlider.value = savedVolume;
        brightnessSlider.value = savedBrightness;
        textsizeSlider.value = savedTextSize;

        // Apply system volume
        AudioListener.volume = savedVolume;

        // Apply brightness
        if (volume.profile.TryGet(out colorAdjustments))
        {
            colorAdjustments.postExposure.value = savedBrightness;
        }

        textElements = FindTMPWithTagInHierarchy(targetTag).ToArray();
        // Apply font size to static text elements
        if (textElements != null)
        {
            foreach (var text in textElements)
            {
                text.fontSize = savedTextSize;
            }
        }

        OnVolumeChanged(savedVolume);
        UpdateAllFontSizes(savedTextSize);

        // Add listeners
        volumeSlider.onValueChanged.AddListener(OnVolumeChanged);
        brightnessSlider.onValueChanged.AddListener(SetBrightness);
        textsizeSlider.onValueChanged.AddListener(UpdateAllFontSizes);

        Debug.Log("Volume: " + savedVolume + ", Brightness: " + savedBrightness + ", Text Size: " + savedTextSize);


    }
    public void OnVolumeChanged(float value)
    {
        AudioListener.volume = value;
        //PlayerPrefs.SetFloat("volume", value);
        //PlayerPrefs.Save();
        SettingsManager.Instance.SetVolume(value);
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

    public void UpdateAllFontSizes(float newSize)
    {
        // Update static text
        foreach (var text in textElements)
        {
            text.fontSize = newSize;
        }

        // Update dynamic text
        foreach (var text in dynamicTextInstances)
        {
            if (text != null)
                text.fontSize = newSize;
        }

        //PlayerPrefs.SetFloat("textSize", newSize);
        //PlayerPrefs.Save();
        SettingsManager.Instance.SetTextSize(newSize);
        Debug.Log("Text size updated to: " + newSize);
    }

    // Call this when instantiating a new text prefab
    public void RegisterDynamicText(TextMeshProUGUI text)
    {
        if (text != null && !dynamicTextInstances.Contains(text))
        {
            text.fontSize = textsizeSlider.value;
            dynamicTextInstances.Add(text);
        }
    }

    // Optional: Clear dynamic list when switching pages
    public void ClearDynamicText()
    {
        dynamicTextInstances.Clear();
    }

    public static List<TextMeshProUGUI> FindTMPWithTagInHierarchy(string tag)
    {
        List<TextMeshProUGUI> foundTexts = new List<TextMeshProUGUI>();
        Scene activeScene = SceneManager.GetActiveScene();
        GameObject[] rootObjects = activeScene.GetRootGameObjects();

        foreach (GameObject rootObj in rootObjects)
        {
            TraverseHierarchyAndAddTMP(rootObj.transform, tag, foundTexts);
        }
        return foundTexts;
    }

    private static void TraverseHierarchyAndAddTMP(Transform parent, string tag, List<TextMeshProUGUI> list)
    {
        if (parent.CompareTag(tag))
        {
            TextMeshProUGUI tmp = parent.GetComponent<TextMeshProUGUI>();
            if (tmp != null)
            {
                list.Add(tmp);
            }
        }

        for (int i = 0; i < parent.childCount; i++)
        {
            TraverseHierarchyAndAddTMP(parent.GetChild(i), tag, list);
        }
    }
}