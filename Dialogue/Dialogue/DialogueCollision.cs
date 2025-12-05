using UnityEngine;
using UnityEngine.InputSystem.LowLevel;

public class DialogueCollision : MonoBehaviour, IPlayerInteraction
{
    [SerializeField] private DialogueSystem DialogueSystemScript;
    [SerializeField] private PlayerMove PlayerMoveScript;
    public UICleaner UICleanerScript;
    public GameObject[] UIObjects;
    public GameObject DialogueOBJ;
    public LayerMask PlayerLayer;

    [SerializeField] string talkMessage;

    public string PlayerName;
    public string[] PlayerDialogueLines;
    public Sprite PlayerIcon;

    public string NPCName;
    public string[] NPCDialogueLines;
    public Sprite NPCIcon;

    public float Radius;


    private bool OnRadius;
    private bool IsInteracting;

    void Start()
    {
        GameObject PlayerMovRef = GameObject.FindGameObjectWithTag("Player");
        if (PlayerMovRef != null)
        {
            PlayerMoveScript = PlayerMovRef.GetComponent<PlayerMove>();
        }
        DialogueOBJ.SetActive(false);
        IsInteracting = false;
    }
    void Update()
    {
        VerifyCollision();
    }

    public void VerifyCollision()
    {
        Collider[] Hit = Physics.OverlapSphere(transform.position, Radius, PlayerLayer);
        OnRadius = (Hit.Length > 0);

        if (OnRadius && Input.GetKeyDown(KeyCode.E) && !IsInteracting)
        {
            PlayerMoveScript.SwitchtoFreeLock = true;
            IsInteracting = true;
            UICleanerScript.CleanInterface(DialogueOBJ, UIObjects, true, false);

            DialogueSystemScript.ButtonObject.SetActive(false);
            DialogueSystemScript.CanSkip = true;

            DialogueSystemScript.RestartDialogueIndex();
            DialogueSystemScript.InsertDialogue(PlayerIcon, PlayerName, PlayerDialogueLines, NPCIcon, NPCName, NPCDialogueLines);
            DialogueSystemScript.MouseState(false);
        }

        if (!OnRadius && IsInteracting)
        {
            PlayerMoveScript.SwitchtoFreeLock = false;
            DialogueSystemScript.DoesAChoice = false;
            IsInteracting = false;
            DialogueOBJ.SetActive(false);

            UICleanerScript.CleanInterface(DialogueOBJ, UIObjects, false, true);

            DialogueSystemScript.ClearVisibleLines();
            DialogueSystemScript.SaveStartSets();
            DialogueSystemScript.RestartDialogueIndex();
            DialogueSystemScript.MouseState(true);
            DialogueSystemScript.SetStartDialogue();
        }

        if (IsInteracting && Input.GetKeyDown(KeyCode.Space))
        {
            DialogueSystemScript.SkipDialogue();
        }
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, Radius);
    }

    public bool Interaction()
    {
        return true;
    }

    public string InteractionMessage(bool completedInteraction)
    {
        return "";
    }

    public string LookMessage()
    {
        return talkMessage;
    }
}