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
    private Transform revertBackToNoParent;


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
        revertBackToNoParent = this.transform;
        this._state = State.SPAWNED;
    }


    private void Update()
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


    private void OnCollisionEnter(Collision collision)
    {
        if(this._state == State.BOUNCING && collision.transform.tag != "Projectile" && collision.transform.tag != "Player")
        {
            Debug.DrawRay(collision.GetContact(0).point, collision.GetContact(0).normal, Color.red, 10);
            Vector3 d, n, r;

                d = velocity;
                n = collision.GetContact(0).normal;
                r = d - (2 * Vector3.Dot(d, n) * n);

                velocity = r;
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

    public void Held()
    {
        this.transform.position = Vector3.Lerp(this.transform.position, this.transform.parent.transform.position, 0.2f);
    }

    //Here is what will be called when a player has interacted and picked up the projectile
    public void PickingUp(Transform targetTransform)
    {
        this.rb.isKinematic = true;
        this.rb.detectCollisions = false;


        this.transform.parent = targetTransform.transform;
        
        this.transform.position = Vector3.Lerp(this.transform.position, targetTransform.transform.position, 0.2f);

        this._state = State.HELD; 
    }

    private void PickingUp()
    {
        PickingUp(playerReference.transform);
    }

    //Shoot needs the direction of where it needs to fire from
    public virtual void Shoot(Vector3 Direction)
    {
        if(this.rb.isKinematic == true && this.rb.detectCollisions == false)
        {
            this.rb.isKinematic = false;
            this.rb.detectCollisions = true;
            this.transform.parent = null;
        }

        this.Direction = Direction;
        this._state = State.SHOT;

    }
    
    private void Shoot()
    {
        Shoot(this.transform.forward);
    }

}
