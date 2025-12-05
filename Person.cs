using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "Person", menuName = "Scriptable Objects/Person")]
public class DialoguePerson : ScriptableObject
{
    public string PersonName;
    public string[] PersonLines;
    public Sprite PersonSprite;

    public float TypingSpeed;
    public AudioClip[] LineAudios;

    public bool SequenceLines;
    public float AutoProgressionDelay;
}

public class DialogueSystemI : MonoBehaviour
{
    public DialogueAudio DialogueAudioScript;

    public AudioSource DialogueAudioSource;

    public GameObject DialogueOBJ;

    public DialoguePerson DialogueData;
    public Image PersonIcon;

    public TMP_Text PersonName, DialogueLines;

    public AudioClip[] DialogueAudios;

    public int DialogueIndex;

    public bool IsDialogueTyping, IsDialogueActive;

    public bool CanInteract()
    {
        return !IsDialogueActive;
    }


    public void StartDialogue()
    {
        IsDialogueActive = true;
        PersonName.SetText(DialogueData.name);
        PersonIcon.sprite = DialogueData.PersonSprite;
        DialogueOBJ.SetActive(true);
    }
    public void Interact()
    {
        if (DialogueData == null || !IsDialogueActive)
            return;

        if (IsDialogueActive)
        {
            StartDialogue();
        }

    }

    public IEnumerator DialogueTyping()
    {
        IsDialogueTyping = true;
        
            foreach (char playerletter in DialogueData.PersonLines[DialogueIndex].ToCharArray())
            {
                DialogueAudioScript.CallDub(DialogueAudioSource, DialogueAudios, DialogueIndex);
                DialogueLines.text += playerletter;
                yield return new WaitForSeconds(DialogueData.TypingSpeed);
            }
        IsDialogueTyping = false;
    }
}


