using System.Collections;
using UnityEngine;

public class BattleScript : MonoBehaviour
{
    [SerializeField] Transform SpawnPos; // the transform the units will spawn at

    [SerializeField] float SpawnDelay; // the delay on the spawning of the units

    [SerializeField] GameObject UnitPrefab; // the infantry object
    [SerializeField] GameObject TankPrefab; // the tank object

    int EnemyPicker;

    bool Spawning = false; // bool value to check if spawning of a unit is underway
    private void Update()
    {
        EnemyPicker = Random.Range(0,31); // a random number will be chosen between 0 and 30

        if(!Spawning) // if the spawning bool is false it will run the coroutine "Spawning Unit" once
        {
            StartCoroutine(SpawningUnit());
        }
    }

    IEnumerator SpawningUnit()
    {
        Spawning = true; // the spawning bool is set to "true" for the duration on the Coroutine
        if (EnemyPicker <= 25) // if the random value is smaller than or equal to 25 it will spawn the weaker infantry
        {
            GameObject UnitPrefabClone = Instantiate(UnitPrefab, SpawnPos.position, SpawnPos.rotation);
            yield return new WaitForSeconds(SpawnDelay);
        }
        else // otherwise it will spawn a stronger tank
        {
            GameObject UnitPrefabClone = Instantiate(TankPrefab, SpawnPos.position, SpawnPos.rotation);
            yield return new WaitForSeconds(SpawnDelay);
        }
        Spawning = false; // this bool being set to false will allow the coroutine to start again.
    }
}
