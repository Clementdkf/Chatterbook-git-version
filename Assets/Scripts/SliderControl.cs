using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using TMPro;

public class SliderControl : MonoBehaviour
{
    public string targetTag = "Book Text";
    [Header("UI Sliders")]
    public Slider volumeSlider;
    public Slider brightnessSlider;
    public Slider textsizeSlider;

    [Header("Post Processing")]
    public Volume volume;

    private ColorAdjustments colorAdjustments;

    private List<TextMeshProUGUI> dynamicTextInstances = new List<TextMeshProUGUI>();
    private Dictionary<TextMeshProUGUI, float> baseFontSizes = new Dictionary<TextMeshProUGUI, float>();

    void Awake()
    {
        // Subscribe to scene load event
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDestroy()
    {
        // Unsubscribe to avoid leaks
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

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

        // Initial refresh
        UpdateAllFontSizes(savedScale);

        volumeSlider.onValueChanged.AddListener(OnVolumeChanged);
        brightnessSlider.onValueChanged.AddListener(SetBrightness);
        textsizeSlider.onValueChanged.AddListener(UpdateAllFontSizes);

        Debug.Log($"Volume: {savedVolume}, Brightness: {savedBrightness}, Text Scale: {savedScale}");
    }

    void OnEnable()
    {
        float savedScale = SettingsManager.Instance?.CurrentTextScale ?? PlayerPrefs.GetFloat("textScale", 1f);
        UpdateAllFontSizes(savedScale);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        float savedScale = SettingsManager.Instance?.CurrentTextScale ?? PlayerPrefs.GetFloat("textScale", 1f);
        UpdateAllFontSizes(savedScale);
    }

    public void OnVolumeChanged(float value)
    {
        AudioListener.volume = value;
        SettingsManager.Instance.SetVolume(value);
    }

    public void SetBrightness(float value)
    {
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
        // Refresh static texts each time
        var textElements = FindTMPWithTagInHierarchy(targetTag);

        foreach (var text in textElements)
        {
            if (text != null && !baseFontSizes.ContainsKey(text))
            {
                baseFontSizes[text] = text.fontSize;
            }
        }

        // Apply scaling to all static texts
        foreach (var text in textElements)
        {
            if (text != null && baseFontSizes.ContainsKey(text))
            {
                text.fontSize = baseFontSizes[text] * scale;
            }
        }

        // Apply scaling to dynamic texts
        foreach (var text in dynamicTextInstances)
        {
            if (text != null && baseFontSizes.ContainsKey(text))
                text.fontSize = baseFontSizes[text] * scale;
        }

        SettingsManager.Instance.SetTextScale(scale);
        Debug.Log("Text scale updated to: " + scale);
    }

    public void RegisterDynamicText(TextMeshProUGUI text, bool allowScaling = true)
    {
        if (text == null) return;
        dynamicTextInstances.RemoveAll(t => t == null);

        if (!dynamicTextInstances.Contains(text))
        {
            dynamicTextInstances.Add(text);

            if (!baseFontSizes.ContainsKey(text))
            {
                baseFontSizes[text] = text.fontSize;
            }

            if (allowScaling)
            {
                float scale = SettingsManager.Instance?.CurrentTextScale ?? textsizeSlider.value;
                text.fontSize = baseFontSizes[text] * scale;
            }
        }
    }

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
