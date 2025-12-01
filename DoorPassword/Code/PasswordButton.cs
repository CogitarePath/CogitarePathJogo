using System.Security.Cryptography;
using TMPro;
using UnityEngine;

public class PasswordButton : DoorPassword
{
    TextMeshProUGUI keyNumber;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        keyNumber = gameObject.GetComponentInChildren<TextMeshProUGUI>();
    }

    public void Click(int number)
    {
        ClickNumber(keyNumber.text);
    }

}
