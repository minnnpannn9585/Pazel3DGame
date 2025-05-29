using System;
using Enemy;
using UnityEngine;
using Utilities;

namespace Snowman.Skills
{
    public class Spike : MonoBehaviour
    {
        public ParticleSystem spikeVfx;
        private float _attack;
        private ShieldBreakEfficiency _efficiency;

        private void Update()
        {
            if (spikeVfx.isStopped)
            {
                Destroy(gameObject);
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Enemy")) return;
            
            other.GetComponent<BaseEnemy>().TakeDamage(_attack, _efficiency);
        }

        public void SetSpike(float attack, ShieldBreakEfficiency efficiency)
        {
            _attack = attack;
            _efficiency = efficiency;
        }
    }
}
