using Player;
using Snowman;
using UnityEngine;

namespace Enemy
{
    /*
     * A type of enemy skill that spread fire as a ring
     */
    public class FireRing : MonoBehaviour
    {
        public float maxRadius = 10f;
        public float duration = 2f;
        
        private float _damage;
        private Transform _trans;
        private ParticleSystem _particle;
        private SphereCollider _coll;
        private float _startTime;

        private void Awake()
        {
            _coll = GetComponent<SphereCollider>();
            _particle = GetComponentInChildren<ParticleSystem>();
            _startTime = Time.time;
            _coll.radius = 0;
        }

        private void Update()
        {
            if (_trans != null)
            {
                transform.position = _trans.position;
            }
            
            var timeElapsed = Time.time - _startTime;
            var timeFraction = Mathf.Clamp(timeElapsed / duration, 0f, 1f);
            _coll.radius = Mathf.Lerp(0f, maxRadius, timeFraction);
            
            if (_particle.isStopped) Destroy(gameObject);
        }
        
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                other.gameObject.GetComponent<PlayerAttribute>().TakeDamage(_damage);
            }
            else if (other.CompareTag("Snowman"))
            {
                other.gameObject.GetComponent<SnowmanTakeDamage>().TakeDamage(_damage);
            }
        }
        
        /*
         * Follow the enemy that releases this fire ring
         */
        public void FollowAt(Transform trans, float damage)
        {
            _trans = trans;
            _damage = damage;
            _coll.radius = 0;
        }

        public void SetFireRing(float damage)
        {
            _damage = damage;
        }
    }
}

