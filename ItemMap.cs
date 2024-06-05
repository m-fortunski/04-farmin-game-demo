using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemMap : MonoBehaviour
{
    public BaseController baseController;
    public string ItemName;
    public int ItemTimer;
    public float ItemDurability;

    void Awake()
    {
        baseController = GameObject.Find("BaseController").GetComponent<BaseController>();
    }

    public void OnTriggerStay(Collider collider)
    {
        if(collider.gameObject.tag=="Player")
        {
            if(Vector3.Distance(this.transform.position, collider.transform.position) > 2)
            {
                var temp = new Vector3(collider.transform.position.x - this.transform.position.x, 0, collider.transform.position.z - this.transform.position.z);
                this.transform.position += temp.normalized / 128;
            }
            else
            {
                baseController.AddCoins(1);
                Destroy(this.gameObject);

            }
        }
    }

}

public class Item
{
    // 0 - nic, 1 - pa³ka, 2 - miecz, 3 - siekiera, 4 - m³otek, 5 - hoe
    public int itemType { get; set; }
    public int itemCount { get; set; }
    public string name;
    public int itemTier;
    public float itemDurability;
    public float itemDamage;
    public int inventorySlot;
}
