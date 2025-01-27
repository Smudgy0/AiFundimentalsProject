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
    private UiManager UIM;
    [SerializeField] GameObject[] ConnectedPlanets;
    [SerializeField] bool NotUI = true;
    public GameObject LR;
    public ManpowerManager MM;

    bool InvokeOngoing = false;
    bool OngoingLosses = false;
    int EnemySizePicker;

    float Troopdif;

    bool allNeighboursPlayerControlled = true;

    private void Awake()
    {
        UIM = FindAnyObjectByType<UiManager>();
    }

    private void Start()
    {

        if (NotUI == true)
        {
            InvokeRepeating("ManpowerCreation", 1, 1);
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

        /* if (planet.name == "Ezec(Clone)") {
            Debug.Log("Invade at start planet " + planet.name);
            planet.invasionActive = true;
            planet.troopsInbound = false;
        }
        */

        Debug.Log("on start " + planet.name + 
            " troopsInbound " + planet.troopsInbound +
            " invasionActive " + planet.invasionActive
        );
    }

    void Update()
    {
        EnemyTroops();
        LiveCombat();

        EnemySizePicker = Random.Range(0, 4);

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
            Troopdif = Troopdif / 1000000;
            planet.alliedControl = planet.alliedControl - Troopdif;
            planet.enemyControl = planet.enemyControl + Troopdif;

            if (OngoingLosses == false)
            {
                OngoingLosses = true;
                StartCoroutine("ComabtLosses");
            }
        }

        if (planet.pTroopCount > planet.eTroopCount)
        {
            Troopdif = planet.pTroopCount - planet.eTroopCount;
            Troopdif = Troopdif / 1000000;
            planet.enemyControl = planet.enemyControl - Troopdif;
            planet.alliedControl = planet.alliedControl + Troopdif;

            if (OngoingLosses == false)
            {
                OngoingLosses = true;
                StartCoroutine("ComabtLosses");
            }
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
            OngoingLosses = true;
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

            // Check whether any neighboring planets are under enemy control
            for (int i = 0; i < planet.ConnectedWorlds.Length; i++)
            {
                Planet current = InvasionMScript.findPlanet(planet.ConnectedWorlds[i].planetID);

                if (!current.PlayerControlled) {
                    allNeighboursPlayerControlled = false;
                    planet.invasionActive = true;
                    break;
                }
            }

            if (allNeighboursPlayerControlled) {
                planet.invasionActive = false;
            }

            // Set next planet to be attacked?
            if(planet.invasionActive == false)
            {
                CancelInvoke("AddEnemyTroops");
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
            planet.eTroopCount = planet.eTroopCount + 50;
        }

        else if(EnemySizePicker == 2)
        {
            planet.eTroopCount = planet.eTroopCount + 100;
        }

        else if(EnemySizePicker == 3)
        {
            planet.eTroopCount = planet.eTroopCount + 150;
        }
    }

    IEnumerator AddInvadingEnemyTroops()
    {

        planet.troopsInbound = true;
        yield return new WaitForSeconds(1);
        if(EnemySizePicker == 1)
        {
            planet.eTroopCount = planet.eTroopCount + 10;
        }

        else if(EnemySizePicker == 2)
        {
            planet.eTroopCount = planet.eTroopCount + 25;
        }

        else if(EnemySizePicker == 3)
        {
            planet.eTroopCount = planet.eTroopCount + 50;
        }

        planet.troopsInbound = false;
    }

    IEnumerator ComabtLosses()
    {
        yield return new WaitForSeconds(5);
        planet.eTroopCount -= 100;
        planet.pTroopCount -= 100;
    }
}
