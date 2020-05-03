using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGroundHitbox : MonoBehaviour
{
    [SerializeField]
    private PlayerController pc;

    private void OnTriggerStay(Collider other)
    {
        pc.HitboxOnGround = true;
    }

    private void OnTriggerExit(Collider other)
    {
        pc.HitboxOnGround = false;
    }
}
