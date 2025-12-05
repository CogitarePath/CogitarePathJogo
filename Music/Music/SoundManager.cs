using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour
{
    public AudioSource SoundEffectSource;
    public AudioSource AmbienceSource;
    public AudioSource EnemySource;
 
    public AudioClip[] Audios;
    public AudioClip ThisSceneAmbience;

    public bool IsPlaying;

    void Start()
    {
        // toca o som ambiente em loop
        PlayAudio(AmbienceSource, ThisSceneAmbience, true, 0.3f);
    }

    public void PlayAudio(AudioSource AudioSource, AudioClip Clip, bool IsLoop = false, float Volume = 0.5f, int Priority = 128)         
    {
            AudioSource.clip = Clip;
            AudioSource.loop = IsLoop;
            AudioSource.volume = Volume;
            AudioSource.priority = Priority;
            AudioSource.Play();
            IsPlaying = true;
    }

    public void StopAudio(AudioSource AudioSource, bool WaitTheFinal = false)
    {
        if (!WaitTheFinal)
        {
            AudioSource.clip = null;
            AudioSource.Stop();
            IsPlaying = false;
        }
        else if (WaitTheFinal)
        {
            if (!AudioSource.isPlaying)
            {
                AudioSource.clip = null;
                AudioSource.Stop();
                IsPlaying = false;
            }
        }
    }
}