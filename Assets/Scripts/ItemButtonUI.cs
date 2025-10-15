using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class ItemButtonUI : MonoBehaviour
{
    public TextMeshProUGUI titleText;
    public Image itemImage;
    private string sceneToLoad;
    
    public void Setup(ContentItem item)
    {
        titleText.text = item.title;
        itemImage.GetComponent<Image>().sprite = item.image;
        sceneToLoad = item.sceneName;

        GetComponent<Button>().onClick.RemoveAllListeners();
        GetComponent<Button>().onClick.AddListener(() => SceneManager.LoadScene(sceneToLoad));
    }
}