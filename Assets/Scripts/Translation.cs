using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization.Settings;

public class Translation : MonoBehaviour
{
    public void ChiLocale()
    {
        var chineseLocale = LocalizationSettings.AvailableLocales.GetLocale("zh-Hant");
        LocalizationSettings.SelectedLocale = chineseLocale;
        Debug.Log("Switched to Chinese");
    }

    public void EngLocale()
    {
        var englishLocale = LocalizationSettings.AvailableLocales.GetLocale("en");
        LocalizationSettings.SelectedLocale = englishLocale;
        Debug.Log("Switched to English");
    }

    
}
