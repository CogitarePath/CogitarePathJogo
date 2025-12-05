using System.Collections;
using System.Security.Cryptography;
using TMPro;
using UnityEngine;

public class DoorPassword : MonoBehaviour
{

    [SerializeField] private GameObject display;
    [SerializeField] TextMeshProUGUI[] displayPassword;
    protected static int nextDigit = 0;
     private string[] password = new string[] { string.Empty, string.Empty, "", "" };
    [SerializeField] private string[] correctPassword = new string[] { "1", "3", "4", "9" };

    public ShowKeypad creatorScript;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
        PauseController.SetPause(true);

        nextDigit = 0;
        displayPassword[0].text = "_";
        creatorScript.objectVisible = true;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && creatorScript.CanClose)
        {
            Debug.Log(creatorScript);
            CloseDisplay();            
        }
    }



    public void ConfirmPassword()
    {
        for (int i = 0; i < password.Length; i++)
        {
            if (displayPassword[i].text != correctPassword[i])
            {
                StartCoroutine(ErrorMessage()); 
                nextDigit = 0;
                password[nextDigit] = "_";
                return;

            }
        }
        OpenDoor();
        creatorScript.WasUsed = true;        
        StartCoroutine(CorrectPassword());

    }

    public void DeletePassword()
    {
        password = new string[] { string.Empty, string.Empty, string.Empty, string.Empty };
        for (int i = 0; i < displayPassword.Length; i++)
        {
            displayPassword[i].text = string.Join("", password);
        }
        nextDigit = 0;
        displayPassword[0].text = "_";
    }

    void OpenDoor()
    {
        for (int i = 0; i < password.Length; i++)
        {
            displayPassword[i].color = Color.green;
        }       
    }
    public void ClickNumber(string KeyNumber)
    {

        password[nextDigit] = KeyNumber;
        Debug.Log(nextDigit);
        displayPassword[nextDigit].text = KeyNumber;
        for (int i = 0; i < password.Length; i++)
        {
            password[i] = displayPassword[i].text;
        }
        if (nextDigit < 3)
        {
            nextDigit += 1;

            displayPassword[nextDigit].text = "_";
        }

    }
    IEnumerator ErrorMessage()
    {
        displayPassword[0].text = "E ";
        displayPassword[1].text = "R ";
        displayPassword[2].text = "R ";
        displayPassword[3].text = "O";
        yield return new WaitForSeconds(2f);
        displayPassword[0].text = "_";
        displayPassword[1].text = string.Empty;
        displayPassword[2].text = string.Empty;
        displayPassword[3].text = string.Empty;
    }
    IEnumerator CorrectPassword()
    {
        yield return new WaitForSeconds(2f);
        CloseDisplay();
        creatorScript.UnlockDoors();
        Destroy(creatorScript);
    }

    void CloseDisplay()
    {
        creatorScript.SwitchCanvas();
        Destroy(display);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        creatorScript.objectVisible = false;
        PauseController.SetPause(false);
        creatorScript.enabled = true;
    }
    
}
