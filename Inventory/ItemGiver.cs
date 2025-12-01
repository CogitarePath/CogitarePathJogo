using UnityEngine;

public class ItemGiver : InteractableSave, IPlayerInteraction
{

    private ItemGiver script;

    public InventoryControl inventory;

    public InventoryItem givenItem;

    protected void Awake()
    {
        InteractableId = CreateObjectId(gameObject);
        script = GetComponent<ItemGiver>();

        // clona o scriptableObject para que o original não seja alterado
        givenItem = ScriptableObject.Instantiate(givenItem);

        CreategiveMessage();
    }

    void CreategiveMessage()
    {
        if (givenItem.itemAmount != 1)
        {
            // coloca a mensagem no plural
            lookMessage = "Pegar " + givenItem.itemAmount + " " + givenItem.itemName + "s (E)";
            actionMessage = "Guardou " + givenItem.itemAmount + " " + givenItem.itemName + "s";
        }
        else
        {
            lookMessage = "Pegar " + givenItem.itemAmount + " " + givenItem.itemName + " (E)";
            actionMessage = "Guardou " + givenItem.itemAmount + " " + givenItem.itemName;
        }
        failMessage = "Inventário cheio";
    }

    public bool Interaction()
    {

        return BasicGiveInteraction();

    }

    public string LookMessage()
    {
        return lookMessage;
    }

    public string InteractionMessage(bool completedInteraction)
    {
        if (completedInteraction)
        {
            return actionMessage;
        }
        else
        {
            return failMessage;
        }
    }

    public override void LoadUsed()
    {
        Debug.Log(gameObject);
        Destroy(gameObject);
    }

    protected bool BasicGiveInteraction()
    {
        WasUsed = inventory.GetItem(givenItem);

        if (WasUsed)
        {
            LoadUsed();
        }
        return WasUsed;
        //remove o script para que o jogador não pegue o mesmo item denovo
    }
}
