using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeController : MonoBehaviour
{
    public GameObject Sun;
    public float Ticks;
    public int GameHour;
    public int GameMinute;
    public int GameDay;
    public bool TimeFlow = true;

    void Start()
    {
        GameHour = 8;
        GameMinute=0;
        GameDay = 0;
    }

    void FixedUpdate()
    {
        if( TimeFlow ==true)
        {
            Ticks += 1;
            if (Ticks > 14)
            {
                GameMinute += 1;
                Ticks = 0;
            }
            if (GameMinute > 59)
            {
                GameMinute = 0;
                GameHour += 1;
            }
            if(GameHour> 23)
            {
                GameHour = 0;
                GameDay+= 1;
            }
        }
    }
}
