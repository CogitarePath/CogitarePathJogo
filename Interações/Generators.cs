using System.Collections;
using TMPro;
using UnityEngine;

public class Generators : ItemUse
{
    public PlayerMove PlayerMoveScript;
    public UICleaner UICleanerScript;
    public Animator PlayerAnimator;

    public TMP_Text ValueText;

    protected float MaxGas, ActualGas;
    [SerializeField] protected bool CanCharge;

    public UnityEngine.UI.Slider gasSlider;

    [SerializeField] protected Transform lockPlayerPosition;

    protected void StartGenerator()
    {
        GameObject UICleanerRef = GameObject.FindGameObjectWithTag("UICleaner");
        if (UICleanerRef != null)
        {
            UICleanerScript = UICleanerRef.GetComponent<UICleaner>();
        }
        MaxGas = 100;
        gasSlider.maxValue = MaxGas;

        ActualGas = 0;
        gasSlider.value = ActualGas;


        ValueText.text = "" + ActualGas;
    }


    protected void GeneratorCollision()
    {
        PlayerAnimator.SetBool("UseGerator", true);
        UICleanerScript.ControlAnimations(PlayerAnimator, true, "UseGerator");
        UICleanerScript.ControlAnimations(PlayerAnimator, false, "Walk", "Run", "Normal_Idle");

        PlayerMoveScript.freeze = true;

        CanCharge = true;

    }


    protected void Charge(float amount)
    {
        ValueText.text = "" + ActualGas;
        gasSlider.value = ActualGas;
        if (ActualGas < MaxGas)
        {
            PlayerAnimator.gameObject.transform.SetPositionAndRotation(lockPlayerPosition.position, new Quaternion(0, 0.999783337f, 0, -0.020817453f));
            ActualGas += amount;
        }
        else if (ActualGas >= MaxGas)
        {
            ActualGas = 100;
            WasUsed = true;
            LoadUsed();
            CanCharge = false;
            UICleanerScript.ControlAnimations(PlayerAnimator, false, "UseGerator");
            UICleanerScript.ControlAnimations(PlayerAnimator, true, "Normal_Idle");
            PlayerMoveScript.freeze = false;
        }
    }

    public override void LoadUsed()
    {
        Destroy(this);
        ValueText.text = "100";
    }

}
