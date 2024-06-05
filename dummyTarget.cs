using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dummyTarget : MonoBehaviour
{
    public float HP;
    public GameObject target;
    public SceneController sceneController;

    void Start()
    {
        HP = 100;
        target = GameObject.Find("Player");
        sceneController = GameObject.Find("SceneController").GetComponent<SceneController>();
    }

    void ChangeColor()
    {
        this.gameObject.GetComponent<Renderer>().material.SetColor("_Color", Color.blue);

    }

    public void RecieveDamage(float damage)
    {
        HP-=damage;

        if(HP <= 0 ) 
        {
            Destroy(this.gameObject);
        }
        ChangeColor();
        this.transform.position+= new Vector3(this.transform.position.x - target.transform.position.x, 0, this.transform.position.z - target.transform.position.z);

    }

    public void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<PlayerController>().LooseHP(10, this.gameObject);
        }

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (sceneController.TimeFlow == true)
        {
            Vector3 deltaPos = new Vector3(this.transform.position.x - target.transform.position.x, 0, this.transform.position.z - target.transform.position.z) / 128;
            this.transform.rotation = Quaternion.LookRotation(deltaPos);
            this.transform.position -= deltaPos;
        }
    }
}
