using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plot : MonoBehaviour
{
    public PlotData plotData = new PlotData() { plotStructures = new int[50*50] };

    void Start()
    {
        plotData.plotStructures[0] = 1;


    }

    void OnTriggerEnter(Collider collision)
    {
        if (collision.tag == "Player")
        {
            collision.GetComponent<PlayerController>().CurrentPlot = this.gameObject;
        }
    }
}
