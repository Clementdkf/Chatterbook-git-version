using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Instructions : MonoBehaviour
{
    public PanelManager panelManager;
    public GameObject menuPanel;
    public GameObject bookPanel;
    public GameObject minigamePanel;
    public GameObject[] minigameText;
    public GameObject[] Buttons;
    private int currentIndex = 0;
    // Start is called before the first frame update
    public void MenuPanel()
    {
        panelManager.OpenPanel(menuPanel);
    }

    public void BookPanel()
    {
        panelManager.OpenPanel(bookPanel);
    }

    public void MinigamePanel()
    {
        panelManager.OpenPanel(minigamePanel);
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
        panelManager.OpenPanel(menuPanel);
        foreach (GameObject obj in Buttons)
        {
            if (obj != null)
            {
                obj.SetActive(true);
            }
        }
    }
}
