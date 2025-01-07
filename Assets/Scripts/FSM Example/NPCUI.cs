using UnityEngine;
using TMPro;

public class NPCUI : MonoBehaviour
{
    [SerializeField] TMP_Text Goods_Text;

    [SerializeField] float goodsMade = 0;

    [SerializeField] GameObject industrySmoke;
    [SerializeField ]int ActiviteWorkshops;


    public void ActiveWorkshop()
    {
        ActiviteWorkshops++;
    }
    public void InActiveWorkshop()
    {
        ActiviteWorkshops--;
    }
    public void GoodsMade()
    {
        goodsMade++;
    }

    public void GoodLost()
    {
        goodsMade--;
    }

    private void Update()
    {
        Goods_Text.SetText(goodsMade.ToString());

        if(ActiviteWorkshops > 0)
        {
            industrySmoke.SetActive(true);
        }
        else { industrySmoke.SetActive(false); }
    }
}
