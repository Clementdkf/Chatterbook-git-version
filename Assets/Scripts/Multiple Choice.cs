using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.SceneManagement;

public class MultipleChoice : MonoBehaviour
{
    public Button CorrectAnswer;
    public Button[] WrongAnswers;
    public TextMeshProUGUI CorrectText;
    public TextMeshProUGUI WrongText;
    public TextMeshProUGUI QuestionText;

    [Header("Audio")]
    public AudioClip correctClip;
    private AudioSource audioSource;
    public AudioClip WrongClip;

    [Header("Record")]
    public TextMeshProUGUI CorrectCountText;
    public TextMeshProUGUI WrongCountText;

    // Reference to the ScriptableObject for this specific scene's data
    private SceneQuizData currentSceneData;

    void Start()
    {
        // Get the current scene's name
        string currentSceneName = SceneManager.GetActiveScene().name;

        // Get the specific SceneQuizData from the GameStatisticsManager
        currentSceneData = ReceivingRecords.Instance.GetQuizDataForScene(currentSceneName);
        if (currentSceneData == null)
        {
            Debug.LogError("Quiz data for scene '" + currentSceneName + "' not found!");
            return;
        }

        CorrectText.gameObject.SetActive(false);
        WrongText.gameObject.SetActive(false);
        audioSource = GetComponent<AudioSource>();

        // Update UI with current counts
        CorrectCountText.text = currentSceneData.correctCount.ToString();
        WrongCountText.text = currentSceneData.wrongCount.ToString();
    }

    public void Correct()
    {
        if (currentSceneData == null) return;
        
        Debug.Log("Correct");
        CorrectText.gameObject.SetActive(true);
        QuestionText.gameObject.SetActive(false);
        WrongText.gameObject.SetActive(false);
        CorrectAnswer.GetComponent<Image>().color = Color.green;
        foreach (Button wrong in WrongAnswers)
        {
            wrong.gameObject.SetActive(false);
        }
        CorrectAnswer.interactable = false;
        audioSource.PlayOneShot(correctClip);

        // Update the ScriptableObject data, not static variables
        currentSceneData.correctCount++;
        CorrectCountText.text = currentSceneData.correctCount.ToString();
    }

    public void Wrong(Button pressedButton)
    {
        if (currentSceneData == null) return;

        Debug.Log("Wrong");
        WrongText.gameObject.SetActive(true);
        pressedButton.gameObject.SetActive(false);
        audioSource.PlayOneShot(WrongClip);

        // Update the ScriptableObject data
        currentSceneData.wrongCount++;
        WrongCountText.text = currentSceneData.wrongCount.ToString();
    }
}
