using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPoolable
{
    bool IsAlive { get; set; }
    void InitializePool();
}
