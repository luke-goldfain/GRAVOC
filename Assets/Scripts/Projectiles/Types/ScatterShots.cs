using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Projectiles.Types
{
    public class ScatterShots : Shot
    {
        public ScatterShots(Projectile p)
        {
            projectile = p;
            _movementSpeed = 20f;
            velocity = p.transform.forward * _movementSpeed;
            _maxBounces = 15;
            _currentBounce = 0;
            _state = State.SPAWNED;
        }

        public override void Explode()
        {
            throw new NotImplementedException();
        }

        public override void Held()
        {
            throw new NotImplementedException();
        }

        public override void OnCollisionEnter(Collision collision)
        {
            throw new NotImplementedException();
        }

        public override void PickingUp(Transform targetTransform)
        {
            throw new NotImplementedException();
        }

        public override void PickingUp()
        {
            throw new NotImplementedException();
        }

        public override void Shoot(Vector3 Direction)
        {
            throw new NotImplementedException();
        }

        public override void Shoot()
        {
            throw new NotImplementedException();
        }

        public override void Start()
        {
            throw new NotImplementedException();
        }

        public override void Update()
        {
            throw new NotImplementedException();
        }
    }
}
