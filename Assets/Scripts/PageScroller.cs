using UnityEngine;
using UnityEngine.SceneManagement;

public class PageScroller : MonoBehaviour
{
    [Header("Pages")]
    public GameObject[] pages;

    [Header("Navigation Buttons")]
    public GameObject NextButton;
    private int currentPage = 0;

    [Header("Audio")]
    public AudioClip pagescrollingClip;
    private AudioSource audioSource;

    void Start()
    {
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
        } else {
            NextButton.SetActive(true);
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

    public void ChangeScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
    
    public void PlayPageScrollSound()
    {
        if (audioSource != null && pagescrollingClip != null)
        {
            audioSource.PlayOneShot(pagescrollingClip);
        }
    }
}