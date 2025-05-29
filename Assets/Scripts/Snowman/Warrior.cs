using System.Collections;
using Snowman.Skills;
using UnityEngine;
using Utilities;

namespace Snowman
{
    public class Warrior : BaseSnowman
    {
        public GameObject slashPrefab;
        public float attackRange;
        public Transform slashStartTrans;
        private Coroutine _attackCoroutine;
        private AudioSource _audioSource;

        protected override void Awake()
        {
            _audioSource = GetComponent<AudioSource>();
            base.Awake();
        }

        protected override void Update()
        {
            base.Update();
            if (TargetTrans != null && Vector3.Distance(TargetTrans.position, transform.position) <= attackRange*2)
                StartAttacking();
            else
                StopAttacking();
        }

        private IEnumerator AttackCoroutine()
        {
            while (Vector3.Distance(TargetTrans.position, transform.position) <= attackRange*2)
            {
                yield return new WaitForSeconds(MySnowmanSO.attackSpeed);
                var slashGO = Instantiate(slashPrefab, slashStartTrans.position, Quaternion.identity);
                slashGO.GetComponent<DimensionalSlash>().SetAttack(MySnowmanSO.attack, level == SnowmanLevel.Advanced, MySnowmanSO.shieldBreakEfficiency);
                _audioSource.Play();
            }
        }
        
        private void StartAttacking()
        {
            _attackCoroutine ??= StartCoroutine(AttackCoroutine());
        }
        
        private void StopAttacking()
        {
            if (_attackCoroutine == null) return;
            StopCoroutine(_attackCoroutine);
            _attackCoroutine = null;
        }
    }
}
