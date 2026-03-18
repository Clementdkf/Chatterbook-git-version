using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System;
//using UnityEngine.UIElements;
using UnityEngine.Localization.SmartFormat.Utilities;

public class PageScroller : MonoBehaviour
{
    public string targetTag = "pages";

    [Header("Pages")]
    public GameObject[] pages;

    [Header("Buttons")]
    public GameObject NextButton;
    public GameObject homeButton;
    public GameObject bookMarkButton;
    private int currentPage = 0;
    private bool isPressed;

    [Header("Audio")]
    public AudioClip pagescrollingClip;
    private AudioSource audioSource;

    [Header("Progress bar")]
    public Slider progressBar;

    void Start()
    {
        pages = FindGameObjectsWithTagInHierarchy(targetTag).ToArray();
        foreach (GameObject obj in pages)
        {
            obj.SetActive(false);
        }
        pages[0].SetActive(true);

        string bookmarkKey = SceneManager.GetActiveScene().name + "_BookmarkedPage";

        if (gameObject.tag != "UIPanels")
        {
            if (PlayerPrefs.HasKey(bookmarkKey))
            {
                currentPage = PlayerPrefs.GetInt(bookmarkKey);
                isPressed = true;
                bookMarkButton.GetComponent<Image>().color = new Color(100f/255f, 230f/255f, 255f/255f);
            }
            else
            {
                currentPage = 0;
                isPressed = false;
                bookMarkButton.GetComponent<Image>().color = Color.white;
            }
            ShowPage(currentPage);
        }
        
        audioSource = GetComponent<AudioSource>();

        if (progressBar != null)
        {
            progressBar.minValue = 0;
            progressBar.maxValue = pages.Length - 1;
            progressBar.value = currentPage;
            progressBar.onValueChanged.AddListener(OnProgressBarChanged);
        }

    }

    void Update()
    {
        if (currentPage == pages.Length - 1)
        {
            NextButton.SetActive(false);
            homeButton.SetActive(true);
            Debug.Log("Last Page");

        } else {
            NextButton.SetActive(true);
            homeButton.SetActive(false);
        }
    }

    //move to the next page of the book
    public void NextPage()
    {
        if (currentPage < pages.Length - 1)
        {
            currentPage++;
            ShowPage(currentPage);
        }
    }

    //move to the previous page of the book
    public void PreviousPage()
    {
        if (currentPage > 0)
        {
            currentPage--;
            ShowPage(currentPage);
        }
    }

    void ShowPage(int index)
    {
        for (int i = 0; i < pages.Length; i++)
        {
            pages[i].SetActive(i == index);
        }

        if (progressBar != null)
        {
            progressBar.value = index;
        }
    }

    //controlling the behaviour of the progress bar
    void OnProgressBarChanged(float value)
    {
        int newPage = Mathf.RoundToInt(value);
        if (newPage != currentPage)
        {
            currentPage = newPage;
            ShowPage(currentPage);
        }
    }
    
    //controlling the page scrolling sound
    public void PlayPageScrollSound()
    {
        if (audioSource != null && pagescrollingClip != null)
        {
            audioSource.PlayOneShot(pagescrollingClip);
        }
    }

    //allowing users to book mark a certain page
    public void BookMark()
    {
        Image buttonImage = bookMarkButton.GetComponent<Image>();
        isPressed = !isPressed;

        // Create a unique key based on the scene name
        string bookmarkKey = SceneManager.GetActiveScene().name + "_BookmarkedPage";

        if (isPressed)
        {
            PlayerPrefs.SetInt(bookmarkKey, currentPage);
            PlayerPrefs.Save();
            Debug.Log("Bookmarked Page in " + SceneManager.GetActiveScene().name + ": " + currentPage);

            buttonImage.color = new Color(100f/255f, 230f/255f, 255f/255f); // light blue
        }
        else
        {
            PlayerPrefs.DeleteKey(bookmarkKey);
            PlayerPrefs.Save();
            Debug.Log("Bookmark cleared for " + SceneManager.GetActiveScene().name);

            buttonImage.color = Color.white;
        }
    }

    //Finding the game objects in the hierachy with the required tag
    public static List<GameObject> FindGameObjectsWithTagInHierarchy(string tag)
    {

        List<GameObject> foundObjects = new List<GameObject>();
        Scene activeScene = SceneManager.GetActiveScene();

        GameObject[] rootObjects = activeScene.GetRootGameObjects();
        Debug.Log(rootObjects);
        foreach (GameObject rootObj in rootObjects)
        {
            TraverseHierarchyAndAddTagged(rootObj.transform, tag, foundObjects);
        }
        return foundObjects;
    }

    //adding the gameobjects to the list according to the hierarchy
    private static void TraverseHierarchyAndAddTagged(Transform parent, string tag, List<GameObject> list)
    {
        if (parent.CompareTag(tag))
        {
            list.Add(parent.gameObject);
        }

        for (int i = 0; i < parent.childCount; i++)
        {
            TraverseHierarchyAndAddTagged(parent.GetChild(i), tag, list);
        }
    }
}