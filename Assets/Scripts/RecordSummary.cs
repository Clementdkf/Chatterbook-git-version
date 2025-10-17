using UnityEngine;
using TMPro;

public class RecordSummary : MonoBehaviour
{
    public TextMeshProUGUI scene1ResultsText;
    public TextMeshProUGUI scene2ResultsText;   
    public TextMeshProUGUI scene3ResultsText;

void Start()
{
    // Ensure there are enough data entries before accessing them by index
    var allSceneQuizData = ReceivingRecords.Instance.allSceneQuizData;

    // Display results for the first scene
    if (allSceneQuizData.Count > 0)
    {
        var sceneData = allSceneQuizData[0];
        scene1ResultsText.text = $"正確: {sceneData.correctCount}             錯誤: {sceneData.wrongCount}";
    }
    else
    {
        scene1ResultsText.text = "第一關: 沒有資料";
    }

    // Display results for the second scene
    if (allSceneQuizData.Count > 1)
    {
        var sceneData = allSceneQuizData[1];
        scene2ResultsText.text = $"正確: {sceneData.correctCount}             錯誤: {sceneData.wrongCount}";
    }
    else
    {
        scene2ResultsText.text = "沒有資料";
    }
    
    // Display results for the third scene
    if (allSceneQuizData.Count > 2)
    {
        var sceneData = allSceneQuizData[2];
        scene3ResultsText.text = $"正確: {sceneData.correctCount}             錯誤: {sceneData.wrongCount}";
    }
    else
    {
        scene3ResultsText.text = "沒有資料";
    }
}

}
