using System;
using Cinemachine;
using Player;
using Snowman;
using UnityEngine;

namespace Enemy
{
    public class ThrowingRock : MonoBehaviour
    {
        private float _attack;
        public bool isLanded;
        private CinemachineImpulseSource _impulseSource;
        private AudioSource _audioSource;
        
        private void Awake()
        {
            _audioSource = GetComponent<AudioSource>();
            _impulseSource = GetComponent<CinemachineImpulseSource>();
        }

        private void OnCollisionEnter(Collision other)
        {
            if (isLanded) return;
            
            if (other.gameObject.CompareTag("Ground"))
            {
                _audioSource.Play();
                isLanded = true;
                _impulseSource.GenerateImpulseWithForce(1f);
            }

            if (other.gameObject.CompareTag("Player"))
            {
                other.gameObject.GetComponent<PlayerAttribute>().TakeDamage(_attack);
            }

            if (other.gameObject.CompareTag("Snowman"))
            {
                other.gameObject.GetComponent<SnowmanTakeDamage>().TakeDamage(_attack);
            }
        }

        public void SetAttack(float attack)
        {
            _attack = attack;
        }
    }
}
