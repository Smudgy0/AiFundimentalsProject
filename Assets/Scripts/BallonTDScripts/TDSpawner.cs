using System.Collections;
using UnityEngine;

public class TDSpawner : MonoBehaviour
{
    [SerializeField] Transform SpawnPos;

    [SerializeField] float SpawnDelay;

    [SerializeField] GameObject UnitPrefab;
    [SerializeField] GameObject TankPrefab;
    [SerializeField] GameObject SpeedPrefab;

    int EnemyPicker;

    bool Spawning = false;
    private void Update()
    {
        EnemyPicker = Random.Range(0, 101);

        if (!Spawning)
        {
            StartCoroutine(SpawningUnit());
        }
    }

    IEnumerator SpawningUnit()
    {
        Spawning = true;
        if (EnemyPicker < 65)
        {
            GameObject UnitPrefabClone = Instantiate(UnitPrefab, SpawnPos.position, SpawnPos.rotation);
            yield return new WaitForSeconds(SpawnDelay);
        }
        if (EnemyPicker >= 65  && EnemyPicker <= 85)
        {
            GameObject UnitPrefabClone = Instantiate(TankPrefab, SpawnPos.position, SpawnPos.rotation);
            yield return new WaitForSeconds(SpawnDelay);
        }
        else
        {
            GameObject UnitPrefabClone = Instantiate(SpeedPrefab, SpawnPos.position, SpawnPos.rotation);
            yield return new WaitForSeconds(SpawnDelay);
        }
        Spawning = false;
    }
}
