using UnityEngine;
using UnityEngine.SceneManagement;

public class PageScroller : MonoBehaviour
{
    public GameObject[] pages;
    public GameObject HomeButton;
    public GameObject NextButton;
    private int currentPage = 0;

    void Start()
    {
        ShowPage(currentPage);
        HomeButton.SetActive(false);
    }

    void Update()
    {
        if (currentPage == pages.Length - 1)
        {
            HomeButton.SetActive(true);
            NextButton.SetActive(false);
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
}