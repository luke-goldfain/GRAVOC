//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using UnityEngine;

//namespace Assets.Scripts.Projectiles.Types
//{
//    public class NormalShots : Shot
//    {
//        public NormalShots(Projectile p)
//        {
//            projectile = p;
//            _movementSpeed = 20f;
//            velocity = p.transform.forward * _movementSpeed;
//            _maxBounces = 15;
//            _currentBounce = 0;
//            _state = State.SPAWNED;
//            isInstantiatedByScatterShots = false;
//        }

//        public override void Start()
//        {
//            throw new NotImplementedException();
//        }

//        public override void Update()
//        {
//            switch (_state)
//            {
//                //This is will be for when the projectile is floating in a spawn point.
//                case State.SPAWNED:
//                    velocity = Vector3.zero;
//                    break;
//                //Want this state to be used for when a projectile gets picked up
//                case State.PICKEDUP:
//                    break;
//                case State.HELD:
//                    Held();
//                    break;
//                case State.SHOT:
//                    velocity = Direction.normalized * _movementSpeed;
//                    this._state = State.BOUNCING;
//                    break;
//                case State.BOUNCING:
//                    velocity = velocity.normalized * _movementSpeed;
//                    Debug.DrawLine(projectile.transform.position, projectile.rb.velocity, Color.green);
//                    Explode();
//                    break;
//            }

//            projectile.rb.velocity = velocity;
//        }

//        //PRojectile gets disabled along with its collider. then should be placed into a list of disabled projectiles that can then be enabled and put to a spawn location on the map
//        public override void Explode()
//        {
//            if (_currentBounce >= _maxBounces)
//            {
//                projectile.gameObject.SetActive(false);
//                projectile.GetComponent<SphereCollider>().enabled = false;
//            }
//        }

//        public override void Held()
//        {
//            projectile.transform.position = Vector3.Lerp(projectile.transform.position, projectile.transform.parent.transform.position, 0.2f);
//        }

//        public override void OnCollisionEnter(Collision collision)
//        {
//            if (_state == State.BOUNCING && collision.transform.tag != "Projectile" && collision.transform.tag != "Player")
//            {
//                Debug.DrawRay(collision.GetContact(0).point, collision.GetContact(0).normal, Color.red, 10);
//                Vector3 d, n, r, f;

//                d = velocity;
//                n = collision.GetContact(0).normal;   
//                r = d - (2 * Vector3.Dot(d, n) * n);

//                if (isInstantiatedByScatterShots)
//                {
//                    r = Quaternion.AngleAxis(chosenAngle, Vector3.up) * r;
//                }               

//                velocity = r;
//                this._currentBounce++;
//            }
//        }

//        //Here is what will be called when a player has interacted and picked up the projectile
//        public override void PickingUp(Transform targetTransform)
//        {
//            projectile.rb.isKinematic = true;
//            projectile.rb.detectCollisions = false;

//            projectile.transform.parent = targetTransform.transform;

//            projectile.transform.position = Vector3.Lerp(projectile.transform.position, targetTransform.transform.position, 0.2f);

//            _state = State.HELD;
//        }

//        public override void PickingUp()
//        {
//            PickingUp(projectile.playerReference.transform);
//        }

//        //Shoot needs the direction of where it needs to fire from
//        public override void Shoot(Vector3 d)
//        {
//            if (projectile.rb.isKinematic == true && projectile.rb.detectCollisions == false)
//            {
//                projectile.rb.isKinematic = false;
//                projectile.rb.detectCollisions = true;
//                projectile.transform.parent = null;
//            }

//            Direction = d;
//            _state = State.SHOT;
//        }

//        public override void Shoot()
//        {
//            Shoot(projectile.transform.forward);
//        } 
//    }
//}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalShots : Projectile
{
    public void Start()
    {

        rb = GetComponent<Rigidbody>();
        this._state = State.SPAWNED;
        this._currentBounce = 0;
        this.velocity = this.transform.forward * _movementSpeed;
        this.gameObject.SetActive(true);
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
}
