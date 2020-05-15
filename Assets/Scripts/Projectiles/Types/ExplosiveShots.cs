using Assets.Scripts.Projectiles;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExplosiveShots : Projectile
{
    public ParticleSystem Explosion;
    private void Start()
    {
        _movementSpeed = 20f;
        rb = GetComponent<Rigidbody>();
        velocity = transform.forward * _movementSpeed;
        _maxBounces = 3;
        _currentBounce = 0;
        _state = State.SPAWNED;
    }
    //public ExplosiveShots(Projectile p)
    //{
    //    projectile = p;
    //    _movementSpeed = 20f;
    //    velocity = p.transform.forward * _movementSpeed;
    //    _maxBounces = 3;
    //    _currentBounce = 0;
    //    _state = State.SPAWNED;
    //}
    //public override void Start()
    //{
    //    throw new NotImplementedException();
    //}

    public void Update()
    {
        switch (_state)
        {
            //This is will be for when the projectile is floating in a spawn point.
            case State.SPAWNED:
                velocity = Vector3.zero;
                break;
            //Want this state to be used for when a projectile gets picked up
            case State.PICKEDUP:
                break;
            case State.HELD:
                Held();
                break;
            case State.SHOT:
                velocity = Direction.normalized * _movementSpeed;
                this._state = State.BOUNCING;
                break;
            case State.BOUNCING:
                velocity = velocity.normalized * _movementSpeed;
                Debug.DrawLine(this.transform.position, rb.velocity, Color.green);
                Explode();
                break;
        }

        if (Input.GetKeyDown(KeyCode.G))
        {
            SceneManager.LoadScene("Environment Test");
        }

        rb.velocity = velocity;
    }

    //PRojectile gets disabled along with its collider. then should be placed into a list of disabled projectiles that can then be enabled and put to a spawn location on the map
    //public override void Explode()
    //{
    //    if (_currentBounce >= _maxBounces)
    //    {
    //        projectile.gameObject.SetActive(false);
    //        projectile.GetComponent<SphereCollider>().enabled = false;
    //    }
    //}

    //public override void Held()
    //{
    //    projectile.transform.position = Vector3.Lerp(projectile.transform.position, projectile.transform.parent.transform.position, 0.2f);
    //}

    public override void OnCollisionEnter(Collision collision)
    {
        if (_state == State.BOUNCING && collision.transform.tag != "Projectile" || collision.transform.tag != "Player")
        {
            ParticleSystem explosion = Instantiate(Explosion, transform.position, Quaternion.identity) as ParticleSystem;

            Debug.DrawRay(collision.GetContact(0).point, collision.GetContact(0).normal, Color.red, 10);
            Vector3 d, n, r;

            d = velocity;
            n = collision.GetContact(0).normal;
            r = d - (2 * Vector3.Dot(d, n) * n);

            velocity = r;
            this._currentBounce++;
        }
    }

    //Here is what will be called when a player has interacted and picked up the projectile
    //public override void PickingUp(Transform targetTransform)
    //{
    //    projectile.rb.isKinematic = true;
    //    projectile.rb.detectCollisions = false;

    //    projectile.transform.parent = targetTransform.transform;

    //    projectile.transform.position = Vector3.Lerp(projectile.transform.position, targetTransform.transform.position, 0.2f);

    //    _state = State.HELD;
    //}

    //public override void PickingUp()
    //{
    //    PickingUp(projectile.playerReference.transform);
    //}

    //Shoot needs the direction of where it needs to fire from
    //public override void Shoot(Vector3 d)
    //{
    //    if (projectile.rb.isKinematic == true && projectile.rb.detectCollisions == false)
    //    {
    //        projectile.rb.isKinematic = false;
    //        projectile.rb.detectCollisions = true;
    //        projectile.transform.parent = null;
    //    }

    //    Direction = d;
    //    _state = State.SHOT;
    //}

    //public override void Shoot()
    //{
    //    Shoot(projectile.transform.forward);
    //}
}
