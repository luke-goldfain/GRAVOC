//using Assets.Scripts.Projectiles;
//using Assets.Scripts.Projectiles.Types;
//using System;
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class Projectile : MonoBehaviour
//{
//    public Shot _shot;
//    public Rigidbody rb;
//    public PlayerController playerReference;
//    public GameObject prefabProjectile;
//    public ParticleSystem Explosion;

//    void Start()
//    {
//        if (_shot == null)
//        {   
//            randomProjectile();
//        }

//        playerReference = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
//    }

//    void Update()
//    {
//        if (_shot != null)
//        {
//            _shot.Update();
//        }
//    }

//    private void randomProjectile()
//    {
//        int randomInt = UnityEngine.Random.Range(0, 4);

//        if (randomInt == 0)
//        {
//            setNormalShots();
//        }
//        else if (randomInt == 1)
//        {
//            setExplosiveShots();
//        }
//        else if (randomInt == 2)
//        {
//            setPrecisionShots();
//        }
//        else
//        {
//            setScatterShots();
//        }
//    }

//    public void setNormalShots()
//    {
//        _shot = new NormalShots(this);
//    }

//    public void setScatterShots()
//    {
//        _shot = new ScatterShots(this);
//    }

//    public void setPrecisionShots()
//    {
//        _shot = new PrecisionShots(this);
//    }

//    public void setExplosiveShots()
//    {
//        _shot = new ExplosiveShots(this);
//    }

//    public virtual void OnCollisionEnter(Collision collision)
//    {
//        _shot.OnCollisionEnter(collision);
//    }
//}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum State
{
    HELD,
    SHOT,
    PICKEDUP,
    SPAWNED,
    BOUNCING,
    DONE
}

public abstract class Projectile : MonoBehaviour, IProjectable
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
            if (this._direction != value)
            {
                this._direction = value;
            }
        }
    }

    public virtual void SetUpProjectile()
    {
        this.velocity = new Vector3(0, 0, 0);
        rb = GetComponent<Rigidbody>();
        rb.velocity = this.velocity;
        this._state = State.SPAWNED;
        this._currentBounce = 0;
        this.gameObject.SetActive(true);
        this.GetComponent<SphereCollider>().enabled = true;
    }

    protected Vector3 velocity;
    protected Rigidbody rb;

    private void Start()
    {
        _movementSpeed = 20f;
        rb = GetComponent<Rigidbody>();
        velocity = this.transform.forward * _movementSpeed;
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
                if(this.transform.parent != null)
                {
                    this.transform.parent = null;
                }
                Explode();
                break;
            case State.DONE:
                this.velocity = new Vector3(0, 0, 0);
                rb.velocity = new Vector3(0, 0, 0);
                break;
        }
        rb.velocity = velocity;
    }


    public virtual void OnCollisionEnter(Collision collision)
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

        }

    }

    //PRojectile gets disabled along with its collider. then should be placed into a list of disabled projectiles that can then be enabled and put to a spawn location on the map
    public void Explode()
    {
        if (this._currentBounce >= _maxBounces)
        {
            this.gameObject.SetActive(false);
            this.GetComponent<SphereCollider>().enabled = false;
            this._state = State.DONE;
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
        //this.playerReference = targetTransform.GetComponent<PlayerController>();

        // Luke G addition: Assign player reference when picked up, used to determine which player this should hurt when shot
        this.playerReference = targetTransform.transform.root.GetComponent<PlayerController>();

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
        if (this.rb.isKinematic == true && this.rb.detectCollisions == false)
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

