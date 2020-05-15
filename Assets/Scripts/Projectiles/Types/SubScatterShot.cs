using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubScatterShot : Projectile
{
    private int chosenAngle;
    public int ChosenAngle
    {
        get { return this.chosenAngle; }
        set
        {
            if(this.chosenAngle != value)
            {
                this.chosenAngle = value;
            }
        }
    }
    private bool isInstantiatedByScatterShots;
    public bool IsInstantiatedByScatterShots
    {
        get { return this.isInstantiatedByScatterShots; }
        set
        {
            if(this.isInstantiatedByScatterShots != value)
            {
                this.isInstantiatedByScatterShots = value;
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        this._state = State.BOUNCING;
        this._currentBounce = 0;
        this.velocity = this.transform.forward * _movementSpeed;
        this.gameObject.SetActive(true);
        this.GetComponent<SphereCollider>().enabled = true;
    }

    public override void SetUpProjectile()
    {
        rb = GetComponent<Rigidbody>();
        this._currentBounce = 0;
        this.gameObject.SetActive(true);
        this.GetComponent<SphereCollider>().enabled = true;
    }

    // Update is called once per frame
    void Update()
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
                Debug.DrawLine(transform.position, rb.velocity, Color.green);
                Explode();
                break;
        }

        rb.velocity = velocity;
    }
    public override void OnCollisionEnter(Collision collision)
    {
        if (_state == State.BOUNCING && collision.transform.tag != "Projectile" && collision.transform.tag != "Player")
        {
            Debug.DrawRay(collision.GetContact(0).point, collision.GetContact(0).normal, Color.red, 10);
            Vector3 d, n, r, f;

            d = velocity;
            n = collision.GetContact(0).normal;
            r = d - (2 * Vector3.Dot(d, n) * n);

            //should only happen once when initially contacted
            if (isInstantiatedByScatterShots)
            {
                r = Quaternion.AngleAxis(chosenAngle, Vector3.up) * r;
                this.isInstantiatedByScatterShots = false;
            }
            velocity = r;
            this._currentBounce++;
        }
    }
}



