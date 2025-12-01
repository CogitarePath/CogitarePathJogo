using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameMenu : MonoBehaviour
{
    [SerializeField] private GameObject Menu;

    bool MenuOpen;

    GameObject player;

    public static GameObject[] collectedPages = new GameObject[5];

    GameObject shownPage;

    Button[] buttons;

    private void Awake()
    {
        player = GameObject.FindWithTag("Player");
        GetComponent<PauseController>().enabled = true;
        buttons = GetComponentsInChildren<Button>();
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            Menu.SetActive(true);
            PauseController.SetPause(true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            player.SetActive(false);
            MenuOpen = true;
        }

        if (MenuOpen && Input.GetKeyDown(KeyCode.E) && shownPage != null)
        {
            Destroy(shownPage);
        }
    }

    public void BackToMain()
    {
        SceneManager.LoadScene("MenuInicial");
        PauseController.SetPause(false);
    }

    public void ShowPage(int page)
    {
        shownPage = Instantiate(collectedPages[page], Menu.transform);
        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].enabled = false;
        }
    }

    public void CloseMenu()
    {
        Menu.SetActive(false);
        PauseController.SetPause(false );
        player.SetActive(true);
        MenuOpen = false;
    }
}
