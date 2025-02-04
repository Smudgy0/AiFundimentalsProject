using System.Collections;
using NUnit.Framework.Constraints;
using NUnit.Framework.Internal;
using Unity.VisualScripting;
using UnityEngine;

public class PlanetManager : MonoBehaviour
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

    bool allNeighboursPlayerControlled = true; // this bool value will be for checking if neighbouring planets are controlled by an enemy to tell the ai it can be attacked

    private void Awake()
    {
        UIM = FindAnyObjectByType<UiManager>(); //gets the UI Manager
    }

    private void Start()
    {
        InvokeRepeating("ComabtLosses", 2, 2); /// triggers the combat loses Function
        if (NotUI == true) //checks if the objects "NotUI" value is true and if its true it will trigger a seperate function for each object
        {
            InvokeRepeating("ManpowerCreation", 1, 1);
            InvokeRepeating("ResourceCreation", 1, 1);
            for (int i = 0; i < ConnectedPlanets.Length; i++) // checks every worlds
            {
                GameObject LineClone = Instantiate(LR, Vector3.zero, Quaternion.identity);
                LineRenderer lr = LineClone.GetComponent<LineRenderer>();
                lr.SetPosition(0, transform.position);
                lr.SetPosition(1, ConnectedPlanets[i].transform.position);
            }
        }

        InvasionMScript.SetupPlanets();

        for (int i = 0; i < InvasionMScript.ListOfPlanets.Length; i++) // a check to ensure that the array has the correct planet ID's
        {
            if (PlanetObjectIdentifyer == InvasionMScript.ListOfPlanets[i].planetID)
            {
                planet = InvasionMScript.ListOfPlanets[i];
                Debug.Log("Got planet " + planet.name);
            }
        }

        planet.invasionActive = false; // the bool to determine if an enemy invasion is ongoing
        planet.troopsInbound = false; // a bool to ensure the function doesn't run more than once

        bool allNeighboursPlayerControlled = this.isAllNeighboursPlayerControlled();

        if (!allNeighboursPlayerControlled) { // this if statement will use the function above to check if all planets neigbouring the planet running this script is allied controlled or not, if its not then an invasion will occur
            planet.invasionActive = true;
        }

        Debug.Log("on start " + planet.name + 
            " troopsInbound " + planet.troopsInbound +
            " invasionActive " + planet.invasionActive
        );
    }

    void Update()
    {
        if (!allNeighboursPlayerControlled) // allways checks if the "allNeighboursPlayerControlled" value is false so the ai can start an invasion
        {
            planet.invasionActive = true;
        }

        if (planet.eTroopCount < 0)
        {
            planet.eTroopCount = 0;
        }
        if (planet.pTroopCount < 0)
        {
            planet.pTroopCount = 0;
        }
        // these if statements prevents the troops on the planet on either side from going below 0

        EnemyTroops(); // runs the function which checks if the ai can invade the world and if they can "invasionActive" is set to true
        LiveCombat(); // runs the function for planet combat

        EnemySizePicker = Random.Range(0, 4);
        AlliedLossPicker = Random.Range(0, 4);

        if (planet.invasionActive == true && planet.troopsInbound == false) // checks if the ai can send troops to the world
        {
            Debug.Log("Adding troops" + planet.name);
            StartCoroutine("AddInvadingEnemyTroops");
        }
        else if (planet.invasionActive == false) // if not it canceles any invading troops to the planet to ensure that if a planet is cut off for the ai they can't send any troops to it from another.
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

        /* These calculations above check the difference between the ai's troops and the player troops on any given world and
         * calculates the % value the planets control will shift, if the ai has more the control % will shift in their favour
         * otherwise the control % will shift in the favour of the player.
        */ 

        if (planet.alliedControl > 100) // prevents player control going above 100
        {
            planet.alliedControl = 100;
        }

        else if (planet.alliedControl < 0) // prevents player control going below 0
        {
            planet.alliedControl = 0;
        }

        if (planet.enemyControl > 100) // prevents enemy control going above 100 and disabling invading forces in prepareation for the ai's factories on the planet to start working
        {
            planet.enemyControl = 100;
            StopCoroutine("AddInvadingEnemyTroops");
            StopCoroutine("ComabtLosses");
        }

        else if (planet.enemyControl < 0) // prevents enemy control going below 0
        {
            planet.enemyControl = 0;
        }

        if (planet.alliedControl == 100) // this if statement checks if the player control is 100%.
        {
            if (!planet.PlayerControlled) // this if statement checks if the planet was not player controlled before sending a message in the debug console to say its been taken
            {
                Debug.Log(planet.name + " has been taken");
            }


            bool allNeighboursPlayerControlled = this.isAllNeighboursPlayerControlled(); // runs the bool function to check if its neighbours are under allied control or not

            if (allNeighboursPlayerControlled) { // if it finds that all neighbours are controlled by the player the ai will stop its attacks on the planet
                planet.invasionActive = false;
                CancelInvoke("AddEnemyTroops");
                StopCoroutine("AddInvadingEnemyTroops");
            }

            if(planet.invasionActive == false) // this if statement ensures that if the planet is unable to be invaded by checking if the invasionActive bool is false
            {
                StopCoroutine("AddInvadingEnemyTroops");
                planet.eTroopCount = 0;
            }
            planet.PlayerControlled = true; // sets the planet to be controlled by the player
            OngoingLosses = true;
            StopCoroutine("ComabtLosses");
        }
        
        if (planet.enemyControl == 100) // if the game detects the ai has seized full controll of the planet it will set the planets player control bool to false and then it checks its neighbours to write in the debug console that a attack is incoming for that planet
        {
            if (planet.PlayerControlled) // checks if the planet was player controlled before as the player may have taken 10% from the ai and the ai took it back.
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

    void ResourceCreation() // this function runs the resoureManager script to add resources for the player
    {
        if (NotUI == true)
        {
            if (planet.PlayerControlled == true)
            {
                PBM.AddResources();
            }
        }
    }

    void ManpowerCreation() // this function runs the manpowerManager script to add manpower for the player
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
        if (planet.PlayerControlled == false && InvokeOngoing == false) // this if statment checks if the planet is not player controll and if its not the ai will start focusing its army their
        {
            InvokeRepeating("AddEnemyTroops", 1, 2);
            Debug.Log("readEnemyTroops");
            InvokeOngoing = true;
        }
        else if (planet.PlayerControlled == true && InvokeOngoing == false) // this else if statement checks if the planet is controlled by the player and disables the invokeOngoing bool to allow the planet to trigger another invasion by the ai if needed.
        {
            InvokeOngoing = false;
        }
    }

    void AddEnemyTroops() // this function runs if a planet is fully controlled by the ai meaning they can send more troops to its defense compared to sending troops to an invasion, this value is also effected by a difficulty modifer
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

    IEnumerator AddInvadingEnemyTroops() // this function runs if there is an invasion ongoing on the planet where the ai sends less troops to it due to the lack of control on the planet, this also is effected by a difficulty modifer
    {

        planet.troopsInbound = true; // this bool prevents this function from triggering more than once
        yield return new WaitForSeconds(1); // waits 1 second until the enemy ai adds troops.
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

    bool isAllNeighboursPlayerControlled() // this function will go through the array starting with setting the value of "allNeighboursPlayerControlled" to true and then running through the array and if any planet is not player control the ai will attack the planet running this script
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
