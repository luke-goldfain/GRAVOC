using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionRemove : MonoBehaviour
{
    private const float despawnTime = 0.5f;
    private float despawnTimer;

    private void Start()
    {
        despawnTimer = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        despawnTimer += Time.deltaTime;

        if (despawnTimer >= despawnTime)
        {
            this.gameObject.SetActive(false);
        }
    }
}
