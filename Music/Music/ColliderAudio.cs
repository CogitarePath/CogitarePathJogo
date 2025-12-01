using UnityEngine;

public class ColliderAudio : SoundManager
{
    public AudioClip ThisAudio;

    public int Priority = 128;
    public float Volume;
    public bool IsLoop;
    private void OnTriggerEnter(Collider other)
    {
        PlayAudio(EnemySource, ThisAudio, IsLoop, Volume, Priority);
        Debug.Log("Entrou");
    }
}
