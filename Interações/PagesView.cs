using UnityEngine;

public class PagesView : MonoBehaviour
{
    [SerializeField] GameObject[] pages;
    [SerializeField] int currentPage;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentPage = 0;
        pages[currentPage].SetActive(true);
    }

    public void NextPage()
    {
        pages[currentPage].SetActive(false);
        currentPage++;
        pages[currentPage].SetActive(true);
    }

    public void PreviousPage()
    {
        pages[currentPage].SetActive(false);
        currentPage++;
        pages[currentPage].SetActive(true);
    }
}
