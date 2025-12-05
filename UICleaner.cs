using UnityEngine;

public class UICleaner : MonoBehaviour
{
    private int R;

    public void CleanInterface(GameObject ActiveThisObject, GameObject[] UIObjects, bool ActiveThisOBJ_ActiveType, bool UIOBJS_SetActiveType, GameObject ExclusionItem = default)
    {
        R = UIObjects.Length - 1;
        
        ActiveThisObject.SetActive(ActiveThisOBJ_ActiveType);
        foreach (GameObject uiobject in UIObjects)
        {
            UIObjects[R].SetActive(UIOBJS_SetActiveType);
            R--;
        }
       if(ExclusionItem != null && ExclusionItem.activeSelf)
        {
            ExclusionItem.SetActive(false);
        }
    }
    public void ControlAnimations(Animator ThisAnimator, bool State, params string[] Tags)  //AnimationClip[] AnimationsClips)
    {
        int I = 0;
        foreach (var anims in Tags)
        {
            ThisAnimator.SetBool(Tags[I], State);
            I++;
        }
    }
}
