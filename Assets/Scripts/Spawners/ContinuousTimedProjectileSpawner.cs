using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContinuousTimedProjectileSpawner : TimedSpawner
{
    private void Update()
    {
        base.addDeadProjectilesToRemoveList();
        base.removeObjectInListToRemove();


        ContinuousSpawn();
       
    }

    protected virtual void ContinuousSpawn()
    {
        if (this.transform.childCount == 0)
        {
            lastSpawnTime += Time.deltaTime;
            if (lastSpawnTime > SpawnTime)
            {
                lastSpawnTime = 0.0f;
                this.Spawn(this.gameObject);
            }
        }
    }
}
