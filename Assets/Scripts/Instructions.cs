using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Instructions : MonoBehaviour
{
    public GameObject menuPanel;
    public GameObject bookPanel;
    public GameObject minigamePanel;
    public GameObject[] minigameText;
    public GameObject[] Buttons;
    private int currentIndex = 0;
    // Start is called before the first frame update
    public void MenuPanel()
    {
        menuPanel.SetActive(true);
        bookPanel.SetActive(false);
        minigamePanel.SetActive(false);
    }

    public void BookPanel()
    {
        menuPanel.SetActive(false);
        bookPanel.SetActive(true);
        minigamePanel.SetActive(false);
    }

    public void MinigamePanel()
    {
        menuPanel.SetActive(false);
        bookPanel.SetActive(false);
        minigamePanel.SetActive(true);
        for (int i = 0; i < minigameText.Length; i ++)
        {
            minigameText[i].SetActive(i == 0);
        }
    }

    public void nextText()
    {
        // Deactivate current text
        minigameText[currentIndex].SetActive(false);

        // Move to next index (wrap around if at the end)
        currentIndex = (currentIndex + 1) % minigameText.Length;

        // Activate next text
        minigameText[currentIndex].SetActive(true);
    }

    public void ExitButton(GameObject panel) 
    {
        panel.SetActive(false); 
        foreach (GameObject obj in Buttons)
        {
            if (obj != null)
            {
                obj.SetActive(false);
            }
        }
    } 

    public void HelpButton()
    {
        menuPanel.SetActive(true);
                foreach (GameObject obj in Buttons)
        {
            if (obj != null)
            {
                obj.SetActive(true);
            }
        }
    }
}
