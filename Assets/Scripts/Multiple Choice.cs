using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class MultipleChoice : MonoBehaviour
{
    // Start is called before the first frame update
    public Button CorrectAnswer;
    public Button[] WrongAnswers;
    public TMPro.TextMeshProUGUI CorrectText;
    public TMPro.TextMeshProUGUI WrongText;
    public TMPro.TextMeshProUGUI QuestionText;
    void Start()
    {
        CorrectText.gameObject.SetActive(false);
        WrongText.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Correct()
    {
        Debug.Log("Correct");
        CorrectText.gameObject.SetActive(true);  // Show the "Correct!" text
        QuestionText.gameObject.SetActive(false); // Hide the question text
        WrongText.gameObject.SetActive(false); // Hide the "Wrong!" text if it was shown
        CorrectAnswer.GetComponent<Image>().color = Color.green;
        foreach (Button wrong in WrongAnswers) // Disable wrong answers
        {
            wrong.gameObject.SetActive(false);
        }
        CorrectAnswer.interactable = false; // Disable the correct answer button
    }

    public void Wrong(Button pressedButton)
    {
        Debug.Log("Wrong");
        WrongText.gameObject.SetActive(true); // Show the "Wrong!" text
        //pressedButton.interactable = false; // Disable only the pressed wrong answer
        //pressedButton.GetComponent<Image>().color = Color.red; // Optional: give visual feedback
        pressedButton.gameObject.SetActive(false); // Hide the pressed wrong answer
    }
}
