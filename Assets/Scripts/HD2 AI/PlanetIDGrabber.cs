using System;
using System.Collections;
using NUnit.Framework.Constraints;
using NUnit.Framework.Internal;
using UnityEngine;

public class PlanetIDGrabber : MonoBehaviour
{
    public Planet planet;

    public int PlanetObjectIdentifyer = 0;

    [SerializeField] InvasionManager InvasionMScript;
    private UiManager UIM;
    [SerializeField] GameObject[] ConnectedPlanets;
    [SerializeField] bool ControlledByPlayer = true;
    [SerializeField] bool NotUI = true;
    public GameObject LR;
    public ManpowerManager MM;

    bool InvokeOngoing = false;
    bool InvasionActive = false;
    bool TroopsInbound = false;

    int control;

    int troopSpeedDelay = 1;

    int SiegedPlanetIDValue = 0;

    private void Awake()
    {
        UIM = FindAnyObjectByType<UiManager>();
    }

    private void Start()
    {
        InvasionActive = false;

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

        //planet = Instantiate(planet);

        for (int i = 0; i < InvasionMScript.ListOfPlanets.Length; i++)
        {
            if (PlanetObjectIdentifyer == InvasionMScript.ListOfPlanets[i].planetID)
            {
                planet = InvasionMScript.ListOfPlanets[i];
            }
        }
    }

    void Update()
    {
        EnemyTroops();
        Invasion();

        if (InvasionActive == true && TroopsInbound == false)
        {
            StartCoroutine("AddInvadingEnemyTroops");
        }
        else if (InvasionActive == false)
        {
            StopCoroutine("AddInvadingEnemyTroops");
            TroopsInbound = false;
        }
    }

    void ManpowerCreation()
    {
        if (NotUI == true)
        {
            if (ControlledByPlayer == true)
            {
                MM.AddManpower();
            }
        }
    }

    void Invasion()
    {

        for (int i = 0; i < planet.ConnectedWorlds.Length; i++)
        {
            if (planet.ConnectedWorlds[i].PlayerControlled == false)
            {
                InvasionActive = true;
            }
        }
    }

    void EnemyTroops()
    {
        if (ControlledByPlayer == false && InvokeOngoing == false)
        {
            InvokeRepeating("AddEnemyTroops", 1, 2);
            Debug.Log("readEnemyTroops");
            InvokeOngoing = true;
        }
        else if (ControlledByPlayer == true && InvokeOngoing == false)
        {
            InvokeOngoing = false;
        }
    }

    void AddEnemyTroops()
    {
        planet.eTroopCount = planet.eTroopCount + 100;
    }

    /*void AddInvadingEnemyTroops()
    {
        planet.eTroopCount = planet.eTroopCount + 25;
        Debug.Log(planet.planetName);
    }*/

    IEnumerator AddInvadingEnemyTroops()
    {
        TroopsInbound = true;
        yield return new WaitForSeconds(1);
        planet.eTroopCount = planet.eTroopCount + 25;
        Debug.Log(planet.planetName);
        TroopsInbound = false;
    }
}
