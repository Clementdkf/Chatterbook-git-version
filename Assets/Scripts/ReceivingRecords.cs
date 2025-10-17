using System.Collections.Generic;
using UnityEngine;

public class ReceivingRecords : MonoBehaviour
{
    public static ReceivingRecords Instance;

    public List<SceneQuizData> allSceneQuizData = new List<SceneQuizData>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public SceneQuizData GetQuizDataForScene(string sceneName)
    {
        foreach (var data in allSceneQuizData)
        {
            if (data.sceneName == sceneName)
            {
                return data;
            }
        }
        return null; // Return null if data for the scene isn't found
    }

    public void ResetAllQuizData()
    {
        foreach (var data in allSceneQuizData)
        {
            data.correctCount = 0;
            data.wrongCount = 0;
        }
        Debug.Log("All quiz data has been reset.");
    }
}
