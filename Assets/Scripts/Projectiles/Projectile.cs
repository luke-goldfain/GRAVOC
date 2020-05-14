using Assets.Scripts.Projectiles;
using Assets.Scripts.Projectiles.Types;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public Shot _shot;
    public Rigidbody rb;
    public PlayerController playerReference;

    void Start()
    {
        if (_shot == null)
        {
            _shot = new NormalShots(this);
        }
    }

    void Update()
    {
        if (_shot != null)
        {
            _shot.Update();
        }
    }

    void setNormalShots()
    {
        _shot = new NormalShots(this);
    }

    void setExplosiveShots()
    {
        if (this._shot._currentBounce >= this._shot._maxBounces)
        {
            this.gameObject.SetActive(false);
            this.GetComponent<SphereCollider>().enabled = false;
        }
    }

    public void Held()
    {
        this.transform.position = Vector3.Lerp(this.transform.position, this.transform.parent.transform.position, 0.2f);
    }

    //Here is what will be called when a player has interacted and picked up the projectile
    public void PickingUp(Transform targetTransform)
    {
        this.rb.isKinematic = true;
        this.rb.detectCollisions = false;


        this.transform.parent = targetTransform.transform;

        // Assign player reference when picked up, used to determine which player this should hurt when shot
        this.playerReference = targetTransform.transform.root.GetComponent<PlayerController>();
        
        this.transform.position = Vector3.Lerp(this.transform.position, targetTransform.transform.position, 0.2f);

        this._shot._state = State.HELD; 
    }

    void setScatterShots()
    {
        _shot = new ScatterShots(this);
    }

    public virtual void OnCollisionEnter(Collision collision)
    {
        _shot.OnCollisionEnter(collision);
    }
}
