using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class ItemButtonUI : MonoBehaviour
{
    public TextMeshProUGUI titleText;
    private string sceneToLoad;
    
    public void Setup(ContentItem item)
    {
        titleText.text = item.title;
        sceneToLoad = item.sceneName;

        GetComponent<Button>().onClick.RemoveAllListeners();
        GetComponent<Button>().onClick.AddListener(() => SceneManager.LoadScene(sceneToLoad));
    }
}