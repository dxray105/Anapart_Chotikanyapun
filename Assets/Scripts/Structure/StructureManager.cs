using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

 

public class StructureManager : MonoBehaviour
{
    [SerializeField] private bool isConstructing;

 

    public GameObject curBuildingPrefab;
    public GameObject buildingParent;

 

    [SerializeField] private Vector3 cursorPos;
    [SerializeField] private GameObject buildingCursor;
    [SerializeField] private GameObject gridPlane;

 

    private GameObject ghostBuilding;

 

    //Working at any farm
    [SerializeField] private GameObject curStructure;
    public GameObject CurStructure { get { return curStructure; } set { curStructure = value; } }

 

    //Demolish
    [SerializeField] private bool isDemolishing;
    public GameObject demolishCursor;

 

    private Camera cam;

 

    void Awake()
    {
        cam = Camera.main;
    }

 

    // Update is called once per frame
    void Update()
    {
        cursorPos = Selector.instance.GetCurTilePosition();

 

        if (isConstructing)
        {
            buildingCursor.transform.position = cursorPos;
            gridPlane.SetActive(true);
        }
        else if (isDemolishing)
        {
            demolishCursor.transform.position = cursorPos;
            gridPlane.SetActive(true);
        }
        else
        {
            gridPlane.SetActive(false);
        }

 

        //Left Click in different mode
        if (Input.GetMouseButtonDown(0))
        {
            if (isConstructing) //construct mode
                PlaceBuilding();
            else if (isDemolishing) //demolish mode
                Demolish();
            else
                CheckLeftClick(Input.mousePosition); //normal mode
        }

 

        //Cancel Building/Demolishing
        if (Input.GetMouseButtonDown(1) || Input.GetKeyDown(KeyCode.Escape))
        {
            CancelConstruction();

 

            if (isDemolishing)
            {
                ToggleDemolish();
            }
        }
    }

 

    public void BeginNewBuildingPlacement(GameObject prefab) //map w button
    {
        isConstructing = true;

 

        curBuildingPrefab = prefab;

 

        ghostBuilding = Instantiate(curBuildingPrefab, cursorPos, Quaternion.identity);
        ghostBuilding.GetComponent<FindBuildingSite>().plane.SetActive(true);

 

        buildingCursor = ghostBuilding;
        buildingCursor.SetActive(true);
    }

 

    private bool CheckMoney(GameObject obj)
    {
        int cost = obj.GetComponent<Structure>().costToBuild;

 

        if (cost <= GameManager.instance.money)
            return true;
        else
            return false;
    }

 

    private void DeductMoney(int cost)
    {
        GameManager.instance.money -= cost;
        UI.instance.UpdateHeaderPanel();
    }

 

    private void PlaceBuilding()
    {
        if (CheckMoney(curBuildingPrefab) == false)
            return;

 

        if (buildingCursor.GetComponent<FindBuildingSite>().CanBuild == false)
            return;

 

        GameObject structureObj = Instantiate(curBuildingPrefab, cursorPos, 
                                            Quaternion.identity, buildingParent.transform);

 

        Structure s = structureObj.GetComponent<Structure>();
        GameManager.instance.structures.Add(s);
        DeductMoney(s.costToBuild);

 

        if (!Input.GetKey(KeyCode.LeftShift))
            CancelConstruction(); //Building construction done
    }

 

    private void CancelConstruction()
    {
        isConstructing = false;
        gridPlane.SetActive(false);

 

        if (buildingCursor != null)
            buildingCursor.SetActive(false);

 

        if (ghostBuilding != null)
            Destroy(ghostBuilding);
    }

 

    public void ToggleDemolish() //map w Demolish button
    {
        isConstructing = false;
        isDemolishing = !isDemolishing;

 

        gridPlane.SetActive(isDemolishing);

 

        demolishCursor.SetActive(isDemolishing);
        demolishCursor.transform.position = new Vector3(0f, -99f, 0f);
    }

 

    public void Demolish()
    {
        GameObject colliding = demolishCursor.GetComponent<Demolish>().Colliding;

 

        if (colliding == null)
            return;

 

        Structure s = GameManager.instance.structures.Find(x => 
                                                        x.transform.position == colliding.transform.position);

 

        if (s != null)
        {
            GameManager.instance.structures.Remove(s);
            Destroy(s.gameObject);
        }

 

        UI.instance.UpdateHeaderPanel();
    }

 

    public void CheckLeftClick(Vector2 mousePos)
    {
        Ray ray = cam.ScreenPointToRay(mousePos);
        RaycastHit hit;

 

        // if we left click something
        if (Physics.Raycast(ray, out hit, 1000))
        {
            // Mouse over UI
            if (EventSystem.current.IsPointerOverGameObject())
            {
                return;
            }

 

            curStructure = hit.collider.gameObject;
            switch (hit.collider.tag)
            {
                case "Farm":
                    Structure s = CurStructure.GetComponent<Structure>();
                    //Open Farm Panel
                    UI.instance.ToggleFarmPanel(true);
                    break;
            }
        }
    }

 

    public void CallStaff()
    {
        GameManager.instance.SendStaff(CurStructure);
    }
}