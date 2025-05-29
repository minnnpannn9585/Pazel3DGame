using System;
using System.Collections;
using Enemy;
using UnityEngine;
using Utilities;

namespace Snowman.Skills
{
    public class IceSpike : MonoBehaviour
    {
        private float _attack;
        private ShieldBreakEfficiency _shieldBreakEfficiency;
        public float attackSpeed = 0.5f;
        public float totalDuration = 5f;
        public float slowRate = 0.4f;
        public ParticleSystem particle;
        
        private SphereCollider _sphereCollider;

        private void Awake()
        {
            _sphereCollider = GetComponent<SphereCollider>();
            _sphereCollider.radius = 1;
            StartCoroutine(Attack());
        }

        private void Update()
        {
            if (particle.isStopped) Destroy(gameObject);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Enemy")) return;
            var enemy = other.gameObject.GetComponent<BaseEnemy>();
            enemy.TakeDamage(_attack, _shieldBreakEfficiency);
            enemy.Slowdown(enemy.speed, slowRate, 1f);
        }

        private IEnumerator Attack()
        {
            while (totalDuration > 0)
            {
                _sphereCollider.enabled = !_sphereCollider.enabled; 
                yield return new WaitForSeconds(attackSpeed);
                totalDuration -= attackSpeed;
            }
            Destroy(gameObject);
        }
        
        public void SetAttack(float attack, ShieldBreakEfficiency shieldBreakEfficiency)
        {
            _attack = attack;
            _shieldBreakEfficiency = shieldBreakEfficiency;
        }
    }
}
