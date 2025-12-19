using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class PageScroller : MonoBehaviour
{
    public string targetTag = "pages";

    [Header("Pages")]
    public GameObject[] pages;

    [Header("Navigation Buttons")]
    public GameObject NextButton;
    public GameObject homeButton;
    private int currentPage = 0;

    [Header("Audio")]
    public AudioClip pagescrollingClip;
    private AudioSource audioSource;

    void Start()
    {
        pages = FindGameObjectsWithTagInHierarchy(targetTag).ToArray();
        foreach (GameObject obj in pages)
        {
            obj.SetActive(false);
        }
        pages[0].SetActive(true);

        if (gameObject.tag != "UIPanels")
        {
            ShowPage(currentPage);
        }
        //ShowPage(currentPage);
        audioSource = GetComponent<AudioSource>();
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

    public void NextPage()
    {
        if (currentPage < pages.Length - 1)
        {
            currentPage++;
            ShowPage(currentPage);
        }
    }

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
    }
    
    public void PlayPageScrollSound()
    {
        if (audioSource != null && pagescrollingClip != null)
        {
            audioSource.PlayOneShot(pagescrollingClip);
        }
    }

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