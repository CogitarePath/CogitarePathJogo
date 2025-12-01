using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy
{
    public string EnemyName;
    public float SanityDamage;
    public float Speed;
    public float DamageDistance;
    public float PursuitDistance;
    public bool CanWalk;
    public bool PlayerInRadius;
    public bool GetStunned;
    public Enemy(string name, float damage, float speed, float damagedistance, float Pursuitdistance, bool canwalk, bool playerInradius, bool getstunned)
    {
        EnemyName = name;
        SanityDamage = damage;
        Speed = speed;
        DamageDistance = damagedistance;
        PursuitDistance = Pursuitdistance;
        CanWalk = canwalk;
        PlayerInRadius = playerInradius;
        GetStunned = getstunned;
    }

}

    public class PatrolEnemy : MonoBehaviour
{
    public UICleaner UICleanerScript;
    public SoundManager SoundManagerScript;

    public Enemy SombradeBruxa = new Enemy("Sombra de Bruxa", 0.05f, 15f, 8, 10, true, false, false);
    public Enemy PlantaOlhodeGato = new Enemy("Planta Olho de Gato", 0.05f, 10, 6, 16, true, false, false);

    protected GameObject[] findPoints;
    protected Transform[] positions;

    [SerializeField] protected SanityBar sanity;
    [SerializeField] protected NavMeshAgent enemy;
    [SerializeField] protected Transform player;
    [SerializeField] protected int nextPosition;

    public static float shadowEnemyRange = 8f;
    public float destinationDistance = 1.5f;

    public bool ClipAlreadyActive = false;
    protected void SetupWaypoints()
    {
        SombradeBruxa.CanWalk = true;
        findPoints = GameObject.FindGameObjectsWithTag("EnemyPoint");

        positions = new Transform[findPoints.Length];
        for (int i = 0; i < findPoints.Length; i++)
        {
            positions[i] = findPoints[i].transform;
        }

        nextPosition = UnityEngine.Random.Range(0, findPoints.Length);

        enemy.SetDestination(positions[nextPosition].position);

    }

    protected void Components()
    {
        GameObject UICleanerRef = GameObject.FindGameObjectWithTag("UICleaner");
        if (UICleanerRef != null)
        {
            UICleanerScript = UICleanerRef.GetComponent<UICleaner>();
        }

        GameObject SoundManagerRef = GameObject.FindWithTag("SoundManager");
        if (SoundManagerRef != null)
        {
            SoundManagerScript = SoundManagerRef.GetComponent<SoundManager>();
            Debug.Log(SoundManagerRef.activeInHierarchy);
        }
        player = GameObject.FindGameObjectWithTag("Player").transform;
        enemy = GetComponent<NavMeshAgent>();

        
    }
    protected void Patrol()
    {
        enemy.isStopped = false;
        if (Vector3.Distance(transform.position, enemy.destination) < destinationDistance )
        {
            enemy.SetDestination(positions[nextPosition].position);
            nextPosition = UnityEngine.Random.Range(0, findPoints.Length);
        }
    }


    public void InRadiusVerify()
    {
        if (SombradeBruxa.PlayerInRadius)
        {
            if (ClipAlreadyActive) return;
            SoundManagerScript.PlayAudio(SoundManagerScript.EnemySource, SoundManagerScript.Audios[2], true);
            ClipAlreadyActive = true;   

        }
        else if (!SombradeBruxa.PlayerInRadius)
        {
            if (ClipAlreadyActive)
            {
                ClipAlreadyActive = false;
                SoundManagerScript.StopAudio(SoundManagerScript.EnemySource, false);
            }

        }
    }

    public void DecreaseSanity(float range)
    {
        sanity.Damage(0.05f);

    }

    public void Stun()
    {
        Debug.Log("gotStunned");
        SombradeBruxa.GetStunned = false;
        StartCoroutine(StunDelay());

    }

    public IEnumerator StunDelay()
    {
        yield return new WaitForSeconds(6);
        enemy.isStopped = false;
        SombradeBruxa.CanWalk = true;
        Debug.Log("StopStun");
    }

}
