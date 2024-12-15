using System;
using System.Collections;
using UnityEngine;

public class BattleScript : MonoBehaviour
{
    [SerializeField] Transform SpawnPos;

    [SerializeField] float SpawnDelay;

    [SerializeField] GameObject UnitPrefab;

    bool Spawning = false;
    private void Update()
    {
        if(!Spawning)
        {
            StartCoroutine(SpawningUnit());
        }
    }

    IEnumerator SpawningUnit()
    {
        Spawning = true;
        GameObject UnitPrefabClone = Instantiate(UnitPrefab, SpawnPos.position, SpawnPos.rotation);
        yield return new WaitForSeconds(SpawnDelay);
        Spawning = false;
    }
}
