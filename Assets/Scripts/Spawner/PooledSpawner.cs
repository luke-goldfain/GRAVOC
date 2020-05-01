using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PooledSpawner : Spawner
{
    protected override GameObject getSpawnObject()
    {
        GameObject spawn = ObjectPoolingManager.Instance.Getobject("Projectile");
        if(spawn != null)
        {
            spawn.SetActive(true);
        }
        return spawn;
    }
}
