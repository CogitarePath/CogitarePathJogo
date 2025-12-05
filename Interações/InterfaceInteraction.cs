using UnityEngine;
using System.Collections;

public class InterfaceInteraction : InteractableSave
{
    [SerializeField] protected GameObject objectInstance;
    [SerializeField] protected GameObject UIElementPrefab;
    [SerializeField] protected GameObject MainCanvas;
    [SerializeField] protected GameObject InteractionCanvas;

    static protected bool canvasSwitched;
    public bool CanClose = false;

    public void SwitchCanvas()
    {
        canvasSwitched = !canvasSwitched;
        if (canvasSwitched)
        {
            MainCanvas.SetActive(false);
            InteractionCanvas.SetActive(true);

        }
        else
        {
            MainCanvas.SetActive(true);
            InteractionCanvas.SetActive(false);

            foreach(Transform child in InteractionCanvas.transform)
            {
                Destroy(child.gameObject);
            }
        }
        PauseController.SetPause(canvasSwitched);
    }

    public IEnumerator Delay(float delayTime)
    {
        CanClose = false;
        yield return new WaitForSeconds(delayTime);
        CanClose = true;
    }

    public override void LoadUsed()
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


}
