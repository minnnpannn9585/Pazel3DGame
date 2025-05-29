using System;
using Enemy;
using Player;
using UnityEngine;
using Utilities;

namespace Snowman.Skills
{
    public class DimensionalSlash : MonoBehaviour
    {
        public ParticleSystem slashVfx;
        
        private float _attack;
        private ShieldBreakEfficiency _shieldBreakEfficiency;
        private bool _isAdvanced;
        private PlayerAttribute _playerAttr;

        private void Awake()
        {
            _playerAttr = GameObject.FindWithTag("Player").GetComponent<PlayerAttribute>();
        }

        private void Update()
        {
            if (slashVfx.isStopped) Destroy(gameObject);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Enemy")) return;
            other.gameObject.GetComponent<BaseEnemy>().TakeDamage(_attack, _shieldBreakEfficiency);

            if (_isAdvanced)
            {
                foreach (var snowman in _playerAttr.snowmanList)
                {
                    snowman.cooldownTimer -= 1;
                }
            }
        }

        public void SetAttack(float attack, bool isAdvanced, ShieldBreakEfficiency shieldBreakEfficiency)
        {
            _attack = attack;
            _isAdvanced = isAdvanced;
            _shieldBreakEfficiency = shieldBreakEfficiency;
        }
    }
}
