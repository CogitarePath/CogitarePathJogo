using System.Collections;
using UnityEngine;
using System.Collections.Generic;

public class ActivateLights : MonoBehaviour
{

    [SerializeField] private Light[] spotlights = new Light[2];
    [SerializeField] private GameObject[] lightCameras = new GameObject[2];

    [SerializeField] private GameObject[] deactivateObjects;
    public static bool generatorActive;
    public static bool fuseActive;

    [SerializeField] private GameObject[] removeObstacles;

    private int currentCamera = 0;

    [SerializeField] private SoundManager soundManager;
    [SerializeField] private AudioClip activateSound;

    public void StartCutscene()
    {
        for(int i = 0; i < deactivateObjects.Length; i++)
        {
            deactivateObjects[i].SetActive(false);
        }
        PauseController.SetPause(true);


        lightCameras[currentCamera].SetActive(true);
        StartCoroutine(CameraChangeDelay());
    }

    IEnumerator CameraChangeDelay()
    {
        StartCoroutine(LightActivation());

        yield return new WaitForSeconds(3f);
        lightCameras[currentCamera].SetActive(false);
        currentCamera += 1;
        lightCameras[currentCamera].SetActive(true);
        StartCoroutine(LightActivation());
        yield return new WaitForSeconds(3f);
        PauseController.SetPause(false);
        for (int i = 0; i < deactivateObjects.Length; i++)
        {
            deactivateObjects[i].SetActive(true);
        }
        for(int i = 0;i < removeObstacles.Length; i++)
        {
            removeObstacles[i].SetActive(false);
        }
    }

    public void ActivateLoaded()
    {
        for( int i = 0; i < spotlights.Length; i++){
            spotlights[i].enabled = true;
        }
        for (int i = 0; i < removeObstacles.Length; i++)
        {
            removeObstacles[i].SetActive(false);
        }
    }

    IEnumerator LightActivation()
    {
        yield return new WaitForSeconds(1f);
        spotlights[currentCamera].enabled = true;
        soundManager.PlayAudio(soundManager.SoundEffectSource, activateSound, false, 1f);
    }

}
