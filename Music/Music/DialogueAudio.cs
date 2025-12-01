using UnityEngine;

public class DialogueAudio : SoundManager
{
    public DialogueSystem DialogueSystemScript;
    void Start()
    {
        GameObject DialogueSystemRef = GameObject.FindWithTag("DialogueSystem");
        if (DialogueSystemRef != null)
        {
            DialogueSystemScript = DialogueSystemRef.GetComponent<DialogueSystem>();
        }
    }

    public void CallDub(AudioSource AudioSource, AudioClip[] LineAudio, int AudioIndex)
    {
        PlayAudio(AudioSource, LineAudio[AudioIndex], false, 0.4f);
    }
}
