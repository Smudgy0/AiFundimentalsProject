using UnityEngine;

public class BackGroundMapManager : MonoBehaviour
{
    public InvasionManager IM;

    [SerializeField] GameObject[] BackPannels;
    [SerializeField] Material RedMaterial;
    [SerializeField] Material BlueMaterial;

    private void Update()
    {
        for (int i = 0; i < IM.ListOfPlanets.Length; i++)
        {
            for (int x = 0; x < BackPannels.Length; x++)
            {
                if (IM.ListOfPlanets[x].PlayerControlled == false)
                {
                    BackPannels[x].gameObject.GetComponent<MeshRenderer> ().material = RedMaterial;
                }
                else
                {
                    BackPannels[x].gameObject.GetComponent<MeshRenderer>().material = BlueMaterial;
                }
            }
        }
    }


}
