using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour
{
    #region Controllers
    public PlayerController playerController;
    public PlotsController plotsController;
    public BaseController baseController;
    public UiController UI;
    #endregion

    #region Grid
    public Material ActiveGridMaterial;
    public Material PlaceholderMaterial;
    public Material ImpossibleMaterial;
    public GameObject[] GridPrev = new GameObject[9];
    public int GridX;
    public int GridZ;
    public LayerMask Map;
    public Camera camera;
    #endregion

    #region Building
    public int SelectedStructureId;
    public GameObject StructurePlaceholder;
    public bool Placable;
    public bool Build;
    public int StructureSize;
    public float BuildDistance = 6;
    #endregion

    void OnEnable()
    {

        SelectedStructureId = 1;
        playerController = this.gameObject.GetComponent<PlayerController>();
        plotsController = GameObject.Find("PlotsController").GetComponent<PlotsController>();
        SelectStructureToConstruct(SelectedStructureId);
        Placable = false;
        camera = GameObject.Find("PlayerCamera").GetComponent<Camera>();
        baseController = GameObject.Find("BaseController").GetComponent<BaseController>();
        UI= GameObject.Find("UI").GetComponent<UiController>();
    }

    void OnDisable()
    {
        Destroy(StructurePlaceholder);
    }

    public void SelectStructureToConstruct(int id)
    {
            SelectedStructureId = id;
            Destroy(StructurePlaceholder); StructurePlaceholder = null;
            StructurePlaceholder = Instantiate(ReturnStructure(id));
            StructurePlaceholder.gameObject.name = "StructurePlaceholder";
            StructurePlaceholder.GetComponent<MeshCollider>().enabled = false;
            StructurePlaceholder.GetComponent<MeshRenderer>().material = PlaceholderMaterial;
    }

    GameObject ReturnStructure(int id)
    {
        switch (id)
        {
            case 1: StructureSize=1; return plotsController.ReturnStructure(1,0); break;
            case 2: StructureSize = 1; return plotsController.ReturnStructure(2,0); break;
            case 3: StructureSize = 1; return plotsController.ReturnStructure(3, 0); break;
            case 4: StructureSize = 3; return plotsController.ReturnStructure(4, 0); break;
        }
        return null;
        
    }
    int StructurePrice(int id)
    {
        switch (id)
        {
            case 1: StructureSize = 1; return 15;
            case 2: StructureSize = 1; return 10;
            case 3: StructureSize = 1; return 15;
            case 4: StructureSize = 3; return 50;
        }
        return 0;

    }

    public void PlaceStructure(int plotX, int plotZ)
    {
        if(plotsController.transform.Find("Plot." + plotX + "." + plotZ).GetComponent<Plot>().plotData.plotStructures[(GridZ) * 50 + GridX] == 0)
        {
            if (baseController.Coins >= StructurePrice(SelectedStructureId))
            {
                if ((SelectedStructureId == 1 || SelectedStructureId == 3) && Placable == true)
                {
                    plotsController.PlotConnectingStructure(SelectedStructureId, plotX, plotZ, GridX, GridZ);
                }
                else if (SelectedStructureId > 1 && Placable == true)
                {
                    plotsController.PlotStructure(SelectedStructureId, StructureSize, plotX, plotZ, GridX, GridZ, 0);
                }
                baseController.Coins -= StructurePrice(SelectedStructureId);
            }
            else
            {
                UI.NotEnoughCoins();
            }
        }
    }

    public void Grid(GameObject obj)
    {
        if(Vector3.Distance(this.transform.position, obj.transform.position) < BuildDistance)
        {
            obj.GetComponent<MeshRenderer>().enabled = true;
            StructurePlaceholder.transform.position = obj.transform.position;
            GridX = obj.GetComponent<GridElement>().GridX;
            GridZ = obj.GetComponent<GridElement>().GridZ;
            if (StructureSize == 1)
            {
                if (GridPrev[0] != null && GridPrev[0] != obj)
                {
                    GridPrev[0].GetComponent<MeshRenderer>().enabled = false;

                    if (GridPrev[1] != null)
                    {
                        for (int i = 0; i < 8; i++)
                        {
                            GridPrev[i + 1].GetComponent<MeshRenderer>().enabled = false;
                        }

                    }
                }


                if (obj.GetComponent<GridElement>().StructureID == 0)
                {
                    obj.GetComponent<MeshRenderer>().material = ActiveGridMaterial;
                    Placable = true;
                }
                else
                {
                    obj.GetComponent<MeshRenderer>().material = ImpossibleMaterial;
                    Placable = false;
                }
                GridPrev[0] = obj;
            }
            else
            {
                if (GridPrev[0] != null && GridPrev[0] != obj)
                {
                    for (int i = 0; i < 9; i++)
                    {
                        if (GridPrev[i] != null)
                        {
                            Debug.Log("Off");
                            GridPrev[i].GetComponent<MeshRenderer>().enabled = false;
                        }
                    }
                    for (int k = 0; k < 8; k++)
                    {
                        GridPrev[k + 1] = null;
                    }
                }
                GridPrev[0] = obj;
                GridPrev[1] = obj.transform.parent.Find("Grid." + (GridX - 1) + "." + (GridZ - 1)).gameObject;
                GridPrev[2] = obj.transform.parent.Find("Grid." + (GridX - 1) + "." + (GridZ)).gameObject;
                GridPrev[3] = obj.transform.parent.Find("Grid." + (GridX - 1) + "." + (GridZ + 1)).gameObject;
                GridPrev[4] = obj.transform.parent.Find("Grid." + (GridX) + "." + (GridZ + 1)).gameObject;
                GridPrev[5] = obj.transform.parent.Find("Grid." + (GridX) + "." + (GridZ - 1)).gameObject;
                GridPrev[6] = obj.transform.parent.Find("Grid." + (GridX + 1) + "." + (GridZ + 1)).gameObject;
                GridPrev[7] = obj.transform.parent.Find("Grid." + (GridX + 1) + "." + (GridZ)).gameObject;
                GridPrev[8] = obj.transform.parent.Find("Grid." + (GridX + 1) + "." + (GridZ - 1)).gameObject;
                bool placable = true;
                for (int i = 0; i < 9; i++)
                {
                    if (GridPrev[i] != null)
                    {
                        GridPrev[i].GetComponent<MeshRenderer>().enabled = true;
                    }
                }
                for (int i = 0; i < 9; i++)
                {
                    if (GridPrev[i].GetComponent<GridElement>().StructureID != 0)
                    {
                        placable = false; break;
                    }
                }
                if (placable == true)
                {
                    for (int i = 0; i < 9; i++)
                    {
                        GridPrev[i].GetComponent<MeshRenderer>().material = ActiveGridMaterial;
                        Placable = true;
                    }
                }
                else
                {
                    for (int i = 0; i < 9; i++)
                    {
                        GridPrev[i].GetComponent<MeshRenderer>().material = ImpossibleMaterial;
                        Placable = false;
                    }
                }
            }
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = 10f;
        mousePos = camera.ScreenToWorldPoint(mousePos);


        Ray ray = camera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 1000f, Map))
        {
            Grid(hit.transform.gameObject);
        }
    }
}
