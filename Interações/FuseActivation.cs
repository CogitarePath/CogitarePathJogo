using UnityEngine;

public class FuseActivation : ItemUse, IPlayerInteraction
{
    [SerializeField] private PlayerMove player;
    [SerializeField] private Gerador GeneratorCheck;

    [SerializeField] private ActivateLights activateLights;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        lookMessage = "Usar " + amountNeeded + "Fusíveis (E)";
        actionMessage = useMessage + amountNeeded + "Fusíveis";


        InteractableId = CreateObjectId(gameObject);
    }

    new public bool Interaction()
    {
        WasUsed = inventory.UseItem(neededItem, amountNeeded);
        if (WasUsed)
        {
            ActivateLights.fuseActive = true;
            if (destroyObj)
            {
                Destroy(gameObject);
            }
            else
            {
                Destroy(this);
            }

            if (ActivateLights.generatorActive)
            {
                activateLights.StartCutscene();
            }
        }
        return WasUsed;
    }

    public override void LoadUsed()
    {
        base.LoadUsed();
        activateLights.ActivateLoaded();
    }
}
