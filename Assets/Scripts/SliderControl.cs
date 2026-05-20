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
    private Dictionary<TextMeshProUGUI, float> baseFontSizes = new Dictionary<TextMeshProUGUI, float>();
    private float newScale;

    void Start()
    {
        float savedVolume = SettingsManager.Instance?.CurrentVolume ?? PlayerPrefs.GetFloat("volume", 1f);
        float savedBrightness = PlayerPrefs.GetFloat("brightness", 0f);
        float savedScale = SettingsManager.Instance?.CurrentTextScale ?? PlayerPrefs.GetFloat("textScale", 1f);

        volumeSlider.value = savedVolume;
        brightnessSlider.value = savedBrightness;
        textsizeSlider.value = savedScale;

        AudioListener.volume = savedVolume;

        if (volume.profile.TryGet(out colorAdjustments))
        {
            colorAdjustments.postExposure.value = savedBrightness;
        }

        textElements = FindTMPWithTagInHierarchy(targetTag).ToArray();

        // Store base sizes BEFORE applying scaling
        foreach (var text in textElements)
        {
            if (text != null && !baseFontSizes.ContainsKey(text))
            {
                baseFontSizes[text] = text.fontSize; // keep the original design size
            }
        }

        // Apply saved scale factor
        UpdateAllFontSizes(savedScale);

        volumeSlider.onValueChanged.AddListener(OnVolumeChanged);
        brightnessSlider.onValueChanged.AddListener(SetBrightness);
        textsizeSlider.onValueChanged.AddListener(UpdateAllFontSizes);

        Debug.Log($"Volume: {savedVolume}, Brightness: {savedBrightness}, Text Scale: {savedScale}");
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
        //Debug.Log("Set brightness: " + value);
        if (colorAdjustments != null)
        {
            colorAdjustments.postExposure.value = value;
            PlayerPrefs.SetFloat("brightness", value);
            PlayerPrefs.Save();
            Debug.Log("brightness: " + value);
        }
    }

    public void UpdateAllFontSizes(float scale)
    {
        foreach (var kvp in baseFontSizes)
        {
            kvp.Key.fontSize = kvp.Value * scale; // scale relative to original
        }

        foreach (var text in dynamicTextInstances)
        {
            if (text != null && baseFontSizes.ContainsKey(text))
                text.fontSize = baseFontSizes[text] * scale;
        }

        SettingsManager.Instance.SetTextScale(scale);
        Debug.Log("Text scale updated to: " + scale);
    }
    // Call this when instantiating a new text prefab
    public void RegisterDynamicText(TextMeshProUGUI text, bool allowScaling = true)
    {
        if (text == null) return;
        dynamicTextInstances.RemoveAll(t => t == null);
        if (text != null && !dynamicTextInstances.Contains(text))
        {
            dynamicTextInstances.Add(text);

            // Store base size for consistency
            if (!baseFontSizes.ContainsKey(text))
            {
                baseFontSizes[text] = text.fontSize;
            }

            // Only apply scaling if allowed
            if (allowScaling)
            {
                float scale = textsizeSlider.value;
                text.fontSize = baseFontSizes[text] * scale;
            }
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