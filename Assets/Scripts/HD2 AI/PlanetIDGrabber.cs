using System.Collections;
using NUnit.Framework.Constraints;
using NUnit.Framework.Internal;
using Unity.VisualScripting;
using UnityEngine;

public class PlanetIDGrabber : MonoBehaviour
{
    public Planet planet;

    public int PlanetObjectIdentifyer = 0;

    [SerializeField] InvasionManager InvasionMScript;
    [SerializeField] PlanetBonusModifer PBM;
    private UiManager UIM;
    [SerializeField] GameObject[] ConnectedPlanets;
    [SerializeField] bool NotUI = true;
    public GameObject LR;
    public ManpowerManager MM;

    bool InvokeOngoing = false;
    bool OngoingLosses = true;
    int EnemySizePicker;
    int AlliedLossPicker;

    int EnemeyProgressMOD = 1;
    int EnemeyAgressionMOD = 1;

    float Troopdif;

    bool allNeighboursPlayerControlled = true;

    private void Awake()
    {
        UIM = FindAnyObjectByType<UiManager>();
    }

    private void Start()
    {
        InvokeRepeating("ComabtLosses", 2, 2);
        if (NotUI == true)
        {
            InvokeRepeating("ManpowerCreation", 1, 1);
            InvokeRepeating("ResourceCreation", 1, 1);
            for (int i = 0; i < ConnectedPlanets.Length; i++)
            {
                GameObject LineClone = Instantiate(LR, Vector3.zero, Quaternion.identity);
                LineRenderer lr = LineClone.GetComponent<LineRenderer>();
                lr.SetPosition(0, transform.position);
                lr.SetPosition(1, ConnectedPlanets[i].transform.position);
            }
        }

        InvasionMScript.SetupPlanets();

        for (int i = 0; i < InvasionMScript.ListOfPlanets.Length; i++)
        {
            if (PlanetObjectIdentifyer == InvasionMScript.ListOfPlanets[i].planetID)
            {
                planet = InvasionMScript.ListOfPlanets[i];
                Debug.Log("Got planet " + planet.name);
            }
        }

        planet.invasionActive = false;
        planet.troopsInbound = false;

        bool allNeighboursPlayerControlled = this.isAllNeighboursPlayerControlled();

        if (!allNeighboursPlayerControlled) {
            planet.invasionActive = true;
        }

        Debug.Log("on start " + planet.name + 
            " troopsInbound " + planet.troopsInbound +
            " invasionActive " + planet.invasionActive
        );
    }

    void Update()
    {

        if(planet.eTroopCount < 0)
        {
            planet.eTroopCount = 0;
        }
        if (planet.pTroopCount < 0)
        {
            planet.pTroopCount = 0;
        }

        EnemyTroops();
        LiveCombat();

        EnemySizePicker = Random.Range(0, 4);
        AlliedLossPicker = Random.Range(0, 4);

        if (planet.invasionActive == true && planet.troopsInbound == false)
        {
            Debug.Log("Adding troops" + planet.name);
            StartCoroutine("AddInvadingEnemyTroops");
        }
        else if (planet.invasionActive == false)
        {
            StopCoroutine("AddInvadingEnemyTroops");
            planet.troopsInbound = false;
        }
    }

    void LiveCombat()
    {
        if (planet.eTroopCount > planet.pTroopCount)
        {
            Troopdif = planet.eTroopCount - planet.pTroopCount;
            Troopdif = Troopdif / (1000000 * PBM.DefenseUpgrades);
            planet.alliedControl = planet.alliedControl - Troopdif;
            planet.enemyControl = planet.enemyControl + Troopdif;
        }

        if (planet.pTroopCount > planet.eTroopCount)
        {
            Troopdif = planet.pTroopCount - planet.eTroopCount;
            Troopdif = Troopdif / 1000000;
            planet.enemyControl = planet.enemyControl - Troopdif;
            planet.alliedControl = planet.alliedControl + Troopdif;
        }

        if (planet.alliedControl > 100)
        {
            planet.alliedControl = 100;
        }

        else if (planet.alliedControl < 0)
        {
            planet.alliedControl = 0;
        }

        if (planet.enemyControl > 100)
        {
            planet.enemyControl = 100;
            StopCoroutine("AddInvadingEnemyTroops");
            StopCoroutine("ComabtLosses");
        }

        else if (planet.enemyControl < 0)
        {
            planet.enemyControl = 0;
        }

        if (planet.alliedControl == 100)
        {
            if (!planet.PlayerControlled)
            {
                Debug.Log(planet.name + " has been taken");
            }


            bool allNeighboursPlayerControlled = this.isAllNeighboursPlayerControlled();

            if (allNeighboursPlayerControlled) {
                planet.invasionActive = false;
                CancelInvoke("AddEnemyTroops");
                StopCoroutine("AddInvadingEnemyTroops");
            }

            // Set next planet to be attacked?
            if(planet.invasionActive == false)
            {
                StopCoroutine("AddInvadingEnemyTroops");
                planet.eTroopCount = 0;
            }
            planet.PlayerControlled = true;
            OngoingLosses = true;
            StopCoroutine("ComabtLosses");
        }
        
        if (planet.enemyControl == 100)
        {
            if (planet.PlayerControlled)
            {
                Debug.Log(planet.name + " has fallen");

                planet.PlayerControlled = false;

                for (int i = 0; i < planet.ConnectedWorlds.Length; i++)
                {
                    Planet current = InvasionMScript.findPlanet(planet.ConnectedWorlds[i].planetID);

                    Debug.Log(current.name + 
                        " PlayerControlled " + current.PlayerControlled +
                        " invasionActive " + current.invasionActive
                    );

                    if (current.PlayerControlled && current.invasionActive == false)
                    {
                        Debug.Log(current.name + "is next planet to be attacked");
                        current.invasionActive = true;
                        current.troopsInbound = false;
                    }
                }
            }
        }
    }

    void ResourceCreation()
    {
        if (NotUI == true)
        {
            if (planet.PlayerControlled == true)
            {
                PBM.AddResources();
            }
        }
    }

    void ManpowerCreation()
    {
        if (NotUI == true)
        {
            if (planet.PlayerControlled == true)
            {
                MM.AddManpower();
            }
        }
    }


    void EnemyTroops()
    {
        if (planet.PlayerControlled == false && InvokeOngoing == false)
        {
            InvokeRepeating("AddEnemyTroops", 1, 2);
            Debug.Log("readEnemyTroops");
            InvokeOngoing = true;
        }
        else if (planet.PlayerControlled == true && InvokeOngoing == false)
        {
            InvokeOngoing = false;
        }
    }

    void AddEnemyTroops()
    {
        if(EnemySizePicker == 1)
        {
            planet.eTroopCount = planet.eTroopCount + (50 * PBM.EnemyAgression);
        }

        else if(EnemySizePicker == 2)
        {
            planet.eTroopCount = planet.eTroopCount + (100 * PBM.EnemyAgression);
        }

        else if(EnemySizePicker == 3)
        {
            planet.eTroopCount = planet.eTroopCount + (150 * PBM.EnemyAgression);
        }
    }

    IEnumerator AddInvadingEnemyTroops()
    {

        planet.troopsInbound = true;
        yield return new WaitForSeconds(1);
        if(EnemySizePicker == 1)
        {
            planet.eTroopCount = planet.eTroopCount + (10 * PBM.EnemyAgression);
        }

        else if(EnemySizePicker == 2)
        {
            planet.eTroopCount = planet.eTroopCount + (25 * PBM.EnemyAgression);
        }

        else if(EnemySizePicker == 3)
        {
            planet.eTroopCount = planet.eTroopCount + (50 * PBM.EnemyAgression);
        }

        planet.troopsInbound = false;
    }

    void ComabtLosses() // The "CombatLosses" function will check the planets enemy and allied troop values and compare them to see how many troops each side will lose every 2 seconds during combat
    {
        Debug.Log("CombatLosses Read");

        if (planet.eTroopCount != 0 && planet.pTroopCount != 0)
        {
            if (planet.eTroopCount > 0 && planet.pTroopCount > 0 && planet.pTroopCount > planet.eTroopCount * 2) // if these troop values are at 0 they will not go down but if the allied units in this case has 100% more troops than the enemy troops, they will do Significantly more damage to the enemy troops overall.
            {
                if (EnemySizePicker == 1)
                {
                    planet.eTroopCount = planet.eTroopCount - 200;
                }

                if (EnemySizePicker == 2)
                {
                    planet.eTroopCount = planet.eTroopCount - 250;
                }

                if (EnemySizePicker == 3)
                {
                    planet.eTroopCount = planet.eTroopCount - 300;
                }
            }
            else if (planet.eTroopCount > 0 && planet.pTroopCount > 0 && planet.pTroopCount > planet.eTroopCount * 1.5) // if these troop values are at 0 they will not go down but if the allied units in this case has 50% more troops than the enemy troops, they will do much more damage to the enemy troops overall.
            {
                if (EnemySizePicker == 1)
                {
                    planet.eTroopCount = planet.eTroopCount - 75;
                }

                if (EnemySizePicker == 2)
                {
                    planet.eTroopCount = planet.eTroopCount - 100;
                }

                if (EnemySizePicker == 3)
                {
                    planet.eTroopCount = planet.eTroopCount - 125;
                }
            }
            else if (planet.eTroopCount > 0 && planet.pTroopCount > 0 && planet.pTroopCount > planet.eTroopCount * 1.2) // if these troop values are at 0 they will not go down but if the allied units in this case has 20% more troops than the enemy troops, they will do more damage to the enemy troops overall.
            {
                if (EnemySizePicker == 1)
                {
                    planet.eTroopCount = planet.eTroopCount - 25;
                }

                if (EnemySizePicker == 2)
                {
                    planet.eTroopCount = planet.eTroopCount - 50;
                }

                if (EnemySizePicker == 3)
                {
                    planet.eTroopCount = planet.eTroopCount - 75;
                }
            }
            else // if the allied troops is not 20% higher than the enemy the loses the enemy takes will decrease
            {
                if (EnemySizePicker == 1)
                {
                    planet.eTroopCount = planet.eTroopCount - 5;
                }

                if (EnemySizePicker == 2)
                {
                    planet.eTroopCount = planet.eTroopCount - 10;
                }

                if (EnemySizePicker == 3)
                {
                    planet.eTroopCount = planet.eTroopCount - 15;
                }
            }

            if (planet.pTroopCount > 0 && planet.eTroopCount > 0 && planet.eTroopCount > planet.pTroopCount * 2) // this check is the opposite to the above where instead if the enemy troops is 100% higher than the allied troops the allied troops will take Significantly more damge
            {
                if (AlliedLossPicker == 1)
                {
                    planet.pTroopCount = planet.pTroopCount - 200;
                }

                if (AlliedLossPicker == 2)
                {
                    planet.pTroopCount = planet.pTroopCount - 250;
                }

                if (AlliedLossPicker == 3)
                {
                    planet.pTroopCount = planet.pTroopCount - 300;
                }
            }
            else if (planet.pTroopCount > 0 && planet.eTroopCount > 0 && planet.eTroopCount > planet.pTroopCount * 1.5) // this check is the opposite to the above where instead if the enemy troops is 50% higher than the allied troops the allied troops will take much more damge
            {
                if (AlliedLossPicker == 1)
                {
                    planet.pTroopCount = planet.pTroopCount - 75;
                }

                if (AlliedLossPicker == 2)
                {
                    planet.pTroopCount = planet.pTroopCount - 100;
                }

                if (AlliedLossPicker == 3)
                {
                    planet.pTroopCount = planet.pTroopCount - 125;
                }
            }
            else if (planet.pTroopCount > 0 && planet.eTroopCount > 0 && planet.eTroopCount > planet.pTroopCount * 1.2) // this check is the opposite to the above where instead if the enemy troops is 20% higher than the allied troops the allied troops will take more damge
            {
                if (AlliedLossPicker == 1)
                {
                    planet.pTroopCount = planet.pTroopCount - 25;
                }

                if (AlliedLossPicker == 2)
                {
                    planet.pTroopCount = planet.pTroopCount - 50;
                }

                if (AlliedLossPicker == 3)
                {
                    planet.pTroopCount = planet.pTroopCount - 75;
                }
            }
            else // if enemy troops do not have 20% more units than the allied troops the allied troops take less losses
            {
                if (AlliedLossPicker == 1)
                {
                    planet.pTroopCount = planet.pTroopCount - 5;
                }

                if (AlliedLossPicker == 2)
                {
                    planet.pTroopCount = planet.pTroopCount - 10;
                }

                if (AlliedLossPicker == 3)
                {
                    planet.pTroopCount = planet.pTroopCount - 15;
                }
            }
        }
        OngoingLosses = true;
    }

    bool isAllNeighboursPlayerControlled()
    {
        bool allNeighboursPlayerControlled = true;

        for (int i = 0; i < planet.ConnectedWorlds.Length; i++)
        {
            Planet current = InvasionMScript.findPlanet(planet.ConnectedWorlds[i].planetID);

            if (!current.PlayerControlled) {
                allNeighboursPlayerControlled = false;
                break;
            }
        }

        return allNeighboursPlayerControlled;
    }
}
