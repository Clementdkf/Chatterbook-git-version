using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

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

    // Separate lists for dynamic texts
    private List<TextMeshProUGUI> scalableDynamicTexts = new List<TextMeshProUGUI>();
    private List<TextMeshProUGUI> fixedDynamicTexts = new List<TextMeshProUGUI>();

    // Cache of original font sizes per TMP object and per font asset
    private Dictionary<TextMeshProUGUI, Dictionary<TMP_FontAsset, float>> baseFontSizes 
        = new Dictionary<TextMeshProUGUI, Dictionary<TMP_FontAsset, float>>();

    void Awake()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        LocalizationSettings.SelectedLocaleChanged += OnLocaleChanged;
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        LocalizationSettings.SelectedLocaleChanged -= OnLocaleChanged;
    }

    void Start()
    {
        float savedVolume = SettingsManager.Instance.CurrentVolume;
        float savedBrightness = PlayerPrefs.GetFloat("brightness", 0f);
        float savedScale = SettingsManager.Instance?.CurrentTextScale ?? PlayerPrefs.GetFloat("textScale", 1f);

        if (volumeSlider != null) volumeSlider.value = savedVolume;
        if (brightnessSlider != null) brightnessSlider.value = savedBrightness;
        if (textsizeSlider != null) textsizeSlider.value = savedScale;

        AudioListener.volume = savedVolume;

        if (volume != null && volume.profile.TryGet(out colorAdjustments))
        {
            colorAdjustments.postExposure.value = savedBrightness;
        }

        CacheBaseFontSizes();

        if (volumeSlider != null)
        {
            volumeSlider.onValueChanged.RemoveListener(OnVolumeChanged);
            volumeSlider.value = savedVolume;
            volumeSlider.minValue = 0f;
            volumeSlider.maxValue = 1f;
            volumeSlider.wholeNumbers = false;
            AudioListener.volume = savedVolume;
            volumeSlider.onValueChanged.AddListener(OnVolumeChanged);
        }

        if (brightnessSlider != null) brightnessSlider.onValueChanged.AddListener(SetBrightness);
        if (textsizeSlider != null) textsizeSlider.onValueChanged.AddListener(UpdateAllFontSizes);

        Debug.Log($"Volume: {savedVolume}, Brightness: {savedBrightness}, Text Scale: {savedScale}");
    }

    void OnEnable()
    {
        float savedScale = SettingsManager.Instance?.CurrentTextScale ?? PlayerPrefs.GetFloat("textScale", 1f);
        UpdateAllFontSizes(savedScale);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        CacheBaseFontSizes();
        float savedScale = SettingsManager.Instance?.CurrentTextScale ?? PlayerPrefs.GetFloat("textScale", 1f);
        if (textsizeSlider != null)
            textsizeSlider.value = savedScale;
        UpdateAllFontSizes(savedScale);

        float savedVolume = SettingsManager.Instance.CurrentVolume;
        if (volumeSlider != null)
        {
            volumeSlider.onValueChanged.RemoveListener(OnVolumeChanged);
            volumeSlider.value = savedVolume;
            AudioListener.volume = savedVolume;
            Debug.Log("I am in OnSceneLoaded and volumeSlider.value is: " + volumeSlider.value);
            Debug.Log("I am in OnSceneLoaded and savedVolume is: " + savedVolume);
            Debug.Log("I am in OnSceneLoaded and AudioListener.volume is: " + AudioListener.volume);
            volumeSlider.onValueChanged.AddListener(OnVolumeChanged);
        }
    }

    private void OnLocaleChanged(Locale locale)
    {
        Debug.Log($"Locale changed to: {locale.Identifier.Code}");
        CacheBaseFontSizes(); // refresh base sizes for new locale/font asset
        float savedScale = SettingsManager.Instance?.CurrentTextScale ?? PlayerPrefs.GetFloat("textScale", 1f);
        UpdateAllFontSizes(savedScale);
    }

    private void CacheBaseFontSizes()
    {
        var textElements = FindTMPWithTagInHierarchy(targetTag);
        foreach (var text in textElements)
        {
            if (text == null) continue;

            TMP_FontAsset currentFont = text.font;
            if (!baseFontSizes.ContainsKey(text))
                baseFontSizes[text] = new Dictionary<TMP_FontAsset, float>();

            if (!baseFontSizes[text].ContainsKey(currentFont))
            {
                baseFontSizes[text][currentFont] = text.fontSize; // store base size for this font asset
                Debug.Log($"Cached base size for {text.name} with {currentFont.name}: {text.fontSize}");
            }
        }
    }

    public void OnVolumeChanged(float value)
    {
        AudioListener.volume = value;
        //SettingsManager.Instance.SetVolume(value);
        Debug.Log("I am in the OnVolumeChanged method and the value is: " + value);
    }

    public void OnVolumeSet()
    {
        SettingsManager.Instance.SetVolume(volumeSlider.value);
    }

    public void SetBrightness(float value)
    {
        if (colorAdjustments != null)
        {
            colorAdjustments.postExposure.value = value;
            PlayerPrefs.SetFloat("brightness", value);
            PlayerPrefs.Save();
            Debug.Log("Brightness: " + value);
        }
    }

    public void UpdateAllFontSizes(float scale)
    {
        // Apply scaling to static texts
        foreach (var kvp in baseFontSizes)
        {
            TextMeshProUGUI text = kvp.Key;
            if (text != null && text.CompareTag(targetTag))
            {
                TMP_FontAsset currentFont = text.font;

                // Lazy caching: if missing, store base size now
                if (!kvp.Value.ContainsKey(currentFont))
                    kvp.Value[currentFont] = text.fontSize;

                text.fontSize = kvp.Value[currentFont] * scale;
            }
        }

        // Apply scaling only to scalable dynamic texts
        foreach (var text in scalableDynamicTexts)
        {
            if (text != null && baseFontSizes.ContainsKey(text))
            {
                TMP_FontAsset currentFont = text.font;

                // Lazy caching: if missing, store base size now
                if (!baseFontSizes[text].ContainsKey(currentFont))
                    baseFontSizes[text][currentFont] = text.fontSize;

                text.fontSize = baseFontSizes[text][currentFont] * scale;
            }
        }

        SettingsManager.Instance?.SetTextScale(scale);
    }

    public void RegisterDynamicText(TextMeshProUGUI text, bool allowScaling = true)
    {
        if (text == null) return;

        TMP_FontAsset currentFont = text.font;
        if (!baseFontSizes.ContainsKey(text))
            baseFontSizes[text] = new Dictionary<TMP_FontAsset, float>();

        if (!baseFontSizes[text].ContainsKey(currentFont))
            baseFontSizes[text][currentFont] = text.fontSize;

        if (allowScaling)
        {
            if (!scalableDynamicTexts.Contains(text))
                scalableDynamicTexts.Add(text);

            float scale = SettingsManager.Instance?.CurrentTextScale ?? textsizeSlider.value;
            text.fontSize = baseFontSizes[text][currentFont] * scale;
        }
        else
        {
            if (!fixedDynamicTexts.Contains(text))
                fixedDynamicTexts.Add(text);
        }
    }

    public void ClearDynamicText()
    {
        scalableDynamicTexts.Clear();
        fixedDynamicTexts.Clear();
    }

    public static List<TextMeshProUGUI> FindTMPWithTagInHierarchy(string tag)
    {
        List<TextMeshProUGUI> foundTexts = new List<TextMeshProUGUI>();
        Scene activeScene = SceneManager.GetActiveScene();
        GameObject[] rootObjects = activeScene.GetRootGameObjects();

        foreach (GameObject rootObj in rootObjects)
            TraverseHierarchyAndAddTMP(rootObj.transform, tag, foundTexts);

        return foundTexts;
    }

    private static void TraverseHierarchyAndAddTMP(Transform parent, string tag, List<TextMeshProUGUI> list)
    {
        if (parent.CompareTag(tag))
        {
            TextMeshProUGUI tmp = parent.GetComponent<TextMeshProUGUI>();
            if (tmp != null) list.Add(tmp);
        }

        for (int i = 0; i < parent.childCount; i++)
            TraverseHierarchyAndAddTMP(parent.GetChild(i), tag, list);
    }

    void LateUpdate()
    {
        float savedScale = SettingsManager.Instance?.CurrentTextScale ?? PlayerPrefs.GetFloat("textScale", 1f);
        UpdateAllFontSizes(savedScale);
    }
}
