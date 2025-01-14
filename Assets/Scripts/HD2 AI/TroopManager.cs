using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;

public class TroopManager : MonoBehaviour
{
    public ManpowerManager MM;
    public TMP_Text troopCount;

    private UiManager uiManager;

    private void Awake()
    {
        uiManager = FindAnyObjectByType<UiManager>();
    }

    public void AddManPower(int changeAmount)
    {
        CalculateManPowerDifference(MM.Manpower, changeAmount);
    }

    public void RemoveManPower(int changeAmount)
    {
        CalculateManPowerDifference(uiManager.currentlySelectedPlanet.pTroopCount, changeAmount);
    }

    void CalculateManPowerDifference(int resourceAmount, int changeAmount)
    {
        if (resourceAmount + changeAmount >= 0)
        {
            if (resourceAmount >= changeAmount)
            {
                MM.Manpower = MM.Manpower - changeAmount;
                uiManager.currentlySelectedPlanet.pTroopCount = uiManager.currentlySelectedPlanet.pTroopCount + changeAmount;
                trooptextchange();
            }
            else
                Debug.Log("Not Enough Stored Man Power");
        }
        else
            Debug.Log("Not Enough Planet Man Power");
    }

    private void trooptextchange()
    {
        troopCount.text = uiManager.currentlySelectedPlanet.pTroopCount.ToString();
    }
}
