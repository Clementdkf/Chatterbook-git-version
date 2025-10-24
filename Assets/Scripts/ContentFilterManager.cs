using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ContentFilterManager : MonoBehaviour
{
    public TMP_Dropdown filterDropdown;
    public Transform buttonContainer;
    public GameObject itemButtonPrefab;
    public Button nextPageButton;
    public Button previousPageButton;

    public List<ContentItem> allItems = new List<ContentItem>();
    private List<ContentItem> currentFilteredItems = new List<ContentItem>();

    private int currentPage = 0;
    private const int itemsPerPage = 4;

    void Start()
    {
        SetupDropdown();
        filterDropdown.onValueChanged.AddListener(ApplyFilter);
        nextPageButton.onClick.AddListener(NextPage);
        previousPageButton.onClick.AddListener(PreviousPage);

        ApplyFilter(0); // Default to "全部"
    }

    void SetupDropdown()
    {
        filterDropdown.ClearOptions();
        filterDropdown.AddOptions(new List<string> {
            "全部", "最受歡迎", "最新", "堅毅", "尊重他人", "責任感", "國民身份認同", "承擔精神", "誠信",
            "仁愛", "守法", "同理心", "勤勞", "團結", "孝親", "其他正確價值觀"
        });
    }

    void ApplyFilter(int index)
    {
        currentPage = 0;

        currentFilteredItems = index switch
        {
            0 => allItems,
            1 => allItems.OrderByDescending(i => i.popularityScore).Take(1).ToList(),
            2 => allItems.OrderByDescending(i => i.ParsedDate).ToList(),
            _ => allItems.Where(i => i.valueTags.Contains(filterDropdown.options[index].text)).ToList()
        };

        UpdatePage();
    }

    void UpdatePage()
    {
        foreach (Transform child in buttonContainer)
            Destroy(child.gameObject); // Clear existing buttons

        int startIndex = currentPage * itemsPerPage; // Calculate start index based on current page
        int endIndex = Mathf.Min(startIndex + itemsPerPage, currentFilteredItems.Count); // Ensure we don't exceed the list count

        for (int i = startIndex; i < endIndex; i++)
        {
            GameObject btn = Instantiate(itemButtonPrefab, buttonContainer); // Create button
            btn.GetComponent<ItemButtonUI>().Setup(currentFilteredItems[i]); // Setup button with item data
        }

        //previousPageButton.interactable = currentPage > 0;
        //nextPageButton.interactable = endIndex < currentFilteredItems.Count;
        previousPageButton.gameObject.SetActive(currentPage > 0);
        nextPageButton.gameObject.SetActive(endIndex < currentFilteredItems.Count);
    }

    public void NextPage()
    {
        int maxPage = Mathf.CeilToInt((float)currentFilteredItems.Count / itemsPerPage) - 1;
        if (currentPage < maxPage)
        {
            currentPage++;
            UpdatePage();
            //Debug.Log("Current Page: " + currentPage);
            //Debug.Log("Max Page: " + maxPage);
        }
    }

    public void PreviousPage()
    {
        if (currentPage > 0)
        {
            currentPage--;
            UpdatePage();
        }
    }
}

[System.Serializable]
public class ContentItem
{
    public string title;
    public string dateString; // Assignable in Inspector, e.g. "2025-09-19"
    public int popularityScore;
    public Sprite image;
    public string sceneName;
    public List<string> valueTags = new List<string>();

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
