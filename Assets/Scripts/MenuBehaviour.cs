using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class MenuBehaviour : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject[] helpPanels;
    public GameObject settingsPanel;
    public Button settingsButton;
    public Button helpButton;
    void Start()
    {
        //settingsPanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Toggle() //for settings button
    {
        settingsPanel.SetActive(true);
        settingsButton.gameObject.SetActive(true);
        helpButton.gameObject.SetActive(true);   
        foreach (GameObject panel in helpPanels)
        {
            if (panel != null)
                panel.SetActive(false);
        }    
    }

    public void ChangeScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void ExitApplication()
    {
        Application.Quit();
    }

    public void ExitButton(GameObject panel)
    {
        panel.SetActive(false);
        settingsButton.gameObject.SetActive(false);
        helpButton.gameObject.SetActive(false);
    }
    
    public void OpenUI(GameObject panel)
    {
        panel.SetActive(true);
    }

    public void SettingsUI()
    {
        settingsPanel.SetActive(true);
        foreach (GameObject panel in helpPanels)
        {
            if (panel != null)
                panel.SetActive(false);
        }
    }

    public void OpenhelpUI()
    {
        settingsPanel.SetActive(false);
        helpPanels[0].SetActive(true);
        helpPanels[1].SetActive(false);
    }

    public void HelpNextUI()
    {
        for (int i = 0; i < helpPanels.Length; i++)
        {
            if (helpPanels[i].activeSelf && i < helpPanels.Length - 1)
            {
                helpPanels[i].SetActive(false);
                helpPanels[i + 1].SetActive(true);
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
                helpPanels[i - 1].SetActive(true);
                break;
            }
        }
    }

}
