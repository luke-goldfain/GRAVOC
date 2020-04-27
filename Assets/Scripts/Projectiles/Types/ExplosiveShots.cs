using Assets.Scripts.Projectiles;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveShots : Shot
{
    public ExplosiveShots(Projectile p)
    {
        projectile = p;
    }

    public override void Explode()
    {
        throw new System.NotImplementedException();
    }

    public override void Held()
    {
        throw new System.NotImplementedException();
    }

    public override void OnCollisionEnter(Collision collision)
    {
        throw new System.NotImplementedException();
    }

    public override void PickingUp(Transform targetTransform)
    {
        throw new System.NotImplementedException();
    }

    public override void PickingUp()
    {
        throw new System.NotImplementedException();
    }

    public override void Shoot(Vector3 Direction)
    {
        throw new System.NotImplementedException();
    }

    public override void Shoot()
    {
        throw new System.NotImplementedException();
    }

    public override void Start()
    {
        throw new System.NotImplementedException();
    }

    public override void Update()
    {
        throw new System.NotImplementedException();
    }
}
