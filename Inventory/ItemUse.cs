using UnityEngine;

public class ItemUse : InteractableSave
{
    [SerializeField] protected string useMessage;
    public InventoryControl inventory;

    //protected ItemUse script;
    public InventoryItem neededItem;
    public int amountNeeded;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        InteractableId = CreateObjectId(gameObject);

        //script = GetComponent<ItemUse>();

        CreateUseMessage();
        
    }

    protected void CreateUseMessage()
    {
        if (amountNeeded != 1)
        {
            // coloca a mensagem no plural
            lookMessage = "Usar " + amountNeeded + " " + neededItem.itemName + "s (E)";
            actionMessage = useMessage + " " + amountNeeded + " " + neededItem.itemName + "s";
            failMessage = "Você precisa de " + amountNeeded + " " + neededItem.itemName + "s";
        }
        else
        {
            lookMessage = "Usar " + amountNeeded + " " + neededItem.itemName + " (E)";
            actionMessage = useMessage + " " + amountNeeded + " " + neededItem.itemName;
            failMessage = "Você precisa de " + amountNeeded + neededItem.itemName;

        }
    }

    public override void LoadUsed()
    {
        if (destroyObj)
        {
            Destroy(gameObject);
        }
        else
        {
            Destroy (this);
        }
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

    public bool Interaction()
    {
        return BasicUseInteraction();
    }

    protected bool BasicUseInteraction()
    {
        WasUsed = inventory.UseItem(neededItem, amountNeeded);
        if (WasUsed)
        {
            if (destroyObj)
            {
                Destroy(gameObject);
            }
            else
            {
                Destroy(this);
            }

        }
        return WasUsed;
    }
}
