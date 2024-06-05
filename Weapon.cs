using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Weapon : MonoBehaviour
{

    public string name;
    public int tier;
    public float damage=50;
    public float swingSpeed;
    public float durability;
    public bool attack;

    void Start()
    {
        attack = false;
    }

    public void OnTriggerEnter(Collider collision)
    {
        if (attack==true&& collision.gameObject.tag == "Enemy")
        {
            attack=false;
            collision.gameObject.GetComponent<dummyTarget>().RecieveDamage(damage);
        }

    }



}

public class WeaponType
{
    public string name { get; set; }
    public int tier { get; set; } 
    public float damage { get; set; }
    public float swingSpeed { get; set; }
    public float durability { get; set; }

}
