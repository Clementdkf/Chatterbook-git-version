using UnityEngine;
using UnityEngine.Localization.Settings;
using TMPro;
using System.Collections.Generic;

public class DynamicFontManager : MonoBehaviour
{
    public static DynamicFontManager Instance;
    
    [Header("Font Assets")]
    public TMP_FontAsset englishFont;
    public TMP_FontAsset chineseFont;

    // Keep track of all dynamic text fields
    public List<TextMeshProUGUI> dynamicTexts = new List<TextMeshProUGUI>();

    void OnEnable()
    {
        LocalizationSettings.SelectedLocaleChanged += OnLocaleChanged;
    }

    void OnDisable()
    {
        LocalizationSettings.SelectedLocaleChanged -= OnLocaleChanged;
    }

    public void OnLocaleChanged(UnityEngine.Localization.Locale newLocale)
    {
        dynamicTexts.RemoveAll(t => t == null);
        TMP_FontAsset targetFont = englishFont;

        if (newLocale.Identifier.Code.StartsWith("zh")) // Chinese locale
        {
            targetFont = chineseFont;
        }

        //dynamicTexts.RemoveAll(t => t == null);
        foreach (var tmp in dynamicTexts)
        {
            if (tmp != null)
            {
                //tmp.text = "";
                tmp.font = targetFont;
            }
        }

        Debug.Log($"Locale changed to {newLocale.Identifier.Code}, applying font {targetFont.name} to {dynamicTexts.Count} texts");

    }

    // Call this when you create new TMP objects dynamically
    public void RegisterText(TextMeshProUGUI tmp)
    {
        if (tmp == null) return;
        dynamicTexts.RemoveAll(t => t == null);
        if (!dynamicTexts.Contains(tmp))
        {
            dynamicTexts.Add(tmp);
        }
    }
    public void ClearTexts()
    {
        dynamicTexts.Clear();
    }

}
