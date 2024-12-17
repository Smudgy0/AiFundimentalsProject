using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TDManager : MonoBehaviour
{
    [SerializeField] int health;
    [SerializeField] float delayBetweenSpawns;
    [SerializeField] float waveDelay;
    [SerializeField] int waveCount;

    [SerializeField] int amountToSpawn;
    [SerializeField] int bloonsAlive;   

    [SerializeField] GameObject ballonPrefab;
    [SerializeField] GameObject TankPrefab;
    [SerializeField] GameObject SpeedPrefab;

    int EnemyPicker;
    int leftoverBollons;

    public GameObject[] waypoints;

    private void Start()
    {
        StartWave();
    }

    #region Waves

    void StartWave()
    {
        amountToSpawn = (int)Mathf.Sqrt(waveCount * 25);
        bloonsAlive = amountToSpawn + leftoverBollons;
        leftoverBollons = 0;
        StartCoroutine("SpawnLoop");
    }

    void EndWave()
    {
        waveCount++;
        Invoke("StartWave", waveDelay);
    }

    public void ForceEndWave()
    {
        leftoverBollons = bloonsAlive;
        CancelInvoke("StartWave");
        EndWave();
    }

    IEnumerator SpawnLoop()
    {
        int amountSpawned = 0;
        while (amountSpawned < amountToSpawn)
        {
            SpawnBloon();
            amountSpawned++;
            yield return new WaitForSeconds(delayBetweenSpawns);
        }
    }

    void SpawnBloon()
    {
        EnemyPicker = UnityEngine.Random.Range(0, 101);
        if (EnemyPicker < 65)
        {
            GameObject ballon = Instantiate(ballonPrefab, waypoints[0].transform.position, waypoints[0].transform.rotation);
        }
        else if (EnemyPicker >= 65 && EnemyPicker <= 85 && waveCount > 4)
        {
            GameObject Tank = Instantiate(TankPrefab, waypoints[0].transform.position, waypoints[0].transform.rotation);
        }
        else if (EnemyPicker > 85 && waveCount > 2) 
        {
            GameObject Speed = Instantiate(SpeedPrefab, waypoints[0].transform.position, waypoints[0].transform.rotation);
        }
        else
        {
            GameObject ballon = Instantiate(ballonPrefab, waypoints[0].transform.position, waypoints[0].transform.rotation);
        }
    }
    #endregion End region

    public void ChangeHealth(int amount)
    {
        health -= amount;
        health = Math.Clamp(health, 0, 50);
        Debug.Log("$You lost {health} health");

        if (health == 0)
        {
            Debug.Log("You Lose");
        }
    }

    public void KillBloon()
    {
        bloonsAlive--;

        if (bloonsAlive <= 0)
        {
            EndWave();
        }
    }
}
