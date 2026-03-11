using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;

public class RecordSummary : MonoBehaviour
{
    public SliderControl sliderControl;
    
    [Header("Record text prefabs")]
    public GameObject recordTextPrefab;
    public Transform contentContainer;
    private List<TextMeshProUGUI> recordTextObjects = new List<TextMeshProUGUI>();

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

    public void UpdateSceneResults()
    {
        var allSceneQuizData = ReceivingRecords.Instance.allSceneQuizData; // Get all quiz data

        // Clear previous text elements
        foreach (var textElement in recordTextObjects)
        {
            Destroy(textElement.gameObject); // Destroy the GameObjects
        }
        recordTextObjects.Clear(); // Clear the list

        // Display current page items
        for (int i = 0; i < itemsPerPage; i++)
        {
            int dataIndex = currentPage * itemsPerPage + i; // Calculate the index in the full data list

            GameObject newTextObj = Instantiate(recordTextPrefab, contentContainer); // Create new text object
            TextMeshProUGUI textComponent = newTextObj.GetComponent<TextMeshProUGUI>();
            sliderControl.RegisterDynamicText(textComponent);// Refresh the text prefab settings

            if (dataIndex < allSceneQuizData.Count)
            {
                var sceneData = allSceneQuizData[dataIndex]; // Get the corresponding scene data
                textComponent.fontSize = 28;
                textComponent.text = $"{sceneData.sceneName}:<space=25>正確: {sceneData.correctCount,2}<space=20>錯誤: {sceneData.wrongCount,2}"; // Set text with formatting
            }

            recordTextObjects.Add(textComponent); // Add to the list for future reference
        }
    } 

    public void PreviousPage()
    {
        if (currentPage > 0)
        {
            currentPage--;
            UpdateRecordPage();
            UpdateSceneResults();
        }
    }

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
