using UnityEngine;

public class PlanetBonusModifer : MonoBehaviour
{
    [SerializeField] public int Resources = 0;

    [SerializeField] public int EnemyAgression = 1;

    [SerializeField] GameObject BetterDefenseButton;
    [SerializeField] GameObject BetterDefenseText;

    [SerializeField] GameObject BetterManpowerButton;
    [SerializeField] GameObject BetterManpowerText;

    public int DefenseUpgrades = 1;
    public int ManpowerUpgrades = 1;

    public void AddResources()
    {
        Resources = Resources + 2;
    }

    public void ImporvedDefenses()
    {
        if(Resources > 1000 && DefenseUpgrades != 5)
        {
            DefenseUpgrades = DefenseUpgrades + 1;
            Resources = Resources - 1000;
        }
        else if (DefenseUpgrades == 5)
        {
            BetterDefenseButton.SetActive(false);
            BetterDefenseText.SetActive(false);
        }
    }

    public void ImprovedManpower()
    {
        if (Resources > 1000 && ManpowerUpgrades != 5)
        {
            ManpowerUpgrades = ManpowerUpgrades + 1;
            Resources = Resources - 1000;
        }
        else if (ManpowerUpgrades == 5)
        {
            BetterManpowerButton.SetActive(false);
            BetterManpowerText.SetActive(false);
        }
    }

    public void IncreaseAiAttackRate()
    {
        if (EnemyAgression !< 5)
        {
            EnemyAgression = EnemyAgression + 1;
        }
    }

    public void DecreaseAiAttackRate()
    {
        if (EnemyAgression! > 1)
        {
            EnemyAgression = EnemyAgression - 1;
        }
    }
}
