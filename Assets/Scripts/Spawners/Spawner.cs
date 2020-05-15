using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour, ISpawner
{
    public bool SpawnerEnabled { get; set; }

    public GameObject SpawnObject;
    public bool Enabled = true;

    protected List<GameObject> gameObjects;
    protected List<GameObject> objectsToRemove;

    // Start is called before the first frame update
    void Start()
    {
        this.gameObjects = new List<GameObject>();
        this.objectsToRemove = new List<GameObject>();
        this.SpawnerEnabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        removeObjectInListToRemove();

        //This is just to test the spawner
        if (Input.GetKeyDown(KeyCode.G))
        {
            if (SpawnObject != null)
            {
                Spawn();
            }
        }
    }

    protected virtual void removeObjectInListToRemove()
    {
        //remove objects in Object to remove list
        foreach (GameObject go in this.objectsToRemove)
        {
            this.gameObjects.Remove(go);
            Object.Destroy(go);
        }
        this.objectsToRemove.Clear();
    }

    public void Spawn()
    {
        if (SpawnerEnabled)
        {
            GameObject spawn = this.getSpawnObject();
            if (spawn != null)
            {
                SetupSpawnObject(spawn); //virtual hook for setting up game object
                this.AddGameObject(spawn);
            }
        }
    }

    public void Spawn(GameObject caster)
    {
        if (SpawnerEnabled)
        {
            GameObject spawn = this.getSpawnObject();
            if (spawn != null)
            {
                SetupSpawnObject(spawn, caster);
                this.AddGameObject(spawn);
            }
        }
    }

    public void SpawnSubScatter(GameObject caster, int chosenAngle, PlayerController pRef)
    {
        if (SpawnerEnabled)
        {
            GameObject spawn = this.getSpawnObject();
            if (spawn != null)
            {
                SetupSpawnObject(spawn, caster, chosenAngle, pRef);
                this.AddGameObject(spawn);
            }
        }
    }

    protected virtual GameObject getSpawnObject()
    {
        GameObject spawn = (GameObject)Instantiate(SpawnObject, this.transform.position, Quaternion.identity, this.transform);
        return spawn;
    }

    public virtual void AddGameObject(GameObject spawn)
    {
        gameObjects.Add(spawn);
    }

    public virtual void SetupSpawnObject(GameObject go)
    {
        go.transform.parent = this.gameObject.transform; //make the spawned object a child of the spawner
    }

    public virtual void SetupSpawnObject(GameObject go, GameObject caster)
    {
        go.transform.parent = this.gameObject.transform; //make the spawned object a child of the spawner
    }

    public virtual void SetupSpawnObject(GameObject go, GameObject caster, int chosenAngle, PlayerController pRef)
    {
        go.transform.parent = this.gameObject.transform; //make the spawned object a child of the spawner
    }
}
