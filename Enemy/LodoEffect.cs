using System;
using System.Collections;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class LodoEffect : MonoBehaviour
{
    public SanityBar SanityBarScript;
    public PlayerMove PlayerMovScript;
    private Coroutine ThisCoroutine;

    private void OnTriggerEnter(Collider other)
    {
       ThisCoroutine = StartCoroutine(SanityDamage());
    }

    private void OnTriggerStay(Collider other)
    {
        PlayerMovScript.moveSpeed = PlayerMovScript.moveSpeed / 4;
       
    }

    private void OnTriggerExit(Collider other)
    {
        StopCoroutine(ThisCoroutine);
    }

    private IEnumerator SanityDamage()
    {
        while (true) {
            SanityBarScript.Damage(0.80f);
            yield return new WaitForSeconds(0.5f);
        }
  
    }

}

