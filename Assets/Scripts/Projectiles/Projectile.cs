﻿using Assets.Scripts.Projectiles;
using Assets.Scripts.Projectiles.Types;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public Shot _shot;
    public Rigidbody rb;
    public PlayerController playerReference;
    public GameObject prefabProjectile;

    void Start()
    {
        if (_shot == null)
        {
            _shot = new ScatterShots(this);
            //randomProjectile();
        }

        playerReference = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }

    void Update()
    {
        if (_shot != null)
        {
            _shot.Update();
        }
    }

    private void randomProjectile()
    {
        int randomInt = UnityEngine.Random.Range(0, 3);

        if (randomInt == 0)
        {
            setNormalShots();
        }
        else if (randomInt == 1)
        {
            setExplosiveShots();
        }
        else
        {
            setScatterShots();
        }
    }

    public void setNormalShots()
    {
        _shot = new NormalShots(this);
    }

    public void setScatterShots()
    {
        _shot = new ScatterShots(this);
    }

    public void setExplosiveShots()
    {
        _shot = new ExplosiveShots(this);
    }

    public virtual void OnCollisionEnter(Collision collision)
    {
        _shot.OnCollisionEnter(collision);
    }
}
