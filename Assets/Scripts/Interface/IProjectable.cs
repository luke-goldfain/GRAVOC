using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IProjectable
{
    void Shoot(Vector3 Direction);
    //void Shoot();
    void Explode();
    //void OnCollisionEnter(Collision collision);
    void Held();
    void PickingUp(Transform targetTransform);
    //void PickingUp();
}
