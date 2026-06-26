using System.Collections;
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using StrapiForUnity;
using Proyecto26;

public class MenuBehaviour : MonoBehaviour
{
    // Start is called before the first frame update
    [Header("Panel Manager")]
    public PanelManager panelManager;
    
    [Header("Panels")]
    public GameObject[] helpPanels;
    public GameObject settingsPanel;

    [Header("Help Buttons")]
    public GameObject[] helpButtons;
    [SerializeField] private SceneQuizData currentSceneData;

    private StrapiComponent strapiComponent;

    private void Awake()
    {
        strapiComponent = StrapiComponent.Instance;
    }

    //Logging out from the log out button
    public void Logout()
    {
        if (strapiComponent != null)
        {
            // Clear authentication state
            strapiComponent.AuthenticatedUser = null;
            strapiComponent.IsAuthenticated = false;

            // Remove JWT from PlayerPrefs
            if (PlayerPrefs.HasKey("jwt"))
            {
                PlayerPrefs.DeleteKey("jwt");
                PlayerPrefs.Save();
            }

            // Clear Authorization header
            if (RestClient.DefaultRequestHeaders.ContainsKey("Authorization"))
            {
                RestClient.DefaultRequestHeaders.Remove("Authorization");
            }

            Debug.Log("User logged out successfully.");
            SceneManager.LoadScene("Login and Register Page");
        }
        else
        {
            Debug.LogWarning("StrapiComponent instance not found. Cannot log out.");
        }
    }
    public void Toggle() //toggling settings panel 
    {
        panelManager.OpenPanel(settingsPanel);
        foreach (GameObject obj in helpButtons)
        {
            if (obj != null)
            {
                obj.SetActive(false);
            }
        }
    }

    //change scenes 
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
        foreach (GameObject obj in helpButtons)
        {
            if (obj != null)
            {
                obj.SetActive(false);
            }
        }
    }


    public void ExitButton(GameObject panel) 
    {
        panel.SetActive(false); 
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

    //opening the help panel
    public void OpenhelpUI()
    {
        settingsPanel.SetActive(false);
        panelManager.OpenPanel(helpPanels[0]);
        helpPanels[1].SetActive(false);
    }

    //opening the next page of the help panel
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

    //opening the previous page of help panel
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

    public void SaveButton()
    {
        //var username = AuthManager.Instance.CurrentUser.user.username;
        DateTime currentTime = DateTime.Now;
        Debug.Log("Current time: "+ currentTime.ToString() + " Scene Name: " + currentSceneData.sceneName + " Wrong count: " + currentSceneData.wrongCount.ToString() + " Correct count: " + currentSceneData.correctCount.ToString());

    }

}
