using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContinuousTimeProjectileSpawner : TimedSpawner
{
    private void Update()
    {
        base.addDeadProjectilesToRemoveList();
        base.removeObjectInListToRemove();


        ContinuousSpawn();
        //if (this.transform.childCount == 0)
        //{
        //    lastSpawnTime += Time.deltaTime;
        //    if (lastSpawnTime > SpawnTime)
        //    {
        //        lastSpawnTime = 0.0f;
        //        this.Spawn();
        //    }
        //}
        //lastSpawnTime += Time.deltaTime;
        //if (lastSpawnTime > SpawnTime)
        //{
        //    lastSpawnTime = 0.0f;
        //    this.Spawn();
        //}
    }

    protected virtual void ContinuousSpawn()
    {
        if (this.transform.childCount == 0)
        {
            lastSpawnTime += Time.deltaTime;
            if (lastSpawnTime > SpawnTime)
            {
                lastSpawnTime = 0.0f;
                this.Spawn();
            }
        }
    }
}
