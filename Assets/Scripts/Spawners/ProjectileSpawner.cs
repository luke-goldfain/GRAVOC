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
            proj.SetUpProjectile();
            proj.gameObject.transform.position = this.transform.position + new Vector3(0, this.transform.lossyScale.y, 0);
        }
    }

    public override void SetupSpawnObject(GameObject go, GameObject caster)
    {
        base.SetupSpawnObject(go);
        if (go.GetComponent<Projectile>() != null)
        {
            Projectile gs = go.GetComponent<Projectile>();
            gs.SetUpProjectile();
            gs.gameObject.transform.position = caster.transform.position + new Vector3(0, caster.transform.lossyScale.y, 0);

        }
    }

    public override void SetupSpawnObject(GameObject go, GameObject caster, int chosenAngle, PlayerController pRef)
    {
        base.SetupSpawnObject(go);
        if (go.GetComponent<Projectile>() != null)
        {
            Projectile gs = go.GetComponent<Projectile>();
            gs.SetUpProjectile();
            gs.gameObject.transform.position = caster.transform.position;
            gs.GetComponent<SubScatterShot>()._state = State.BOUNCING;
            gs.GetComponent<SubScatterShot>().IsInstantiatedByScatterShots = true;
            gs.GetComponent<SubScatterShot>().ChosenAngle = chosenAngle;
            gs.GetComponent<SubScatterShot>().playerReference = pRef;
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
