using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;
using UnityEngine.Localization.Settings;

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

    [Header("Dynamic font")]
    public DynamicFontManager fontManager;
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

    void OnEnable()
    {
        LocalizationSettings.SelectedLocaleChanged += OnLocaleChanged;
    }

    private void OnLocaleChanged(Locale newLocale)
    {
        UpdateSceneResults();
    }

    void OnDisable()
    {
        
    }

    void UpdateRecordPage() //controls the visibility of the next and previous buttons on the record page
    {
        PreviousButton.gameObject.SetActive(currentPage > 0);
        NextButton.gameObject.SetActive(currentPage < (ReceivingRecords.Instance.allSceneQuizData.Count - 1) / itemsPerPage);
    }

    // controlling the record data for each scene and show them in the record panel
    public void UpdateSceneResults()
    {
        fontManager.ClearTexts();
        sliderControl.ClearDynamicText();
        var allSceneQuizData = ReceivingRecords.Instance.allSceneQuizData;

        // Clear previous text elements
        foreach (var textElement in recordGroups)
        {
            Destroy(textElement.gameObject);
        }
        recordGroups.Clear();

        // Display current page items
        for (int i = 0; i < itemsPerPage; i++)
        {
            int dataIndex = currentPage * itemsPerPage + i;
            if (dataIndex >= allSceneQuizData.Count) break;

            var sceneData = allSceneQuizData[dataIndex];

            // Create a new group empty row container
            GameObject row = new GameObject("SceneRow", typeof(RectTransform));
            row.transform.SetParent(contentContainer, false);
            var rowRect = row.GetComponent<RectTransform>();
            rowRect.sizeDelta = new Vector2(600, 40);
            recordGroups.Add(row);

            // Scene name (localized)
            GameObject nameObj = Instantiate(sceneNamePrefab, row.transform);
            var nameText = nameObj.GetComponent<TextMeshProUGUI>();
            sliderControl.RegisterDynamicText(nameText, false);
            fontManager.RegisterText(nameText);
            nameText.fontSize = 25;

            // Bind localized scene name
            sceneData.LocalizedSceneName.StringChanged += (val) =>
            {
                nameText.text = val;
            };

            // Correct text (localized)
            GameObject correctObj = Instantiate(correctTextPrefab, row.transform);
            var correctText = correctObj.GetComponent<TextMeshProUGUI>();
            sliderControl.RegisterDynamicText(correctText, false);
            fontManager.RegisterText(correctText);
            correctText.fontSize = 25;

            sceneData.localizedCorrectLabel.StringChanged += (val) =>
            {
                correctText.text = val + ": ";
            };

            // Correct number
            GameObject correctNumberObj = Instantiate(correctNumberPrefab, row.transform);
            var correctNumber = correctNumberObj.GetComponent<TextMeshProUGUI>();
            sliderControl.RegisterDynamicText(correctNumber, false);
            //fontManager.RegisterText()
            correctNumber.fontSize = 25;
            correctNumber.text = $"{sceneData.correctCount}";

            // Wrong text (localized)
            GameObject wrongObj = Instantiate(wrongTextPrefab, row.transform);
            var wrongText = wrongObj.GetComponent<TextMeshProUGUI>();
            sliderControl.RegisterDynamicText(wrongText, false);
            fontManager.RegisterText(wrongText);
            wrongText.fontSize = 25;

            sceneData.localizedWrongLabel.StringChanged += (val) =>
            {
                wrongText.text = val + ": ";
            };

            // Wrong number
            GameObject wrongNumberObj = Instantiate(wrongNumberPrefab, row.transform);
            var wrongNumber = wrongNumberObj.GetComponent<TextMeshProUGUI>();
            sliderControl.RegisterDynamicText(wrongNumber, false);
            wrongNumber.fontSize = 25;
            wrongNumber.text = $"{sceneData.wrongCount}";

            fontManager.OnLocaleChanged(LocalizationSettings.SelectedLocale);

            // Manual Positioning
            var nameRect = nameObj.GetComponent<RectTransform>();
            var correctRect = correctObj.GetComponent<RectTransform>();
            var correctNumberRect = correctNumberObj.GetComponent<RectTransform>();
            var wrongRect = wrongObj.GetComponent<RectTransform>();
            var WrongNumberRect = wrongNumberObj.GetComponent<RectTransform>();


            var baseNamePos = new Vector2(-250, 0);
            var baseCorrectPos = new Vector2(100, 0);
            var baseCorrectNumPos = new Vector2(250, 3);
            var baseWrongPos = new Vector2(250, 0);
            var baseWrongNumPos = new Vector2(400, 3);

            Vector2 instanceOffset = Vector2.zero;
            var code = LocalizationSettings.SelectedLocale.Identifier.Code;
            Debug.Log("Locale code: " + code);
            if (code.Contains("zh"))
            {
                baseNamePos = new Vector2(-250, 0);
                baseCorrectPos = new Vector2(100, 0);
                baseCorrectNumPos = new Vector2(250, 5);
                baseWrongPos = new Vector2(255, 0);
                baseWrongNumPos = new Vector2(400, 5);
            }
            else if (code.Contains("en"))
            {
                baseNamePos = new Vector2(-250, -3);   // shift scene name right
                baseCorrectPos = new Vector2(50, -3);    // move "Correct" label further right
                baseCorrectNumPos = new Vector2(250, 5);  // adjust number position
                baseWrongPos = new Vector2(220, -3);    // move "Wrong" label
                baseWrongNumPos = new Vector2(400, 5);    // adjust wrong number
            }

            nameRect.anchoredPosition = baseNamePos;
            correctRect.anchoredPosition = baseCorrectPos + instanceOffset;
            correctNumberRect.anchoredPosition = baseCorrectNumPos;
            wrongRect.anchoredPosition = baseWrongPos;
            WrongNumberRect.anchoredPosition = baseWrongNumPos;

            
        }
        
    }

    public void PreviousPage() //controls the previous button on the record page
    {
        if (currentPage > 0)
        {
            currentPage--;
            UpdateRecordPage();
            UpdateSceneResults();
        }
    }

    public void NextPage() //controls the next button on the record page
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
