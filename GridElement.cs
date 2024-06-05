using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridElement : MonoBehaviour
{
    //Basic
    public int StructureID = 0;
    public int GridX;
    public int GridZ;

    //Advanced
    public int StructureLevel;
    public float StructureHP;
    public float StructureProgress=50;
    public float WaterLevel;

    public void ChangeStructure(int id)
    {
        StructureID = id;
    }

    public void PLant()
    {




    }
}
