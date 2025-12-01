using UnityEngine;

public class ResidentialGenerator : Generators, IPlayerInteraction
{
    [SerializeField] private BoxCollider KeypadCollider;

    new public bool Interaction()
    {
        if(inventory.UseItem(neededItem, amountNeeded))
        {
            GeneratorCollision();
            return true;
        }
        else
        {
            return false;
        }
    }

    new public string InteractionMessage(bool completedInteraction)
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

    new public string LookMessage()
    {
        return lookMessage;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartGenerator();
        CreateUseMessage();

        InteractableId = CreateObjectId(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if (CanCharge)
        {
            Charge(0.1f);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        CanCharge = false;
    }

    public override void LoadUsed()
    {
        base.LoadUsed();
        KeypadCollider.enabled  = true;
    }

}
