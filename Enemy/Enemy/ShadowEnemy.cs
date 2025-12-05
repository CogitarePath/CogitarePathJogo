using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ShadowEnemy : PatrolEnemy
{
    public Animator SombradeBruxaAnimator;
    public AudioClip[] EnemyEffects;

    public float ShadowEnemyRange;

    private void Awake()
    {
        Components();
    }
    public void OnEnable()
    {
        
        SetupWaypoints();
    }

    void FixedUpdate()
    {
        InRadiusVerify();

        if (SombradeBruxa.GetStunned)
        {
            SoundManagerScript.PlayAudio(SoundManagerScript.EnemySource, SoundManagerScript.Audios[1], false);
            Stun();
        }

        if (SombradeBruxa.CanWalk)
        {

            UICleanerScript.ControlAnimations(SombradeBruxaAnimator, false, "Chasing");

            if ((Vector3.Distance(transform.position, player.position) < shadowEnemyRange + ShadowEnemyRange)) {

                UICleanerScript.ControlAnimations(SombradeBruxaAnimator, true, "Chasing");

                enemy.SetDestination(player.position);
                SombradeBruxa.PlayerInRadius = true;

                DecreaseSanity(shadowEnemyRange);
            }
            else
            {
                Patrol();
                SombradeBruxa.PlayerInRadius = false;
            }

        }

    }


    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, shadowEnemyRange + ShadowEnemyRange);
    }


}
