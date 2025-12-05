using System;
using UnityEngine;
using UnityEngine.Events;

public class PauseController : MonoBehaviour
{
    public static PlayerMove player;


    public static bool IsPaused { get; private set; } = false;

    public static void SetPause(bool pause)
    {
        IsPaused = pause;
        player.freeze = IsPaused;
        if (pause)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    private void OnEnable()
    {
        gameObject.SetActive(true);
        player = GameObject.FindWithTag("Player").GetComponent<PlayerMove>();
        SetPause(false);
    }

}
