using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : MonoBehaviour
{
    public GameObject dummyPrefab;
    int timer = 1000;
    int difficulty=0;

    void SpawnDummy(int count)
    {
        for(int i = 0; i < count; ++i)
        {
            Instantiate(dummyPrefab, new Vector3(Random.Range(-25, 26), 0, Random.Range(-25, 26)), Quaternion.Euler(0,0,0));
        }
    }


    // Start is called before the first frame update
    void Start()
    {


    }

    // Update is called once per frame
    void FixedUpdate()
    {
        timer += 1;
        if (timer > 1000)
        {
            difficulty += 2;
            timer = 0;
            //SpawnDummy(5+difficulty);
        }
    }
}
