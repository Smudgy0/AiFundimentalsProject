using UnityEngine;

public class IDManager : MonoBehaviour
{
    [SerializeField] public int PlanetID;
    [SerializeField] int PlanetControl = 100;

    [SerializeField] GameObject[] ConnectedPlanets;

    [SerializeField] bool ControlledByPlayer = true;
    [SerializeField] bool NotUI = true;

    public ManpowerManager MM;
    [SerializeField]
    private GameObject LR;

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

    }
}
