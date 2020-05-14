using Assets.Scripts.Projectiles;
using Assets.Scripts.Projectiles.Types;
<<<<<<< HEAD
using System;
=======
>>>>>>> origin/master
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public Shot _shot;
    public Rigidbody rb;
    public PlayerController playerReference;
    public GameObject prefabProjectile;
    public ParticleSystem Explosion;

    void Start()
    {
        if (_shot == null)
<<<<<<< HEAD
        {   
            randomProjectile();
        }

        playerReference = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
=======
        {
            _shot = new NormalShots(this);
        }
>>>>>>> origin/master
    }

    void Update()
    {
        if (_shot != null)
        {
            _shot.Update();
        }
    }

<<<<<<< HEAD
    private void randomProjectile()
    {
        int randomInt = UnityEngine.Random.Range(0, 4);

        if (randomInt == 0)
        {
            setNormalShots();
        }
        else if (randomInt == 1)
=======
    void setNormalShots()
    {
        _shot = new NormalShots(this);
    }

    void setExplosiveShots()
    {
        if (this._shot._currentBounce >= this._shot._maxBounces)
>>>>>>> origin/master
        {
            setExplosiveShots();
        }
        else if (randomInt == 2)
        {
            setPrecisionShots();
        }
        else
        {
            setScatterShots();
        }
    }

    public void setNormalShots()
    {
        _shot = new NormalShots(this);
    }

    public void setScatterShots()
    {
<<<<<<< HEAD
        _shot = new ScatterShots(this);
    }

    public void setPrecisionShots()
    {
        _shot = new PrecisionShots(this);
    }

    public void setExplosiveShots()
    {
        _shot = new ExplosiveShots(this);
    }

    public virtual void OnCollisionEnter(Collision collision)
    {
        _shot.OnCollisionEnter(collision);
    }
=======
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
>>>>>>> origin/master
}
