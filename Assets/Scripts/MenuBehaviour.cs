using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MenuBehaviour : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject settingsPanel;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ToggleSettings(GameObject settingsPanel)
    {
        settingsPanel.SetActive(!settingsPanel.activeSelf);
    }

    public void ChangeScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void ExitApplication()
    {
        Application.Quit();
    }
    
    public void CloseUI() {
        settingsPanel.SetActive(false);
    }
    
}
