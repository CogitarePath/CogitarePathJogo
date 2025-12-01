using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CatEnemy : PatrolEnemy
{

    public Animator OlhodeGatoAnimator;
    public AudioClip EnemyEffect;

    private Collider[] nearShadowEnemies;
    private NavMeshAgent[] shadowAgents;
    private LayerMask shadowsMask;

    public float catEnemyRange = 13f;

    void Start()
    {
        OlhodeGatoAnimator.SetBool("EarthExit", true);
    }

    private void Awake()
    {
        Components();

        shadowsMask = LayerMask.GetMask("Shadows");
    }
    public void OnEnable()
    {
        
        SetupWaypoints();

    }

    void FixedUpdate()
    {
        InRadiusVerify();
        if (PlantaOlhodeGato.GetStunned)
        {
            Stun();
        }
        if (PlantaOlhodeGato.CanWalk)
        {
            UICleanerScript.ControlAnimations(OlhodeGatoAnimator, false, "EarthEntry", "EarthExit");
            UICleanerScript.ControlAnimations(OlhodeGatoAnimator, true, "Walk");

            if ((Vector3.Distance(transform.position, player.position) < catEnemyRange))
            {
                Alert();
                PlantaOlhodeGato.PlayerInRadius = true;
                UICleanerScript.ControlAnimations(OlhodeGatoAnimator, false, "Walk");
                UICleanerScript.ControlAnimations(OlhodeGatoAnimator, true, "SpotPlayer");
                DecreaseSanity(catEnemyRange);
            }
            else
            {
                Patrol();
                shadowEnemyRange = 6f;
                PlantaOlhodeGato.PlayerInRadius = false;
                UICleanerScript.ControlAnimations(OlhodeGatoAnimator, false, "SpotPlayer");
            }

            if ((Vector3.Distance(transform.position, player.position) < catEnemyRange / 2))
            {
                UICleanerScript.ControlAnimations(OlhodeGatoAnimator, true, "EarthEntry");
                UICleanerScript.ControlAnimations(OlhodeGatoAnimator, false, "SpotPlayer");
            }
            else
            {
                UICleanerScript.ControlAnimations(OlhodeGatoAnimator, true, "EarthExit");
                UICleanerScript.ControlAnimations(OlhodeGatoAnimator, false, "EarthEntry");

            }

        }
    }

    void Alert()
    {   
        enemy.isStopped = true;
        nearShadowEnemies = Physics.OverlapSphere(transform.position, catEnemyRange - 3f,  shadowsMask);
        shadowAgents = new NavMeshAgent[nearShadowEnemies.Length];

        for (int i = 0; i < nearShadowEnemies.Length; i++)
        {

            shadowAgents[i] = nearShadowEnemies[i].gameObject.GetComponent<NavMeshAgent>();
            shadowAgents[i].SetDestination(player.position);
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, catEnemyRange - 3f);
    }

}
