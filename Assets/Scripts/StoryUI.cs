using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StoryUI : MonoBehaviour
{
    public TMP_Text titleText;
    public TMP_Text dateText;
    public TMP_Text popularityText;

    public void Setup(ContentItem item)
    {
        titleText.text = item.title;
        //dateText.text = item.createdAt.ToShortDateString();
        popularityText.text = $"🔥 {item.popularityScore}";
    }
}