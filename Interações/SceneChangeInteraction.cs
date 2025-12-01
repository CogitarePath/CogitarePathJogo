using System;
using System.Collections;
using Random = UnityEngine.Random;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneChangeInteraction : Interactable, IPlayerInteraction
{
 
    [SerializeField] string SceneName;
    [SerializeField] Vector3 newScenePosition;

    [SerializeField] SaveControl saveControl;

    public bool Interaction()
    {
        ScenePositionControl.sceneStartPosition = newScenePosition;
        Debug.Log(newScenePosition + "cenaaaaaaaaa");

        saveControl.SaveGameData();
        //saveControl.SaveTemporaryData();
        SceneManager.LoadScene(SceneName);

        return true;
    }

    public string InteractionMessage(bool completedInteraction)
    {
        return actionMessage;
    }

    public string LookMessage()
    {
        return lookMessage;
    }
}
