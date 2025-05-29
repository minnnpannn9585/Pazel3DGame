using System.Collections;
using Enemy.FSM;
using UnityEngine;
using Utilities;

namespace Enemy
{
    public class CloakedMagician : BaseEnemy
    {
        public GameObject fireBallPrefab;
        public GameObject fireRingPrefab;
        public Transform fireBallTrans;
        public Transform fireRingTrans;

        protected override void Awake()
        {
            // IdleState = new NormalIdleState();
            // ChaseState = new NormalChaseState();
            // RetreatState = new NormalRetreatState();
            
            NonAttackState = new NormalNonAttackState();
            BasicAttackState = new NormalBasicAttackState();
            BasicSkillState = new NormalBasicSkillState();
            base.Awake();
        }

        protected override void Update()
        {
            base.Update();
            if (targetTrans == null) return;
            var dist = Vector3.Distance(targetTrans.position, transform.position);
            isBasicAttackSatisfied = dist <= attackRange * 2;
            isBasicSkillSatisfied = dist <= attackRange && shield > 0;
        }

        public override IEnumerator BasicAttack()
        {
            var fireBallCount = 0;
            while (fireBallCount < 3)
            {
                var fireBall = Instantiate(fireBallPrefab, fireBallTrans.position, Quaternion.identity);
                fireBall.GetComponent<FireBall>().SetFireBall(transform.forward, attackDamage);
                fireBallCount++;
                yield return new WaitForSeconds(0.5f);
            }
            
            SwitchAttackingState(AttackingState.NonAttack);
        }

        public override IEnumerator BasicSkill()
        {
            var fireRing = Instantiate(fireRingPrefab, fireRingTrans.position, Quaternion.identity, transform);
            fireRing.GetComponent<FireRing>().SetFireRing(attackDamage);
            yield return new WaitForSeconds(0.5f);
            SwitchAttackingState(AttackingState.NonAttack);
        }
    }
}
