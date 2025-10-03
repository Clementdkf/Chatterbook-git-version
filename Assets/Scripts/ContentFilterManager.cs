using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;

public class ContentFilterManager : MonoBehaviour
{
    public TMP_Dropdown filterDropdown;
    public Transform buttonContainer;
    public GameObject itemButtonPrefab;
    public List<ContentItem> allItems = new List<ContentItem>();

    void Start()
    {
        SetupDropdown();
        ApplyFilter(0); // Default to "All"
    }

    void SetupDropdown()
    {
        filterDropdown.ClearOptions();
        filterDropdown.AddOptions(new List<string> { "全部", "最受歡迎", "最新", "堅毅", "尊重他人", "責任感", "國民身份認同", "承擔精神", "誠信", 
  "仁愛", "守法", "同理心", "勤勞", "團結", "孝親", "其他正確價值觀"});
        filterDropdown.onValueChanged.AddListener(ApplyFilter);
    }

    void ApplyFilter(int index)
    {
        List<ContentItem> filteredList = index switch
        {
            0 => allItems,
            1 => allItems.OrderByDescending(i => i.popularityScore).Take(1).ToList(),
            2 => allItems.OrderByDescending(i => i.ParsedDate).ToList(),
            _ => allItems
        };

        foreach (Transform child in buttonContainer)
            Destroy(child.gameObject);

        foreach (var item in filteredList)
        {
            GameObject btn = Instantiate(itemButtonPrefab, buttonContainer);
            btn.GetComponent<ItemButtonUI>().Setup(item);
        }
    }
}

[System.Serializable]
public class ContentItem
{
    public string title;
    public string dateString; // Assignable in Inspector, e.g. "2025-09-19"
    public int popularityScore;
    public string sceneName;

    public DateTime ParsedDate
    {
        get
        {
            DateTime parsed;
            if (DateTime.TryParse(dateString, out parsed))
                return parsed;
            return DateTime.MinValue; // fallback if parsing fails
        }
    }
}
