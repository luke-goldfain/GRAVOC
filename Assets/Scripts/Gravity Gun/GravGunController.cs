using Assets.Scripts.Projectiles;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravGunController : MonoBehaviour
{
    [SerializeField]
    private PlayerController pc;

    [SerializeField]
    private GameObject gravityPulsePrefab;
    
    private Projectile currentProjectile;

    private GameObject currentPulse;
    private GameObject currentPulseHitbox;

    private ProjectileCollideNotifyGun currentPulseCollisionScript;
    
    [SerializeField, Tooltip("The cooldown in seconds for the gravity pulse that pulls projectiles in.")]
    private float pulseCooldown;
    private float pulseTimer;

    [SerializeField, Tooltip("The speed in m/s at which a gravity pulse moves forward.")]
    private float pulseSpeed;

    [SerializeField, Tooltip("The distance in meters a gravity pulse should travel before it is removed from play.")]
    private float pulseDistance;

    private Vector3 currentPulseStartPt;

    private Transform shotHoldTransform;

    private int pNum;

    private bool firePressed;
    private float fireAxisPrevFrame;

    // Start is called before the first frame update
    void Start()
    {
        // subject to change - set the transform to hold shots as this transform
        shotHoldTransform = this.transform;

        pNum = pc.PlayerNumber;
    }

    // Update is called once per frame
    void Update()
    {
        if (pulseTimer > 0f) pulseTimer -= Time.deltaTime;

        if (Input.GetButtonDown("P" + pNum + "Fire1") || (fireAxisPrevFrame == 0f && Input.GetAxis("P" + pNum + "Fire1") != 0))
        {
            firePressed = true;
        }
        else
        {
            firePressed = false;
        }

        if (firePressed)
        {
            if (currentProjectile != null)
            {
                currentProjectile.Shoot(this.transform.forward);

                currentProjectile = null;
            }
            else
            {
                UpdateSpawnPulseOnFire();
            }
        }

        if (currentPulse != null)
        {
            UpdateCheckRemoveCurrentPulseDistance();
        }

        fireAxisPrevFrame = Input.GetAxis("P" + pNum + "Fire1");
    }

    private void UpdateSpawnPulseOnFire()
    {
        if (currentProjectile == null && pulseTimer <= 0f)
        {
            pulseTimer = pulseCooldown;

            if (currentPulse != null)
            {
                RemoveCurrentPulse();
            }

            SpawnGravPulse();
        }
    }

    private void SpawnGravPulse()
    {
        // TODO: Object pool these
        currentPulse = Instantiate(gravityPulsePrefab, shotHoldTransform.position, shotHoldTransform.rotation);

        currentPulseCollisionScript = currentPulse.GetComponentInChildren<ProjectileCollideNotifyGun>(); // TODO: Object Pool
        currentPulseCollisionScript.GravGun = this;

        currentPulseHitbox = currentPulseCollisionScript.gameObject;

        currentPulse.GetComponent<Rigidbody>().velocity = currentPulse.transform.forward * pulseSpeed;

        currentPulseStartPt = currentPulse.transform.position;
    }

    private void UpdateCheckRemoveCurrentPulseDistance()
    {
        if (Vector3.Distance(currentPulse.transform.position, currentPulseStartPt) > pulseDistance)
        {
            RemoveCurrentPulse();
        }
    }

    public void RemoveCurrentPulse()
    {
        currentPulse.SetActive(false); // TODO: Object pool

        currentPulse = null;
        currentPulseHitbox = null;
        currentPulseCollisionScript = null;
    }

    public void GrabProjectile(Projectile proj)
    {
        if (proj._state == State.SPAWNED)
        {
            currentProjectile = proj;

            currentProjectile.PickingUp(this.transform);
        }

        //currentProjectile = proj;

        //currentProjectile._shot.PickingUp(this.transform);
    }
}
