using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PooledSpawner : Spawner
{
    [SerializeField]
    protected string objectName;
    protected override GameObject getSpawnObject()
    {
        GameObject spawn = ObjectPoolingManager.Instance.Getobject(objectName);
        if (spawn != null)
        {
            spawn.SetActive(true);
        }
        return spawn;
    }

    protected override void removeObjectInListToRemove()
    {
        foreach (GameObject go in this.objectsToRemove)
        {
            this.gameObjects.Remove(go);
            go.transform.parent = null;
            go.SetActive(false);
        }
        this.objectsToRemove.Clear();
    }
}
