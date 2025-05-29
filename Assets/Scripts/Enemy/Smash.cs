using System;
using Cinemachine;
using Player;
using Snowball;
using Snowman;
using UnityEngine;

namespace Enemy
{
    public class Smash : MonoBehaviour
    {
        public ParticleSystem smashVfx;
        private float _attack;
        private CinemachineImpulseSource _impulseSource;

        private void Awake()
        {
            _impulseSource = GetComponent<CinemachineImpulseSource>();
            _impulseSource.GenerateImpulseWithForce(1f);
        }

        private void Update()
        {
            if (smashVfx.isStopped)
            {
                Destroy(gameObject);
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                other.GetComponent<PlayerAttribute>().TakeDamage(_attack);
            }

            if (other.CompareTag("Snowman"))
            {
                other.GetComponent<SnowmanTakeDamage>().TakeDamage(_attack);
            }
        }

        public void SetAttack(float attack)
        {
            _attack = attack;
        }
    }
}
