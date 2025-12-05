using UnityEngine;

public class ShowPages : InterfaceInteraction, IPlayerInteraction
{
    public int pageId;

    private void Start()
    {
        lookMessage = "Ler Páginas (Aperte E)";
        actionMessage = "";
        InteractableId = CreateObjectId(gameObject);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && CanClose)
        {
            SwitchCanvas();
            Destroy(objectInstance);
            Destroy(gameObject);
            AddToMenu();
        }
    }

    public bool Interaction()
    {
        if (WasUsed) return true;
        SwitchCanvas();
        objectInstance = Instantiate(UIElementPrefab, InteractionCanvas.transform);
        StartCoroutine(Delay(2f));
        WasUsed = true;
        return true;
    }

    public string LookMessage()
    {
        return lookMessage;
    }

    public string InteractionMessage(bool a)
    {
        return actionMessage;
    }

    public override void LoadUsed()
    {
        Destroy(gameObject);
    }

    private void AddToMenu()
    {
        GameMenu.collectedPages[pageId] = UIElementPrefab;
    }
}
