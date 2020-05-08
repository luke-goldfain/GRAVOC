using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISpawner
{
    bool SpawnerEnabled { get; set; }

    void Spawn();
    void AddGameObject(GameObject spawn);
    void SetupSpawnObject(GameObject go);
}
