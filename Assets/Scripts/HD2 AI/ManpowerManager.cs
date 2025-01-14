using UnityEngine;

public class ManpowerManager : MonoBehaviour
{
    public int Manpower;

    public UiManager UM;

    public void AddManpower()
    {
        Manpower = Manpower + 5;
    }

    private void Update()
    {
        UM.CheckManpower();
    }

}
