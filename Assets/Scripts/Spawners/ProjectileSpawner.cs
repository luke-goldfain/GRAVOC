using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileSpawner : PooledSpawner
{
    public GameObject Projectile;

    public override void SetupSpawnObject(GameObject go)
    {
        base.SetupSpawnObject(go);
        if (go.GetComponent<Projectile>() != null)
        {
            Projectile proj = go.GetComponent<Projectile>();
            proj.gameObject.transform.position = this.transform.position + new Vector3(0, this.transform.lossyScale.y, 0);
            //proj.SetUpProjectile();
        }
    }

    public override void SetupSpawnObject(GameObject go, GameObject caster)
    {
        //base.SetupSpawnObject(go);
        if (go.GetComponent<Projectile>() != null)
        {
            Projectile gs = go.GetComponent<Projectile>();
            gs.gameObject.transform.position = caster.transform.position + new Vector3(0, caster.transform.lossyScale.y, 0);
            //gs.SetUpWeapon(caster);

        }
    }

    // Update is called once per frame
    void Update()
    {
        base.removeObjectInListToRemove();
        addDeadProjectilesToRemoveList();
    }

    protected void addDeadProjectilesToRemoveList()
    {
        Projectile proj;
        foreach (GameObject go in this.gameObjects)
        {
            proj = go.GetComponent<Projectile>();
            if (go.GetComponent<Projectile>() != null)
            {
                if (proj._state == State.DONE)
                {
                    //remove done projectiles
                    this.objectsToRemove.Add(proj.gameObject);
                }
            }
        }
    }
}
