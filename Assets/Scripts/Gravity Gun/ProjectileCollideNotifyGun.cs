using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileCollideNotifyGun : MonoBehaviour
{
    [HideInInspector]
    public GravGunController GravGun;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Projectile")
        {
            GravGun.GrabProjectile(other.gameObject.GetComponent<Projectile>());

            GravGun.RemoveCurrentPulse();
        }
    }
}
