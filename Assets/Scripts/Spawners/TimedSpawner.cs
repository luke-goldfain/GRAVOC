using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimedSpawner : ProjectileSpawner
{
    public float SpawnTime;
    protected float lastSpawnTime;
    protected bool spawned;

    // Update is called once per frame
    void Update()
    {
        base.addDeadProjectilesToRemoveList();
        base.removeObjectInListToRemove();

        lastSpawnTime += Time.deltaTime;
        if ((lastSpawnTime > SpawnTime) && (spawned == false))
        {
            this.Spawn(this.gameObject);
            this.spawned = true;
            this.lastSpawnTime = 0f;
        }

    }
}
