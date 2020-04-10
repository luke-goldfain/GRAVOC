using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravGun : MonoBehaviour
{
    public bool collide = false;
    //public virtual void OnCollisionStay(Collision collision)
    //{
    //    if(collision.gameObject.tag == "Projectile")
    //    {
    //        this.Hit(collision.gameObject);
    //        collide = true;
    //    }
    //}

    public virtual void OnTriggerStay(Collider collision)
    {
        Debug.Log("OnTriggerEnter");
        if(collision.gameObject.tag == "Projectile")
        {
            this.Hit(collision.gameObject);
            collide = true;
        }
    }

    public virtual void OnTriggerExit(Collider collision)
    {
        if(collision.gameObject.tag == "Projectile")
        {
            collide = false;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public virtual void Hit(GameObject gameObject)
    {
        Debug.Log("Crosshair has crossed  the projetile");
    }

    public void PickingUp()
    {
        
    }
}
