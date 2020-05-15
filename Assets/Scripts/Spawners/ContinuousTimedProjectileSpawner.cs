using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContinuousTimedProjectileSpawner : TimedSpawner
{
    //Hack to figure out if a projectile is colliding with the spawner
    private bool holdingProjectile;
    public bool HoldingProjectile
    {
        get
        {
            return this.holdingProjectile;
        }
        set
        {
            if(this.holdingProjectile != value)
            {
                this.holdingProjectile = value;
            }
        }
    }
    private void Update()
    {
        base.addDeadProjectilesToRemoveList();
        base.removeObjectInListToRemove();


        ContinuousSpawn();
       
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Projectile")
        {
            this.HoldingProjectile = true;
            Debug.Log(HoldingProjectile);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if(collision.gameObject.tag == "Projectile")
        {
            this.HoldingProjectile = false;
            Debug.Log(HoldingProjectile);
        }
    }

    protected virtual void ContinuousSpawn()
    {
        if (HoldingProjectile == false)
        {
            lastSpawnTime += Time.deltaTime;
            if (lastSpawnTime > SpawnTime)
            {
                lastSpawnTime = 0.0f;
                this.Spawn(this.gameObject);
                this.HoldingProjectile = true;
            }
        }
    }
}
