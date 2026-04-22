using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

[RequireComponent(typeof(TextMeshProUGUI))]
public class LocalizedTextLayoutRefresher : MonoBehaviour
{
    private TextMeshProUGUI tmpText;

    void Awake()
    {
        tmpText = GetComponent<TextMeshProUGUI>();
    }

    void OnEnable()
    {
        // Subscribe to locale change events
        LocalizationSettings.SelectedLocaleChanged += OnLocaleChanged;
    }

    void OnDisable()
    {
        LocalizationSettings.SelectedLocaleChanged -= OnLocaleChanged;
    }

    private void OnLocaleChanged(Locale newLocale)
    {
        // Force layout rebuild after text swap
        if (tmpText != null)
        {
            LayoutRebuilder.ForceRebuildLayoutImmediate(tmpText.rectTransform);
        }
    }

    // Optional: also refresh when text changes dynamically
    void LateUpdate()
    {
        if (tmpText.isTextOverflowing || tmpText.havePropertiesChanged)
        {
            LayoutRebuilder.ForceRebuildLayoutImmediate(tmpText.rectTransform);
        }
    }
}
