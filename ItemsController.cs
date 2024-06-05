using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemsController : MonoBehaviour
{
    public GameObject Coin;
    public int CoinDuration=1920;
    float deltaHeight=0;
    float ItemHeight = 1.25f;

    public int temp=0;

    public void AddCoins(int count, Vector3 position)
    {
        for(int i = 0; i < count; i++)
        {
            var temp=Instantiate(Coin, new Vector3(position.x+(float)Random.Range(-15,16)/10, 1.5f, position.z + (float)Random.Range(-15, 16) / 10), Quaternion.Euler(90, 0, 0), GameObject.Find("Items").transform);
            temp.GetComponent<ItemMap>().ItemTimer = CoinDuration;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        temp += 1;
        if (temp > 500)
        {
            temp = 0;
            AddCoins(1, new Vector3(Random.Range(-25, 26), 0, Random.Range(-25, 26)));
        }
        if (GameObject.Find("Items").transform.childCount > 0)
        {
            foreach (Transform child in GameObject.Find("Items").transform)
            {
                child.transform.rotation *= Quaternion.Euler(0, 0, 0.75f);
                child.transform.position += new Vector3(0, deltaHeight/ 60, 0);
                child.gameObject.GetComponent<ItemMap>().ItemTimer -= 1;
                deltaHeight = Mathf.Sin(2*Mathf.PI*(child.gameObject.GetComponent<ItemMap>().ItemTimer%100)/99);
                if (child.GetComponent<ItemMap>().ItemTimer < 1)
                {
                    Destroy(child.gameObject);
                }
            }
        }
    }
}
