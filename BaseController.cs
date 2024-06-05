using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseController : MonoBehaviour
{
    public int Coins;
    public int SciencePoints;

    public GameObject GridPrefab;

    public void AddCoins(int number)
    {
        Coins += number;
    }

    // Start is called before the first frame update
    void Start()
    {
        Coins = 10000;
        SciencePoints = 0;

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
