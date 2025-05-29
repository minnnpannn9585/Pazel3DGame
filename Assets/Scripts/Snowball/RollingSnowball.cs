using System;
using Cinemachine;
using Enemy;
using Player;
using Snowman.Skills;
using UnityEngine;
using UnityEngine.Serialization;

namespace Snowball
{
    /*
     * Rolling snowball
     */
    public class RollingSnowball : BaseSnowball
    {
        public float rollingDistance = 15f;
        public Vector2 rollingSize;
        public GameObject explosionVfx;
        private Vector3 _lastPosition;
        private float _accumulatedDistance;
        private PlayerController _playerController;
        private bool _isReleasing;
        private CinemachineImpulseSource _impulseSource;

        protected override void Awake()
        {
            base.Awake();
            _accumulatedDistance = 0f;
            transform.localScale = new Vector3(rollingSize.x, rollingSize.x, rollingSize.x);
            _playerController = PlayerAttr.gameObject.GetComponent<PlayerController>();
            _impulseSource = GetComponent<CinemachineImpulseSource>();
        }

        private void Update()
        {
            if (transform.localScale.x > rollingSize.y && !_isReleasing)
            {
                _playerController.OnRollingSnowballEnd();
            }
        }
        
        private void FixedUpdate()
        {
            if (!_isReleasing) return;
            var distanceMoved = Vector3.Distance(transform.position, _lastPosition);
            _accumulatedDistance += distanceMoved;
            
            _lastPosition = transform.position;
            
            if (_accumulatedDistance > rollingDistance)
            {
                Destroy(gameObject);
            }
        }

        private void OnCollisionEnter(Collision other)
        {
            var otherGO = other.gameObject;
            if (!otherGO.CompareTag("Ground") && !otherGO.CompareTag("Projectile"))
            {
                // Debug.Log(damage);
                var explosion = Instantiate(explosionVfx, transform.position, Quaternion.identity);
                var explosionScript = explosion.GetComponent<SnowmanExplosion>();
                explosionScript.SetRadius(transform.localScale.x, true);
                explosionScript.SetAttack(damage, shieldBreakEfficiency);
                _impulseSource.GenerateImpulseWithForce(0.5f);
                Destroy(gameObject);
            }
        }

        /*
         * Force release snowball when rolling snowball reached the max size
         */
        public void SetReleasingState()
        {
            _isReleasing = true;
            _lastPosition = transform.position;
        }
    }
}
