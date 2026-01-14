using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InfoPanel : MonoBehaviour
{
    // Start is called before the first frame update
    [Header("Pages")]
    public GameObject[] pages;

    [Header("Buttons")]
    public GameObject nextButton;
    public GameObject previousButton;
    private int currentPage = 0;
    // Update is called once per frame
    void Update()
    {
        bool anyPageActive = false;
        foreach (var obj in pages)
        {
            if (obj.activeSelf == true)
            {
                anyPageActive = true;
                if (currentPage == pages.Length - 1)
                {
                    previousButton.SetActive(true);
                    nextButton.SetActive(false);
                    //Debug.Log("Last Page");
                }  
                else if (currentPage == 0) 
                {
                    nextButton.SetActive(true);
                    previousButton.SetActive(false);
                    //Debug.Log("current page count is 0");
                }  
                else 
                {
                    nextButton.SetActive(true);
                    previousButton.SetActive(true);
                }
            } 
            if (anyPageActive == false)
            {
                nextButton.SetActive(false);
                previousButton.SetActive(false);                
            }
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
}
