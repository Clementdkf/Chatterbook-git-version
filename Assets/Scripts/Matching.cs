using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Matching : MonoBehaviour
{
    // Start is called before the first frame update
    public List<Item> MatchingPairs = new List<Item> ();
    private Button selectedButton = null;
    private string selectedKey = null;
    void OnButtonClicked(string key, Button clickedButton)
    {
        if (selectedKey == null)
        {
            selectedKey = key;
            selectedButton = clickedButton;
            Debug.Log("Selected " + key);
        }
    }
}

[System.Serializable]

public class Item
{
    public Button button;
    public Button MatchingButton;
    public String matchKey;

}
