using System.Collections;
using UnityEngine;

public class BattleScript : MonoBehaviour
{
    [SerializeField] Transform SpawnPos;

    [SerializeField] float SpawnDelay;

    [SerializeField] GameObject UnitPrefab;
    [SerializeField] GameObject TankPrefab;

    int EnemyPicker;

    bool Spawning = false;
    private void Update()
    {
        EnemyPicker = Random.Range(0,31);

        if(!Spawning)
        {
            StartCoroutine(SpawningUnit());
        }
    }

    IEnumerator SpawningUnit()
    {
        Spawning = true;
        if (EnemyPicker <= 25)
        {
            GameObject UnitPrefabClone = Instantiate(UnitPrefab, SpawnPos.position, SpawnPos.rotation);
            yield return new WaitForSeconds(SpawnDelay);
        }
        else 
        {
            GameObject UnitPrefabClone = Instantiate(TankPrefab, SpawnPos.position, SpawnPos.rotation);
            yield return new WaitForSeconds(SpawnDelay);
        }
        Spawning = false;
    }
}
