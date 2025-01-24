using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;
using System.Collections;

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
                    currentlySelectedPlanet = PIG.planet;
                    OpenUI();

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
        //StopCoroutine("UpdateTroopValue");
        PlanetUI.SetActive(false);
        currentlySelectedPlanet = null;
    }

    public void OpenUI()
    {
        //StopCoroutine("UpdateTroopValue");
        IDT.text = currentlySelectedPlanet.planetName.ToString();
        troopCount.text = currentlySelectedPlanet.pTroopCount.ToString();
        EtroopCount.text = currentlySelectedPlanet.eTroopCount.ToString();
        alliedControl.text = currentlySelectedPlanet.alliedControl.ToString();
        enemyControl.text = currentlySelectedPlanet.enemyControl.ToString();

        PlanetUI.SetActive(true);
    }

    public void UpdatePlanetPanel()
    {
        if (!currentlySelectedPlanet) {
            return;
        }

        // Debug.Log("Updating text");

        EtroopCount.text = currentlySelectedPlanet.eTroopCount.ToString();
        alliedControl.text = currentlySelectedPlanet.alliedControl.ToString();
        enemyControl.text = currentlySelectedPlanet.enemyControl.ToString();
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

         UpdatePlanetPanel();
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(Camera.main.transform.position , Vector3.up * 50);
    }
}
