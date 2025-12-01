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
        enemyScript = other.GetComponent<PatrolEnemy>();

        if (enemyHit.gameObject.CompareTag("Enemy") && flashlight.stunMode && !enemyScript.SombradeBruxa.GetStunned)
        {

            enemyScript.SombradeBruxa.CanWalk = false;
            enemyScript.SombradeBruxa.GetStunned = true;
            
        }
    }

}
