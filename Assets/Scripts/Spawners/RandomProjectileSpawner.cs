using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomProjectileSpawner : MonoBehaviour
{
    protected float lastSpawnTime;
    public float SpawnTime;

    private void Start()
    {
        lastSpawnTime = 0;
    }

    [Range(0, 1)] public float NormalShotPercentage;
    [Range(0, 1)] public float PrecisionShotPercentage;
    [Range(0, 1)] public float ExplosiveShotPercentage;
    [Range(0, 1)] public float ScatterShotPercentage;


    // Update is called once per frame
    void Update()
    {
        //base.addDeadProjectilesToRemoveList();
        //base.removeObjectInListToRemove();
        ContinuousSpawn();
    }

    protected void ContinuousSpawn()
    {
        if (this.transform.childCount == 0)
        {
            this.lastSpawnTime += Time.deltaTime;
            if (lastSpawnTime > SpawnTime)
            {
                //base.ContinuousSpawn();
                if (Random.value < NormalShotPercentage)
                {
                    Debug.Log("Normal");
                    if (this.GetComponent<NormalShotSpawner>())
                    {
                        this.GetComponent<NormalShotSpawner>().Spawn(this.gameObject);
                    }
                    else
                    {
                        Debug.Log("There is no reference for the NormalShotSpawner");
                    }
                }
                else if (Random.value < PrecisionShotPercentage)
                {
                    Debug.Log("Precision");
                    if (this.GetComponent<PrecisionShotSpawner>())
                    {
                        this.GetComponent<PrecisionShotSpawner>().Spawn(this.gameObject);
                    }

                    else
                    {
                        Debug.Log("There is no reference for the PrecisionSpawner");
                    }
                }
                else if (Random.value < ExplosiveShotPercentage)
                {
                    Debug.Log("ExplosiveShot");
                    if (this.GetComponent<ExplosiveShotSpawner>())
                    {
                        this.GetComponent<ExplosiveShotSpawner>().Spawn(this.gameObject);
                    }
                    else
                    {
                        Debug.Log("There is no reference for the ExplosiveShotSpawner");
                    }
                }
                else if (Random.value < ScatterShotPercentage)
                {
                    Debug.Log("ScatterShot");
                    if (this.GetComponent<ScatterShotSpawner>())
                    {
                        this.GetComponent<ScatterShotSpawner>().Spawn(this.gameObject);
                    }
                    else
                    {
                        Debug.Log("There is no reference for the ScatterShotSpawner");
                    }
                }
                else
                {
                    Debug.Log("NormalShot");
                    if (this.GetComponent<NormalShotSpawner>())
                    {
                        this.GetComponent<NormalShotSpawner>().Spawn(this.gameObject);
                    }
                    else
                    {
                        Debug.Log("There is no reference for the NormalShotSpawner");
                    }
                }

                lastSpawnTime = 0f;
            }
        }
    }
}
