using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;
// The [CreateAssetMenu] attribute allows you to create instances of this ScriptableObject
// directly from the Unity Editor's asset menu.
[CreateAssetMenu(fileName = "New Scene Quiz Data", menuName = "Quiz/Scene Quiz Data")]
public class SceneQuizData : ScriptableObject
{
    public int correctCount = 0;
    public int wrongCount = 0;
    public string sceneName;
    public int sceneID;
    public bool isEnabled = true;
    public LocalizedString LocalizedSceneName;
    public LocalizedString localizedCorrectLabel;
    public LocalizedString localizedWrongLabel;
}