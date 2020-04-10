using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum State
{
    HELD,
    SHOT
}

public class Projectile : IProjectable
{
    public float _movementeSpeed;
    public float _maxBounces;
    public int _damage;
    public State _state;


    public void Explode()
    {
        throw new System.NotImplementedException();
    }

    public void Shoot()
    {
        throw new System.NotImplementedException();
    }
}
