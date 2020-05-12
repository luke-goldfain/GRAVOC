using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubScatterShot : Projectile
{
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        this._state = State.BOUNCING;
        this._currentBounce = 0;
        this.velocity = this.transform.forward * _movementSpeed;
        this.gameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        switch (this._state)
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
                Debug.DrawLine(transform.position, rb.velocity, Color.green);
                Explode();

                break;
        }
        rb.velocity = velocity;
    }

    public override void OnCollisionEnter(Collision collision)
    {
        if (this._state == State.BOUNCING && collision.transform.tag != "Projectile" && collision.transform.tag != "Player")
        {
            Debug.DrawRay(collision.GetContact(0).point, collision.GetContact(0).normal, Color.red, 10);
            Vector3 d, n, r;

            d = velocity;
            n = collision.GetContact(0).normal;
            r = d - (2 * Vector3.Dot(d, n) * n);

            velocity = r;
            this._currentBounce++;
            Debug.Log(this.gameObject.name);
        }
    }
}

