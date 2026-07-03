using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class ReceivingRecords : MonoBehaviour
{
    //public List<SceneQuizData> allScenes;
    public static ReceivingRecords Instance;
    public List<SceneQuizData> allSceneQuizData = new List<SceneQuizData>();
    public delegate void QuizDataRestHandler();
    public static event QuizDataRestHandler OnQuizDataReset;
    private HashSet<string> excludedSceneNames = new HashSet<string> {"Login and Register Page", "配對小遊戲", "Drawing"};
    private HashSet<int> excludedSceneIDs = new HashSet<int> {9};

    void Start()
    {
        Dictionary<string, int> filteredScenes = GetSceneNameandID(allSceneQuizData);
        foreach (var kvp in filteredScenes)
        {
            Debug.Log($"Scene: {kvp.Key}, ID: {kvp.Value}");
        }
    }
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

    public SceneQuizData GetQuizDataForScene(string sceneName) //Getting the corresponding scene's quiz data
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

    public void ResetAllQuizData() //used for the reset button on the record panel
    {
        Debug.Log("ResetAllQuizData called. Count: " + allSceneQuizData.Count);
        foreach (var data in allSceneQuizData)
        {
            data.correctCount = 0;
            data.wrongCount = 0;
        }
        OnQuizDataReset?.Invoke();
        Debug.Log("All quiz data has been reset.");
    }

    private Dictionary<string, int> GetSceneNameandID(List<SceneQuizData> allSceneQuizData) //getting the scene names and IDs except certain scenes
    {
        return allSceneQuizData
            .Where(scene => scene.isEnabled)
            .Where(scene => !excludedSceneNames.Contains(scene.sceneName))
            .Where(scene => !excludedSceneIDs.Contains(scene.sceneID))
            .ToDictionary(scene => scene.sceneName, scene => scene.sceneID);

    }
}
