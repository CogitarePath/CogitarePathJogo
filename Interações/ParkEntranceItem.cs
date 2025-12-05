using UnityEngine;

public class ParkEntranceItem : ItemUse, IPlayerInteraction
{
    [SerializeField] private HingeJoint door1joint;
    [SerializeField] private HingeJoint door2joint;
    [SerializeField] private Rigidbody door1body;
    [SerializeField] private Rigidbody door2body;
    [SerializeField] private GameObject entryBlock;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        CreateUseMessage();

        InteractableId = CreateObjectId(gameObject);
    }

    public override void LoadUsed()
    {
        base.LoadUsed();
        door1body.freezeRotation = false;
        door2body.freezeRotation = false;
        door1joint.useMotor = true;
        door2joint.useMotor = true;
        Destroy(entryBlock);
    }

    new public bool Interaction()
    {
        WasUsed = inventory.UseItem(neededItem, amountNeeded);
        if (WasUsed)
        {
            LoadUsed();
        }
        return WasUsed;
    }

}
