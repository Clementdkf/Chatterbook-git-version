using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class MenuBehaviour : MonoBehaviour
{
    // Start is called before the first frame update
    [Header("Panel Manager")]
    public PanelManager panelManager;
    
    [Header("Panels")]
    public GameObject[] helpPanels;
    public GameObject settingsPanel;

    [Header("Buttons")]
    public Button settingsUIButton;
    public Button helpUIButton;
    public void Toggle() //for settings button
    {
        panelManager.OpenPanel(settingsPanel);
        settingsUIButton.gameObject.SetActive(true);
        helpUIButton.gameObject.SetActive(true);   
    }

    public void ChangeScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void ExitApplication()
    {
        Application.Quit();
    }
    public void OpenUI(GameObject panel)
    {
        panelManager.OpenPanel(panel);
    }


    public void ExitButton(GameObject panel) 
    {
        panel.SetActive(false);
        settingsUIButton.gameObject.SetActive(false); 
        helpUIButton.gameObject.SetActive(false);   
    }

    public void SettingsUI()
    {
        panelManager.OpenPanel(settingsPanel);
        foreach (GameObject panel in helpPanels)
        {
            if (panel != null)
                panel.SetActive(false);
        }
    }

    public void OpenhelpUI()
    {
        settingsPanel.SetActive(false);
        panelManager.OpenPanel(helpPanels[0]);
        helpPanels[1].SetActive(false);
    }

    public void HelpNextUI()
    {
        for (int i = 0; i < helpPanels.Length; i++)
        {
            if (helpPanels[i].activeSelf && i < helpPanels.Length - 1)
            {
                helpPanels[i].SetActive(false);
                panelManager.OpenPanel(helpPanels[i + 1]);
                break;
            }
        }
    }
    public void HelpPreviousUI()
    {
        for (int i = 0; i < helpPanels.Length; i++)
        {
            if (helpPanels[i].activeSelf && i > 0)
            {
                helpPanels[i].SetActive(false);
                panelManager.OpenPanel(helpPanels[i - 1]);
                break;
            }
        }
    }

    public void DisableButton() 
    {
        settingsUIButton.gameObject.SetActive(false); 
        helpUIButton.gameObject.SetActive(false);   
    }

}
