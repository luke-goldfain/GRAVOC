using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pool : MonoBehaviour
{
    public GameObject Projectile;

    public int InitialPoolSize = 1;
    public int MaxPoolSize = 5;


    // Start is called before the first frame update
    void Start()
    {
        CreateObjectPools();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void CreateObjectPools()
    {
        ObjectPoolingManager.Instance.CreatePool(Projectile, this.InitialPoolSize, this.MaxPoolSize, false);
        ObjectPoolingManager.Instance.PoolGameObject = this.gameObject;
    }
}
