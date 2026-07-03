using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System;
//using UnityEngine.UIElements;
using UnityEngine.Localization.SmartFormat.Utilities;
using System.Collections;
using TMPro;

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
        pages = FindGameObjectsWithTagInHierarchy(targetTag).ToArray(); //finding all pages with the target tag
        foreach (GameObject obj in pages) //deactivating all pages and show the current page
        {
            obj.SetActive(false);
        }
        ShowPage(currentPage);

        //StartCoroutine(RefreshTextScaleNextFrame());

        string bookmarkKey = SceneManager.GetActiveScene().name + "_BookmarkedPage";

        if (gameObject.tag != "UIPanels")
        {
            if (PlayerPrefs.HasKey(bookmarkKey)) //see if there is a bookmark key in playprefs, if yes then load that page
            {
                currentPage = PlayerPrefs.GetInt(bookmarkKey);
                isPressed = true;
                bookMarkButton.GetComponent<Image>().color = new Color(100f/255f, 230f/255f, 255f/255f);
                ShowPage(currentPage);
            }
            else //if not, then load the first page
            {
                currentPage = 0;
                isPressed = false;
                bookMarkButton.GetComponent<Image>().color = Color.white;
            }
        }
        
        audioSource = GetComponent<AudioSource>();

        if (progressBar != null) //controls the behaviour of the progress bar
        {
            progressBar.minValue = 0;
            progressBar.maxValue = pages.Length - 1;
            progressBar.value = currentPage;
            progressBar.onValueChanged.AddListener(OnProgressBarChanged);
        }

    }
    void Update()
    {
        if (currentPage == pages.Length - 1) //controlling the next and previous buttons' behaviour
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
    public void NextPage() //function for next button
    {
        if (currentPage < pages.Length - 1)
        {
            currentPage++;
            ShowPage(currentPage);
        }
    }

    //move to the previous page of the book
    public void PreviousPage() //function for previous button
    {
        if (currentPage > 0)
        {
            currentPage--;
            ShowPage(currentPage);
        }
    }

    void ShowPage(int index) //showing the page with the corresponding interger
    {
        for (int i = 0; i < pages.Length; i++)
            pages[i].SetActive(false);

        if (progressBar != null)
            progressBar.value = index;

        pages[index].SetActive(true);

        // Hide texts temporarily
        var texts = pages[index].GetComponentsInChildren<TextMeshProUGUI>(true);
        if (texts.Length > 0)
        {
            foreach (var t in texts) t.enabled = false;
            StartCoroutine(RefreshTextScaleNextFrame(texts));
        }

    }

    private IEnumerator RefreshTextScaleNextFrame(TMPro.TextMeshProUGUI[] texts) //wait one frame to ensure the UI layout is updated
    {
        yield return null; // wait one frame
        Canvas.ForceUpdateCanvases();

        var sliderControl = FindObjectOfType<SliderControl>();
        if (sliderControl != null)
        {
            float savedScale = SettingsManager.Instance?.CurrentTextScale
                               ?? PlayerPrefs.GetFloat("textScale", 1f);
            sliderControl.UpdateAllFontSizes(savedScale);
        }

        // Show texts only after scaling
        foreach (var t in texts) t.enabled = true;
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
        isPressed = !isPressed; //toggle the bookmark bool

        // Create a unique key based on the scene name
        string bookmarkKey = SceneManager.GetActiveScene().name + "_BookmarkedPage";

        if (isPressed) //saves the current page index to PlayerPrefs if true
        {
            PlayerPrefs.SetInt(bookmarkKey, currentPage);
            PlayerPrefs.Save();
            Debug.Log("Bookmarked Page in " + SceneManager.GetActiveScene().name + ": " + currentPage);

            buttonImage.color = new Color(100f/255f, 230f/255f, 255f/255f); // light blue
        }
        else //deletes bookmark key from PlayerPrefs and resets button color
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