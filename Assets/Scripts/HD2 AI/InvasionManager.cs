using Unity.VisualScripting;
using UnityEngine;

public class InvasionManager : MonoBehaviour
{
    [SerializeField] public Planet[] ListOfPlanets;

    public bool isReady = false;


    private void Start()
    {
    }

    public void SetupPlanets()
    {
        if (isReady) {
            return;
        }

        // Clone all primary planets
        for (int i = 0; i < ListOfPlanets.Length; i++)
        {
            ListOfPlanets[i] = Instantiate(ListOfPlanets[i]);
        }

        // Ensure the connected planets also use the same clones
        for (int i = 0; i < ListOfPlanets.Length; i++)
        {
            Planet current = ListOfPlanets[i];
            Debug.Log("Current " + current.name + " has " + current.ConnectedWorlds.Length + " linked worlds");

            for (int x = 0; x < current.ConnectedWorlds.Length; x++)
            {
                Planet currentConnected = current.ConnectedWorlds[x];
                Debug.Log(">>>> " + currentConnected.name);

                Planet matchingClosed = findPlanet(currentConnected.planetID);

                if (matchingClosed)
                {
                    current.ConnectedWorlds[x] = matchingClosed;                
                    Debug.Log(">>>> Matched " + current.ConnectedWorlds[x].name);
                }
                
            }
        }

        isReady = true;
    }

    public Planet findPlanet(int planetID)
    {
         for (int i = 0; i < ListOfPlanets.Length; i++)
         {
            if (planetID == ListOfPlanets[i].planetID)
            {
                return ListOfPlanets[i];
            }
         }

         return null;
    }
}
