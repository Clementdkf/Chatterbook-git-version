using UnityEngine;
using UnityEngine.UI;

public class ButtonFix : MonoBehaviour
{
    public Button resetButton;

    void Start()
    {
        if (resetButton != null)
        {
            resetButton.onClick.RemoveAllListeners(); // optional cleanup
            resetButton.onClick.AddListener(() =>
            {
                ReceivingRecords.Instance.ResetAllQuizData();
            });
        }
    }
}