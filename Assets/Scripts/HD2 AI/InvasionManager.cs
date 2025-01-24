using Unity.VisualScripting;
using UnityEngine;

public class InvasionManager : MonoBehaviour
{
    [SerializeField] public Planet[] ListOfPlanets;


    private void Start()
    {
        for (int i = 0; i < ListOfPlanets.Length; i++)
        {
            ListOfPlanets[i] = Instantiate(ListOfPlanets[i]);
        }
    }
}
