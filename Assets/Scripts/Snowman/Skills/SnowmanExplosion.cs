using System;
using DataSO;
using Enemy;
using Player;
using UnityEngine;
using Utilities;

namespace Snowman.Skills
{
    public sealed class SnowmanExplosion : MonoBehaviour
    {
        private float _attack;
        private ShieldBreakEfficiency _shieldBreakEfficiency;
        public ParticleSystem particle;
        private SphereCollider _collider;
        private bool _isRollingSnowball;
        private PlayerAttribute _playerAttr;

        private void Awake()
        {
            _playerAttr = GameObject.FindWithTag("Player").GetComponent<PlayerAttribute>();
            _collider = GetComponent<SphereCollider>();
        }

        private void Update()
        {
            if (particle.isStopped) Destroy(gameObject);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Enemy")) return;
            var enemy = other.GetComponent<BaseEnemy>();
            if (enemy.isMarked) _attack += 30;
            enemy.TakeDamage(_attack, _shieldBreakEfficiency);
            if (_isRollingSnowball)
            {
                _playerAttr.mana += _attack * _playerAttr.manaRecovery;
            }
        }

        public void SetAttack(float attack, ShieldBreakEfficiency shieldBreakEfficiency)
        {
            _attack = attack;
            _shieldBreakEfficiency = shieldBreakEfficiency;
        }

        public void SetRadius(float radius, bool isRollingSnowball)
        {
            // var particleShape = particle.shape;
            // particleShape.radius = radius;
            var particleMain = particle.main;
            particleMain.startLifetime = radius;
            // particleMain.duration = radius;
            _collider.radius = radius + 1;
            _isRollingSnowball = isRollingSnowball;
        }
    }
}
