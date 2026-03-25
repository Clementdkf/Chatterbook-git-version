using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;

public class RecordSummary : MonoBehaviour
{
    public SliderControl sliderControl;
    
    [Header("Individual text prefabs")]
    public GameObject sceneNamePrefab;
    public GameObject correctTextPrefab;
    public GameObject correctNumberPrefab;
    public GameObject wrongTextPrefab;
    public GameObject wrongNumberPrefab;

    [Header("Container for all Groups")]
    public Transform contentContainer;
    private List<GameObject> recordGroups = new List<GameObject>();

    [Header("Buttons")]
    public Button PreviousButton;
    public Button NextButton; 
    private int currentPage = 0;
    private const int itemsPerPage = 4;

    void Start()
    {
        UpdateRecordPage();
        UpdateSceneResults();
        ReceivingRecords.OnQuizDataReset += UpdateSceneResults;
    }

    void OnDestroy()
    {
        ReceivingRecords.OnQuizDataReset -= UpdateSceneResults;
    }

    void UpdateRecordPage()
    {
        PreviousButton.gameObject.SetActive(currentPage > 0);
        NextButton.gameObject.SetActive(currentPage < (ReceivingRecords.Instance.allSceneQuizData.Count - 1) / itemsPerPage);
    }


    //controlling the record data for each scene and show them in the record panel
    public void UpdateSceneResults()
    {
        var allSceneQuizData = ReceivingRecords.Instance.allSceneQuizData; // Get all quiz data

        // Clear previous text elements
        foreach (var textElement in recordGroups)
        {
            Destroy(textElement.gameObject); // Destroy the GameObjects
        }
        recordGroups.Clear(); // Clear the list

        // Display current page items
        for (int i = 0; i < itemsPerPage; i++)
        {
            int dataIndex = currentPage * itemsPerPage + i; // Calculate the index in the full data list
            if (dataIndex >= allSceneQuizData.Count) break;

            var sceneData = allSceneQuizData[dataIndex];

            //Create a new group empty row container
            GameObject row = new GameObject("SceneRow", typeof(RectTransform));
            row.transform.SetParent(contentContainer, false);
            var rowRect = row.GetComponent<RectTransform>();
            rowRect.sizeDelta = new UnityEngine.Vector2(600, 40); //adjust width/height
            recordGroups.Add(row);

            //formatting for the scene name prefab
            GameObject nameObj = Instantiate(sceneNamePrefab, row.transform);
            var nameText = nameObj.GetComponent<TextMeshProUGUI>();
            sliderControl.RegisterDynamicText(nameText);
            nameText.fontSize = 28;
            nameText.text = sceneData.sceneName + ": ";

            //formatting for the correct text prefab
            GameObject correctObj = Instantiate(correctTextPrefab, row.transform);
            var correctText = correctObj.GetComponent<TextMeshProUGUI>();
            sliderControl.RegisterDynamicText(correctText);
            correctText.fontSize = 28;
            correctText.text = $"正確: ";

            //formatting for the correct number prefab
            GameObject correctNumberObj = Instantiate(correctNumberPrefab, row.transform);
            var correctNumber = correctNumberObj.GetComponent<TextMeshProUGUI>();
            sliderControl.RegisterDynamicText(correctNumber);
            correctNumber.fontSize = 28;
            correctNumber.text = $"{sceneData.correctCount}";

            //formatting for the wrong text prefab
            GameObject wrongObj = Instantiate(wrongTextPrefab, row.transform);
            var wrongText = wrongObj.GetComponent<TextMeshProUGUI>();
            sliderControl.RegisterDynamicText(wrongText);
            wrongText.fontSize = 28;
            wrongText.text = $"錯誤: ";

            //formatting for the wrong number prefab
            GameObject wrongNumberObj = Instantiate(wrongNumberPrefab, row.transform);
            var wrongNumber = wrongNumberObj.GetComponent<TextMeshProUGUI>();
            sliderControl.RegisterDynamicText(wrongNumber);
            wrongNumber.fontSize = 28;
            wrongNumber.text = $"{sceneData.wrongCount}";

            //Manual Positioning
            var nameRect = nameObj.GetComponent<RectTransform>();
            var correctRect = correctObj.GetComponent<RectTransform>();
            var correctNumberRect = correctNumberObj.GetComponent<RectTransform>();
            var wrongRect = wrongObj.GetComponent<RectTransform>();
            var WrongNumberRect = wrongNumberObj.GetComponent<RectTransform>();

            nameRect.anchoredPosition = new UnityEngine.Vector2(-250, 0);
            correctRect.anchoredPosition = new UnityEngine.Vector2(100, 0);
            correctNumberRect.anchoredPosition = new UnityEngine.Vector2(250, 3);
            wrongRect.anchoredPosition = new UnityEngine.Vector2(250, 0); 
            WrongNumberRect.anchoredPosition = new UnityEngine.Vector2(400, 3);
        }
    } 

    //move to the previous page of the record panel
    public void PreviousPage()
    {
        if (currentPage > 0)
        {
            currentPage--;
            UpdateRecordPage();
            UpdateSceneResults();
        }
    }

    //move to the next page of the record panel
    public void NextPage()
    {
        int maxPage = (ReceivingRecords.Instance.allSceneQuizData.Count - 1) / itemsPerPage;
        if (currentPage < maxPage)
        {
            currentPage++;
            UpdateRecordPage();
            UpdateSceneResults();
        }
    }


}
