using UnityEngine;

// The [CreateAssetMenu] attribute allows you to create instances of this ScriptableObject
// directly from the Unity Editor's asset menu.
[CreateAssetMenu(fileName = "New Scene Quiz Data", menuName = "Quiz/Scene Quiz Data")]
public class SceneQuizData : ScriptableObject
{
    public int correctCount = 0;
    public int wrongCount = 0;
    public string sceneName;
}