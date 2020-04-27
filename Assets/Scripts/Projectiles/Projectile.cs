using Assets.Scripts.Projectiles;
using Assets.Scripts.Projectiles.Types;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public Shot _shot;
    public Rigidbody rb;
    public PlayerController playerReference;

    void Start()
    {
        if (_shot == null)
        {
            _shot = new NormalShots(this);
        }
    }

    void Update()
    {
        if (_shot != null)
        {
            _shot.Update();
        }
    }

    void setNormalShots()
    {
        _shot = new NormalShots(this);
    }

    void setExplosiveShots()
    {
        _shot = new ExplosiveShots(this);
    }

    void setFlameShots()
    {
        _shot = new FlameShots(this);
    }
}
