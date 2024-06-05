using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlotsController : MonoBehaviour
{
    public GameObject PlotPrefab;
    public GameObject GridPrefab;

    public int Size=0;

    #region Structures
    public GameObject FencePole;
    public GameObject Fence1;
    public GameObject Fence2;
    public GameObject Fence3;
    public GameObject Fence4;
    public GameObject Fence5;
    public GameObject FarmingPLot;
    public GameObject Road0;
    public GameObject Road1;
    public GameObject Road2;
    public GameObject Road3;
    public GameObject Road4;
    public GameObject Road5;
    public GameObject Well;
    #endregion




    // Start is called before the first frame update
    void Start()
    {
        StartingPlots(Size);
        PlotConnectingStructure(1, 0, 0, 25, 26);
        PlotConnectingStructure(1, 0, 0, 25, 25);
        PlotConnectingStructure(1, 0, 0, 25, 24);
        PlotConnectingStructure(1, 0, 0, 24, 24);
        PlotConnectingStructure(1, 0, 0, 26, 25);

    }
    public void CreateGrid(GameObject obj)
    {
        for (int i = 0; i < 50; i++)
        {
            for (int j = 0; j < 50; j++)
            {
                var temp = Instantiate(GridPrefab);
                temp.transform.parent = obj.transform;
                temp.transform.localPosition = new Vector3(-24.5f + j, 0, -24.5f + i);
                temp.name = "Grid" + "." + j + "." + i;
                temp.GetComponent<GridElement>().GridX = j;
                temp.GetComponent<GridElement>().GridZ = i;
                temp.GetComponent<MeshRenderer>().enabled = false;
            }
        }
    }
    void StartingPlots(int size)
    {
        for (int i = 0; i < 2 * size + 1; i++)
        {
            for (int j = 0; j < 2 * size + 1; j++)
            {
                var temp = Instantiate(PlotPrefab, this.transform);
                temp.transform.position = new Vector3(-size * 50 + j * 50, 0, -size * 50 + i * 50);
                temp.name = "Plot." + (-size + j) + "." + (-size + i);
                CreateGrid(temp);
            }
        }

    }
    public GameObject ReturnStructure(int ID, int type)
    {
        switch (ID, type)
        {
            case (1,0): return FencePole; break;
            case (1,1): return Fence1; break;
            case (1,2): return Fence2; break;
            case (1,3): return Fence3; break;
            case (1,4): return Fence4; break;
            case (1,5): return Fence5; break;
            case (2,0): return FarmingPLot; break;
            case (3, 0): return Road0; break;
            case (3, 1): return Road1; break;
            case (3, 2): return Road2; break;
            case (3, 3): return Road3; break;
            case (3, 4): return Road4; break;
            case (3, 5): return Road5; break;
            case (4, 0): return Well; break;
        }
        return null;
    }
    public void PlotStructure(int structureID, int size, int plotX, int plotZ, int gridX, int gridZ, int rotation)
    {
            float rotationY = 90 * rotation;
            int variant = 0;
            transform.Find("Plot." + plotX + "." + plotZ).GetComponent<Plot>().plotData.plotStructures[gridZ * 50 + gridX] = structureID;
            GameObject.Find("Grid." + gridX + "." + gridZ).GetComponent<GridElement>().ChangeStructure(structureID);
            if (size == 3)
            {
                transform.Find("Plot." + plotX + "." + plotZ).GetComponent<Plot>().plotData.plotStructures[(gridZ + 1) * 50 + gridX + 1] = structureID;
                transform.Find("Plot." + plotX + "." + plotZ).GetComponent<Plot>().plotData.plotStructures[(gridZ + 1) * 50 + gridX] = structureID;
                transform.Find("Plot." + plotX + "." + plotZ).GetComponent<Plot>().plotData.plotStructures[(gridZ + 1) * 50 + gridX - 1] = structureID;
                transform.Find("Plot." + plotX + "." + plotZ).GetComponent<Plot>().plotData.plotStructures[(gridZ) * 50 + gridX + 1] = structureID;
                transform.Find("Plot." + plotX + "." + plotZ).GetComponent<Plot>().plotData.plotStructures[(gridZ) * 50 + gridX - 1] = structureID;
                transform.Find("Plot." + plotX + "." + plotZ).GetComponent<Plot>().plotData.plotStructures[(gridZ - 1) * 50 + gridX + 1] = structureID;
                transform.Find("Plot." + plotX + "." + plotZ).GetComponent<Plot>().plotData.plotStructures[(gridZ - 1) * 50 + gridX] = structureID;
                transform.Find("Plot." + plotX + "." + plotZ).GetComponent<Plot>().plotData.plotStructures[(gridZ - 1) * 50 + gridX - 1] = structureID;

                GameObject.Find("Grid." + (gridX + 1) + "." + (gridZ + 1)).GetComponent<GridElement>().ChangeStructure(structureID);
                GameObject.Find("Grid." + (gridX + 1) + "." + gridZ).GetComponent<GridElement>().ChangeStructure(structureID);
                GameObject.Find("Grid." + (gridX + 1) + "." + (gridZ - 1)).GetComponent<GridElement>().ChangeStructure(structureID);
                GameObject.Find("Grid." + (gridX) + "." + (gridZ + 1)).GetComponent<GridElement>().ChangeStructure(structureID);
                GameObject.Find("Grid." + (gridX) + "." + (gridZ - 1)).GetComponent<GridElement>().ChangeStructure(structureID);
                GameObject.Find("Grid." + (gridX - 1) + "." + (gridZ + 1)).GetComponent<GridElement>().ChangeStructure(structureID);
                GameObject.Find("Grid." + (gridX - 1) + "." + gridZ).GetComponent<GridElement>().ChangeStructure(structureID);
                GameObject.Find("Grid." + (gridX - 1) + "." + (gridZ - 1)).GetComponent<GridElement>().ChangeStructure(structureID);

            }
            var temp = Instantiate(ReturnStructure(structureID, variant), GameObject.Find("Grid." + gridX + "." + gridZ).transform.position, Quaternion.Euler(-90, rotationY - 90, 0));
            temp.name = "Structure";
            temp.transform.position = new Vector3(temp.transform.position.x, 0.05f, temp.transform.position.z);

            temp.transform.parent = GameObject.Find("Grid." + gridX + "." + gridZ).transform;

    }
    public void PlotConnectingStructure(int structureID, int plotX, int plotZ, int gridX, int gridZ)
    {
            int[] analise = new int[4];
            analise[0] = transform.Find("Plot." + plotX + "." + plotZ).GetComponent<Plot>().plotData.plotStructures[(gridZ + 1) * 50 + gridX];
            analise[1] = transform.Find("Plot." + plotX + "." + plotZ).GetComponent<Plot>().plotData.plotStructures[gridZ * 50 + gridX + 1];
            analise[2] = transform.Find("Plot." + plotX + "." + plotZ).GetComponent<Plot>().plotData.plotStructures[(gridZ - 1) * 50 + gridX];
            analise[3] = transform.Find("Plot." + plotX + "." + plotZ).GetComponent<Plot>().plotData.plotStructures[gridZ * 50 + gridX - 1];

            float rotationY = 0;
            int variant = 0;
            for (int j = 0; j < 4; j++)
            {
                if (analise[0] == structureID && analise[3] != structureID)
                {
                    break;
                }
                rotationY += 90;
                int tempee = analise[0];
                analise[0] = analise[1];
                analise[1] = analise[2];
                analise[2] = analise[3];
                analise[3] = tempee;
            }
            //0 - nic, 1 - gora, 2, 2, 3, 4
            for (int i = 0; i < 4; i++)
            {
                if (analise[i] == structureID)
                {
                    variant += 1;
                }
            }
            if (analise[1] == structureID)
            {
                variant += 1;
            }

            transform.Find("Plot." + plotX + "." + plotZ).GetComponent<Plot>().plotData.plotStructures[gridZ * 50 + gridX] = structureID;
            GameObject.Find("Grid." + gridX + "." + gridZ).GetComponent<GridElement>().ChangeStructure(structureID);
            var temp = Instantiate(ReturnStructure(structureID, variant), GameObject.Find("Grid." + gridX + "." + gridZ).transform.position, Quaternion.Euler(-90, rotationY - 90, 0));
            temp.name = "Structure";
            temp.transform.parent = GameObject.Find("Grid." + gridX + "." + gridZ).transform;
            temp.transform.position = new Vector3(temp.transform.position.x, 0.05f, temp.transform.position.z);
            PlotConnectingStructureRefresh(structureID, plotX, plotZ, gridX, gridZ + 1);
            PlotConnectingStructureRefresh(structureID, plotX, plotZ, gridX + 1, gridZ);
            PlotConnectingStructureRefresh(structureID, plotX, plotZ, gridX, gridZ - 1);
            PlotConnectingStructureRefresh(structureID, plotX, plotZ, gridX - 1, gridZ);
        
    }
    public void PlotConnectingStructureRefresh(int structureID, int plotX, int plotZ, int gridX, int gridZ)
    {
        if(transform.Find("Plot." + plotX + "." + plotZ).GetComponent<Plot>().plotData.plotStructures[(gridZ) * 50 + gridX] == structureID)
        {
            Destroy(GameObject.Find("Plot." + plotX + "." + plotZ).transform.Find("Grid." + gridX + "." + gridZ).transform.Find("Structure").gameObject);

            int[] analise = new int[4];
            analise[0] = transform.Find("Plot." + plotX + "." + plotZ).GetComponent<Plot>().plotData.plotStructures[(gridZ + 1) * 50 + gridX];
            analise[1] = transform.Find("Plot." + plotX + "." + plotZ).GetComponent<Plot>().plotData.plotStructures[gridZ * 50 + gridX + 1];
            analise[2] = transform.Find("Plot." + plotX + "." + plotZ).GetComponent<Plot>().plotData.plotStructures[(gridZ - 1) * 50 + gridX];
            analise[3] = transform.Find("Plot." + plotX + "." + plotZ).GetComponent<Plot>().plotData.plotStructures[gridZ * 50 + gridX - 1];

            float rotationY = 0;
            int variant = 0;
            for (int j = 0; j < 4; j++)
            {
                if (analise[0] == structureID && analise[3] != structureID)
                {
                    break;
                }
                rotationY += 90;
                int tempee = analise[0];
                analise[0] = analise[1];
                analise[1] = analise[2];
                analise[2] = analise[3];
                analise[3] = tempee;
            }
            //0 - nic, 1 - gora, 2, 2, 3, 4
            for (int i = 0; i < 4; i++)
            {
                if (analise[i] == structureID)
                {
                    variant += 1;
                }
            }
            if (analise[1] == structureID)
            {
                variant += 1;
            }

            transform.Find("Plot." + plotX + "." + plotZ).GetComponent<Plot>().plotData.plotStructures[gridZ * 50 + gridX] = structureID;
            var temp = Instantiate(ReturnStructure(structureID, variant), GameObject.Find("Grid." + gridX + "." + gridZ).transform.position, Quaternion.Euler(-90, rotationY - 90, 0));
            temp.name = "Structure";
            temp.transform.parent = GameObject.Find("Grid." + gridX + "." + gridZ).transform;
            temp.transform.position = new Vector3(temp.transform.position.x, 0.05f, temp.transform.position.z);

        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }



}

public class PlotData
{
    public Vector2 coordinates { get; set; }
    public int[] plotStructures { get; set; }
}

