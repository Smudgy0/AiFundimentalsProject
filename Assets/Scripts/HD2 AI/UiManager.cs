using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;

public class UiManager : MonoBehaviour
{
    [SerializeField] GameObject PlanetUI;
    public int Manpower;
    public TMP_Text ManpowerText;
    public TMP_Text IDT;
    public TMP_Text CT;
    public TMP_Text troopCount;
    public TMP_Text EtroopCount;
    public TMP_Text alliedControl;
    public TMP_Text enemyControl;

    public int ID;

    public Planet currentlySelectedPlanet;

    public ManpowerManager MAM;
    public IDManager IDMAN;

    Vector2 mousePosition;
    Vector3 mousePositionInWorld;

    private void Start()
    {
        PlanetUI.SetActive(false);
    }

    public void MousePosition(InputAction.CallbackContext value)
    {
        mousePosition = value.ReadValue<Vector2>();
    }

    public void MouseClick(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if(context.performed)
            {
                ClickPlanet();
            }
        }
    }

    bool ClickPlanet()
    {
        Ray ray = Camera.main.ScreenPointToRay(mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            if (hit.collider != null)
            {
                if(hit.collider.TryGetComponent<PlanetIDGrabber>(out PlanetIDGrabber PIG))
                {
                    OpenUI(PIG);
                    currentlySelectedPlanet = PIG.planet;
                    return true;
                }
            }
        }
        // Check to see if over UI, if I am then keep selected and if I'm not them deselect
        // currentlySelectedPlanet = null;
        return false;
    }
    public void CloseUI()
    {
        PlanetUI.SetActive(false);
    }

    public void OpenUI(PlanetIDGrabber planetClicked)
    {
        IDT.text = planetClicked.planet.planetName.ToString();
        troopCount.text = planetClicked.planet.pTroopCount.ToString();
        EtroopCount.text = planetClicked.planet.eTroopCount.ToString();
        alliedControl.text = planetClicked.planet.alliedControl.ToString();
        enemyControl.text = planetClicked.planet.enemyControl.ToString();
        PlanetUI.SetActive(true);
    }

    void UpdateValues(PlanetIDGrabber planetClicked)
    {
        EtroopCount.text = planetClicked.planet.eTroopCount.ToString();
    }

    public void CheckManpower()
    {
        ManpowerText.SetText(MAM.Manpower.ToString());
    }

    private void Update()
    {
        if(Physics.Raycast(transform.position,Vector3.up, 50, 8))
        {
            Debug.Log("Player hit");
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(Camera.main.transform.position , Vector3.up * 50);
    }
}
