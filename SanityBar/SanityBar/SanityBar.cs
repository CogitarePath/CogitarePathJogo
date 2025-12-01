using System.Drawing;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
//using UnityEngine.Color;
using Color = UnityEngine.Color;
using Image = UnityEngine.UI.Image;
public class SanityBar : MonoBehaviour
{

    public PlayerMove PlayerMoveScript;
    public Flashlight FlashlightScript;


    public UnityEngine.UI.Slider SanitySlider;

    public float MaxSanity, ActualSanity;

    public Animator playeranimator;

    private Color color;

    void Start()
    {
        MaxSanity = 100;
        ActualSanity = MaxSanity;
        SanitySlider.maxValue = MaxSanity;
        SanitySlider.value = ActualSanity;
    }

    void Update()
    {
        SanitySlider.value = ActualSanity;
    }

    public void Damage(float amount)
    {
        Debug.Log("damage");
        ActualSanity -= Time.deltaTime * 5;
        Debug.Log(ActualSanity);
        if (ActualSanity > MaxSanity)
        {
            ActualSanity = MaxSanity;
        }

        if (ActualSanity <= 0)
        {
            PlayerMoveScript.DeathReset();
            Debug.Log("Estado do jogador: Insano");
        }

        if (ActualSanity <= 30)
        {
            FlashlightScript.Insane = true;
            FlashlightScript.isOn = false;
            playeranimator.SetBool("InsaneIdle", true);
            playeranimator.SetBool("Normal_Idle", false);
        }
        else
        {
            FlashlightScript.Insane = false;
            FlashlightScript.isOn = true;
            playeranimator.SetBool("InsaneIdle", false);
            playeranimator.SetBool("Normal_Idle", true);
        }

        SanitySlider.value = ActualSanity;
    }
}
