using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Net;
using UnityEngine.Rendering;

public class Matching : MonoBehaviour
{
    // Start is called before the first frame update
    public List<Item> MatchingPairs = new List<Item> ();    
    private Button selectedButton = null; //first button pressed in the first attempt
    private string selectedKey = null;
    private SceneQuizData currentSceneData;
    private AudioSource audioSource;
    public AudioClip correctClip;
    public AudioClip wrongClip;

    void Start()
    {
                // Get the current scene's name
        string currentSceneName = SceneManager.GetActiveScene().name;

        audioSource = GetComponent<AudioSource>();

        // Get the specific SceneQuizData from the GameStatisticsManager
        currentSceneData = ReceivingRecords.Instance.GetQuizDataForScene(currentSceneName); 
        if (currentSceneData == null)
        {
            Debug.LogError("Quiz data for scene '" + currentSceneName + "' not found!");
            return;
        }
        foreach (var pair in MatchingPairs)
        {
            pair.button.onClick.AddListener(() => OnButtonClicked(pair.matchKey, pair.button));
            pair.MatchingButton.onClick.AddListener(() => OnButtonClicked(pair.matchKey, pair.MatchingButton));
        }
    }
    void OnButtonClicked(string key, Button clickedButton) //clickedButton is the button that the player just pressed
    {
        ResetOutlines();
        if (selectedKey == null)
        {
            selectedKey = key;
            selectedButton = clickedButton;
            selectedButton.GetComponent<Outline>().effectColor = Color.yellow;
            Debug.Log("Selected " + key);
        } 
        else
        {
            if (selectedKey == key && selectedButton != clickedButton)
            {
                Debug.Log("Correct guess!");
                selectedButton.GetComponent<Outline>().effectColor = Color.green;
                clickedButton.GetComponent<Outline>().effectColor = Color.green; 
                DisablePair(key);
                selectedKey = null;
                selectedButton = null;
                audioSource.PlayOneShot(correctClip);
                currentSceneData.correctCount++;
            } else if (selectedButton == clickedButton)
            {
            
            }
            else 
            {
                Debug.Log("Wrong guess!");
                selectedButton.GetComponent<Outline>().effectColor = Color.red;
                clickedButton.GetComponent<Outline>().effectColor = Color.red;
                selectedKey = null;
                selectedButton = null;
                audioSource.PlayOneShot(wrongClip);
                currentSceneData.wrongCount++;
            }
        }
    }

    void DisablePair(string key)
    {
        var pair = MatchingPairs.Find(p => p.matchKey == key); 
        if (pair != null)
        {
            pair.button.interactable = false;
            pair.MatchingButton.interactable = false;
        }
    }

    void ResetOutlines()
    {
        foreach (var pair in MatchingPairs)
        {
            if (pair.button != null)
            {
                var outline = pair.button.GetComponent<Outline>();
                if (outline != null && outline.effectColor != Color.green)
                {
                    outline.effectColor = Color.white;
                }
            }

            if (pair.MatchingButton != null)
            {
                var outline = pair.MatchingButton.GetComponent<Outline>();
                if (outline != null && outline.effectColor != Color.green)
                {
                    outline.effectColor = Color.white;
                }
            }
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