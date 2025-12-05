using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Flashlight : MonoBehaviour
{
    public InventoryControl inventoryControl;
    public InventoryItem batteryItem;

    public UICleaner UIcleanerScript;
    public Animator PlayerAnimator;

    public GameObject batteryUI;
    public GameObject lightIcon;
    public GameObject SliderBackground;
    [SerializeField] private Color UVLight = new(0.6448423f, 0.3008188f, 0.9811321f);

    public SanityBar sanityBar;

    private Light lightComponent;

    public GameObject FlashlightOBJ;
    public GameObject ReferenceOBJ;
    public Slider flashlightIcon;

    public float maxBattery = 100f;
    private float batteryUse;
    public static float battery = 100f;

    public bool CantUse, Insane, isOn, stunMode;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameObject UICleanerRef = GameObject.FindGameObjectWithTag("UICleaner");
        if (UICleanerRef != null)
        {
            UIcleanerScript = UICleanerRef.GetComponent<UICleaner>();
        }

        isOn = false;
        lightComponent = GetComponentInChildren<Light>();

        flashlightIcon = GameObject.Find("flashlight").GetComponent<Slider>();

        flashlightIcon.maxValue = maxBattery;
        flashlightIcon.minValue = 0f;

        flashlightIcon.value = battery;
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        if (battery > 0)
        {
            if (Input.GetMouseButtonDown(0) && !Insane && !CantUse)
            {
                isOn = !isOn;
                SwitchLight();
                //Debug.Log("Estado da lanterna:" + isOn);
            }
            

            if (isOn && Input.GetMouseButtonDown(1))
            {
                stunMode = !stunMode;
                SwitchStun();
            }

            if (isOn)
            {
                FlashlightOBJ.SetActive(true);
                UIcleanerScript.ControlAnimations(PlayerAnimator, true, "TakeLantern");
                battery -= batteryUse;
                flashlightIcon.value = battery;
                SanityBar.ActualSanity = Mathf.Clamp(SanityBar.ActualSanity += 0.05f, 0f, 100f);
                sanityBar.SanitySlider.value = SanityBar.ActualSanity;
            }
            else
            {
                FlashlightOBJ.SetActive(false);
                UIcleanerScript.ControlAnimations(PlayerAnimator, false, "TakeLantern", "IdleLantern");
            }
        }
        else if (inventoryControl.UseItem(batteryItem, 1))
        {
            battery = 75f;
        }
        else
        {
            NoBattery();
        }

    }


    void SwitchLight()
    {
        if (isOn)
        {
            FlashlightOBJ.transform.position = ReferenceOBJ.transform.position;
            SliderBackground.SetActive(true);
            lightIcon.SetActive(true);
            lightComponent.enabled = true;
            batteryUse = 0.05f;
        }
        else
        {
            SliderBackground.SetActive(false);
            lightIcon.SetActive(false);
            lightComponent.enabled = false;
            stunMode = false;
        }
    }

    void SwitchStun()
    {
        if (stunMode)
        {
            SliderBackground.GetComponent<Image>().color = UVLight;
            lightIcon.GetComponent<Image>().color = UVLight;
           
            lightComponent.color = UVLight;
            batteryUse = 0.1f;

        }
        else
        {
            SliderBackground.GetComponent<Image>().color = Color.yellow;
            batteryUI.GetComponent<Image>().color = Color.yellow;
            lightIcon.GetComponent<Image>().color = Color.yellow;
        }
    }

    void NoBattery()
    {
        UIcleanerScript.ControlAnimations(PlayerAnimator, false, "TakeLantern", "IdleLantern");

        FlashlightOBJ.gameObject.SetActive(false);
        lightIcon.SetActive(false);

        isOn = false;
        stunMode = false;
        lightComponent.enabled = false;
    }
}

