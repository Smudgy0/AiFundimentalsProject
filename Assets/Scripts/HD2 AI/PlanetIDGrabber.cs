using UnityEngine;

public class PlanetIDGrabber : MonoBehaviour
{
    public Planet planet;
    private UiManager UIM;
    [SerializeField] GameObject[] ConnectedPlanets;
    [SerializeField] bool ControlledByPlayer = true;
    [SerializeField] bool NotUI = true;
    public GameObject LR;
    public ManpowerManager MM;

    bool InvokeOngoing = false;

    int control;

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
                lr.SetPosition(1, ConnectedPlanets[0].transform.position);
            }
        }

        planet = Instantiate(planet);
    }

    private void Update()
    {
        EnemyTroops();
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
        planet.eTroopCount = planet.eTroopCount + 10;
    }
}
