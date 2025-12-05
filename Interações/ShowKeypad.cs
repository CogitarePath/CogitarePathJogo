using UnityEngine;

public class ShowKeypad : InterfaceInteraction, IPlayerInteraction
{
    DoorPassword generatedScript;
    public bool objectVisible = false;

    private Rigidbody doorBody;
    private HingeJoint doorHinge;

    private GameObject interactDoor;

    void Start()
    {
        if(!objectVisible){
            lookMessage = "Digitar Senha (Aperte E)";
            actionMessage = lookMessage;
        }
        InteractableId = CreateObjectId(gameObject);

        doorBody = GetComponentInChildren<Rigidbody>();
        doorHinge = GetComponentInChildren<HingeJoint>();
        
        doorBody.freezeRotation = true;

        interactDoor = GetComponentInChildren<SceneChangeInteraction>().gameObject;

        InteractableId = CreateObjectId(gameObject);
    }

    public bool Interaction()
    {
        if (objectVisible)
        {
            return true;
        }
        else
        {
            SwitchCanvas();
            objectInstance = Instantiate(UIElementPrefab, InteractionCanvas.transform);
            generatedScript = objectInstance.GetComponentInChildren<DoorPassword>();

            generatedScript.creatorScript = this;
            StartCoroutine(Delay(0.2f));
            this.enabled = false;

            return true;
        }
            
    }

    public void UnlockDoors()
    {
        doorBody.freezeRotation = false;
        doorHinge.useMotor = true;
        interactDoor.GetComponent<BoxCollider>().enabled = true;
    }

    public string InteractionMessage(bool completedInteraction)
    {
        return actionMessage;
    }

    public override void LoadUsed()
    {
        Destroy(this);
        UnlockDoors();
    }

    public string LookMessage()
    {
        return lookMessage;
    }

}
