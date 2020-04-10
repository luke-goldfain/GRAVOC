using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicShot : Projectile
{
    [SerializeField]
    private float projectileSpeed = 240f;
    [SerializeField]
    private float totalNumberOfBounces = 6;
    [SerializeField]
    private int damage = 1;

    private float currentNumberOfBounces;

    BasicShot() : base()
    {
        _movementeSpeed = projectileSpeed;
        _maxBounces = totalNumberOfBounces;
        _damage = damage;
        currentNumberOfBounces = totalNumberOfBounces;
    }

    public void Explode()
    {

    }

    public void Shoot()
    {

    }
}
