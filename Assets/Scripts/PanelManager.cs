using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelManager : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject[] panels;
    private GameObject currentPanel;

    public void OpenPanel(GameObject panelToOpen)
    {
        if (currentPanel != null)
        {
            currentPanel.SetActive(false); // Deactivate the current panel
        }
        panelToOpen.SetActive(true); // Activate the new panel
        currentPanel = panelToOpen; // Update the current panel reference
    }   
}
