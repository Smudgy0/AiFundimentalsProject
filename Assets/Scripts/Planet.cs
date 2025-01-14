using UnityEngine;

[CreateAssetMenu(fileName = "Planets", menuName = "Planets", order = 0)]
public class Planet : ScriptableObject
{
    public int planetID;
    public string planetName;
    public int pTroopCount = 0;
    public int eTroopCount = 0;
    public float alliedControl;
    public float enemyControl;
}
