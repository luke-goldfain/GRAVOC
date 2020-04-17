using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum State
{
    HELD,
    SHOT,
    PICKEDUP,
    SPAWNED,
    BOUNCING
}

public class Projectile : MonoBehaviour, IProjectable
{
    public float _movementSpeed;
    public float _maxBounces, _currentBounce;
    public int _damage;
    public State _state;

    public PlayerController playerReference;

    private Vector3 _direction;
    public Vector3 Direction
    {
        get { return this._direction; }
        set
        {
            if(this._direction != value)
            {
                this._direction = value;
            }
        }
    }

    private Vector3 velocity;
    private Rigidbody rb;

    private void Start()
    {
        _movementSpeed = 20f;
        rb = GetComponent<Rigidbody>();
        velocity = this.transform.forward * _movementSpeed;
        this._maxBounces = 15;
        this._currentBounce = 0;
    }

    private void Update()
    {
        switch (this._state)
        {
            case State.SPAWNED:
                break;
            case State.PICKEDUP:
                PickedUp();
                break;
            case State.HELD:
                //this.GetComponent<SphereCollider>().enabled = false;
                break;
            case State.SHOT:
                Shoot();
                //this.GetComponent<SphereCollider>().enabled = true;
                rb.velocity = velocity;
                this._state = State.BOUNCING;
                break;
            case State.BOUNCING:
                velocity = velocity.normalized * _movementSpeed;
                rb.velocity = velocity;
                Debug.DrawLine(transform.position, rb.velocity, Color.green);
                Explode();

                break;
        }
    }


    private void OnCollisionEnter(Collision collision)
    {
        if(this._state == State.BOUNCING)
        {

        Vector3 d, n, r;

        foreach(var contact in collision.contacts)
        {
            Debug.DrawRay(contact.point, contact.normal, Color.red, 10);
            d = velocity;
            n = contact.normal;
            r = d - (2 * Vector3.Dot(d, n) * n);

            velocity = r;
        }
            this._currentBounce++;

        }
            
    }

    //PRojectile gets disabled along with its collider. then should be placed into a list of disabled projectiles that can then be enabled and put to a spawn location on the map
    public void Explode()
    {
        if (this._currentBounce >= _maxBounces)
        {
            this.gameObject.SetActive(false);
            this.GetComponent<SphereCollider>().enabled = false;
        }
    }

    //Here is what will be called when a player has interacted and picked up the projectile
    public void PickedUp()
    {
        if (this.GetComponentInParent<PlayerController>())
        {
            this.transform.position = Vector3.Lerp(this.transform.position, this.playerReference.transform.position, this._movementSpeed * Time.deltaTime);
        }
    }

    //Shoot needs the direction of where it needs to fire from
    public virtual void Shoot(Vector3 Direction)
    {
        this.Direction = Direction;
        this._state = State.SHOT;

    }
    
    private void Shoot()
    {
        Shoot(this.transform.forward);
    }

}
