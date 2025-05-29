using System;
using Enemy;
using UnityEngine;
using Utilities;

namespace Snowman.Skills
{
    public class IceProjectile : MonoBehaviour
    {
        public float speed;
        private Vector3 _direction;
        private float _attack;
        private ShieldBreakEfficiency _efficiency;
        private bool _isAdvanced;

        private void FixedUpdate()
        {
            transform.Translate(_direction*(speed*Time.fixedDeltaTime));
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Snowman") || other.CompareTag("Player") || other.CompareTag("Untagged")) return;

            if (other.CompareTag("Enemy"))
            {
                var enemy = other.GetComponent<BaseEnemy>();
                enemy.TakeDamage(_attack, _efficiency);
                if (_isAdvanced)
                {
                    enemy.SetTargetSign(true);
                }
            }
            
            Destroy(gameObject);
        }

        public void SetProjectile(bool isAdvanced,Vector3 dir, float attack, ShieldBreakEfficiency efficiency)
        {
            _isAdvanced = isAdvanced;
            _direction = dir;
            _attack = attack;
            _efficiency = efficiency;
        }
    }
}
