using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Projectiles
{
    public enum State
    {
        HELD,
        SHOT,
        PICKEDUP,
        SPAWNED,
        BOUNCING
    }

    public abstract class Shot : ScriptableObject, IProjectable
    {
        public Projectile projectile;
        public Vector3 Direction;
        public Vector3 velocity;
        public float _movementSpeed;
        public float _maxBounces, _currentBounce;
        public int _damage;
        public bool isInstantiatedByScatterShots;
        public int chosenAngle;
        public State _state;

        public abstract void Start();
        public abstract void Update();
        public abstract void Explode();
        public abstract void Held();
        public abstract void OnCollisionEnter(Collision collision);
        public abstract void PickingUp(Transform targetTransform);
        public abstract void PickingUp();
        public abstract void Shoot(Vector3 Direction);
        public abstract void Shoot();
    }
}
