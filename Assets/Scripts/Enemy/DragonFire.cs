using System;
using System.Collections;
using Player;
using Snowman;
using UnityEngine;

namespace Enemy
{
    public class DragonFire : MonoBehaviour
    {
        private float _damage;
        private Coroutine _dealDamageToPlayerCoroutine;
        private Coroutine _dealDamageToSnowmanCoroutine;

        private void OnTriggerStay(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                var player = other.GetComponent<PlayerAttribute>();
                _dealDamageToPlayerCoroutine ??= StartCoroutine(DealDamageToPlayer(player));
            }
            
            if (other.CompareTag("Snowman"))
            {
                var snowman = other.GetComponent<SnowmanTakeDamage>();
                _dealDamageToSnowmanCoroutine ??= StartCoroutine(DealDamageToSnowman(snowman));
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                if (_dealDamageToPlayerCoroutine == null) return;
                StopCoroutine(_dealDamageToPlayerCoroutine);
                _dealDamageToPlayerCoroutine = null;
            }
            
            if (other.CompareTag("Snowman"))
            {
                if (_dealDamageToSnowmanCoroutine == null) return;
                StopCoroutine(_dealDamageToSnowmanCoroutine);
                _dealDamageToSnowmanCoroutine = null;
            }
        }

        private IEnumerator DealDamageToPlayer(PlayerAttribute playerAttr)
        {
            while (true)
            {
                playerAttr.TakeDamage(_damage);
                yield return new WaitForSeconds(1f);
            }
        }
        
        private IEnumerator DealDamageToSnowman(SnowmanTakeDamage snowman)
        {
            while (true)
            {
                snowman.TakeDamage(_damage);
                yield return new WaitForSeconds(1f);
            }
        }

        public void SetAttack(float damage)
        {
            _damage = damage;
        }
    }
}
