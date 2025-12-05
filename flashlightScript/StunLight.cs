using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class StunLight : MonoBehaviour
{
    private Rigidbody enemyHit;

    [SerializeField] private Flashlight flashlight;
    [SerializeField] private PatrolEnemy enemyScript;

    private void OnTriggerStay(Collider other)
    {
        enemyHit = other.GetComponent<Rigidbody>();
        enemyScript = other.GetComponent<ShadowEnemy>();
        Debug.Log(other.gameObject);
        if (enemyHit.gameObject.CompareTag("Enemy") && flashlight.stunMode && !enemyScript.GetStunned)
        {
            enemyHit.linearVelocity = Vector3.zero;
            Debug.Log("Puxou o Trigger");
            enemyScript.SombradeBruxa.CanWalk = false;
            enemyScript.SombradeBruxa.GetStunned = true;
            
        }
    }

}
