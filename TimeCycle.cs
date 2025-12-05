using NUnit.Framework.Internal;
using System;
using System.Collections;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class TimeCycle : MonoBehaviour
{
    public TimertoDificult TimertoDificultScript;
    public Transform[] SpawnPoints;
    public GameObject[] EnemyTypePrefab;
    public Texture2D SkyBox_Night;
    public Texture2D SkyBox_Day;
    public Texture2D SkyBox_Sunrise;
    public Texture2D SkyBox_Sunset;

    public int EnemiesNumber;

    public int minutes;
    public int hours;
    public int days;

    private bool IsSafeZone = false;
    public bool CanSpawn;

    private Coroutine ThisCorroutine;
    public int Minutes_ { get { return minutes; } set { minutes = value; OnMinuteChange(value); } }
    public int Hours_ { get { return hours; } set { hours = value; SkyBoxChangeForHour(value); } }
    public int Days_ { get { return days; } set { days = value; } }

    private float TimeSecond;

    void Start()
    {
        CanSpawn = true;
        Hours_ = 16;
        Minutes_ = 59;

        if (!IsSafeZone)
        {
            ThisCorroutine = StartCoroutine(Spawn(40, 1));
        }
        else if (IsSafeZone)
        {
            StopCoroutine(ThisCorroutine);
        }

    }
    void Update()
    {
        TimeSecond += Time.deltaTime;
        if (TimeSecond >= 1)
        {
            Minutes_++;
            TimeSecond = 0;
        }
    }

    private void OnMinuteChange(int value)
    {
        if (value >= 60)
        {
            Hours_++;
            Minutes_ = 0;
        }
        if (Hours_ >= 24)
        {
            Days_++;
            Hours_ = 0;
        }
    }

    private void SkyBoxChangeForHour(int value)
    {
        if (value > 0 && value < 7)
        {
            StartCoroutine(SkyBoxChange(SkyBox_Night, SkyBox_Sunset, 40f));
        }
        if (value >= 8 && value < 18)
        {
            StartCoroutine(SkyBoxChange(SkyBox_Sunrise, SkyBox_Day, 40f));
        }
        if (value >= 18 && value < 22)
        {
            StartCoroutine(SkyBoxChange(SkyBox_Day, SkyBox_Sunrise, 40f));

        }
        if (value == 22)
        {
            StartCoroutine(SkyBoxChange(SkyBox_Sunrise, SkyBox_Night, 40f));
        }
    }


    private IEnumerator Instantiate(int InstantiateValor, int EnemyVariant, float InstantiateDelay)
    {
        InstantiateValor = InstantiateValor + TimertoDificultScript.MultiplySpawn;

        for (int i = 0; i < InstantiateValor; i++)
        {
            if (CanSpawn)
            {
                if (EnemiesNumber < 14)
                {

                    EnemiesNumber += InstantiateValor;
                    Debug.Log(InstantiateValor);
                    Debug.Log(EnemyVariant);
                    int spawnIndex = Random.Range(0, SpawnPoints.Length);
                    GameObject enemy = Instantiate(EnemyTypePrefab[EnemyVariant], SpawnPoints[spawnIndex].position, new Quaternion(), SpawnPoints[spawnIndex]);
                    enemy.GetComponent<NavMeshAgent>().Warp(SpawnPoints[spawnIndex].position);
                    yield return new WaitForSeconds(InstantiateDelay);
                }
                else
                {
                    CanSpawn = false;
                }
            }

        }
    }

    private IEnumerator Spawn(float SpawnDelay, int MultiplyDelay)
    {
        while (true)
        {
            if (Hours_ > 17)
            {
                for (int R = 0; R < 2; R++)
                {
                    StartCoroutine(Instantiate(Random.Range(0, 4), Random.Range(0, 2), 20 * MultiplyDelay));
                }
            }

            else if (Hours_ < 17 || Hours_ == 0)
            {
                for (int X = 0; X < 2; X++)
                {
                    StartCoroutine(Instantiate(Random.Range(0, 4), 0, 20));
                }
            }

            yield return new WaitForSeconds(SpawnDelay * 2);
        }
    }

    private IEnumerator SkyBoxChange(Texture2D a, Texture2D b, float time)
    {
        RenderSettings.skybox.SetTexture("_Texture1", a);
        RenderSettings.skybox.SetTexture("_Texture2", b);
        RenderSettings.skybox.SetFloat("_Blend", 0);
        for (float I = 0; I < time; I += Time.deltaTime)
        {
            RenderSettings.skybox.SetFloat("_Blend", I / time);
            yield return null;
        }
        RenderSettings.skybox.SetTexture("_Texture1", b);
    }



}