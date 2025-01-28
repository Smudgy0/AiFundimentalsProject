using UnityEngine;

public class ManpowerManager : MonoBehaviour
{
    public int Manpower;

    public UiManager UM;
    public PlanetBonusModifer PBM;

    int ManpowerMOD = 0;

    public void AddManpower()
    {
        Manpower = Manpower + (5 + PBM.EnemyAgression) * PBM.ManpowerUpgrades;
    }

    private void Update()
    {

        UM.CheckManpower();
    }

}
