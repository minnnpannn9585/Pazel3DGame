using System.Collections;
using Enemy.FSM;
using UnityEngine;
using Utilities;

namespace Enemy
{
    public class InfantrySoldier : BaseEnemy
    {
        public GameObject spearVfx;
        public GameObject trailVfx;

        private float _randNum;
        
        protected override void Awake()
        {
            // IdleState = new NormalIdleState();
            // ChaseState = new NormalChaseState();
            // RetreatState = new NormalRetreatState();

            NonAttackState = new NormalNonAttackState();
            BasicAttackState = new NormalBasicAttackState();
            BasicSkillState = new NormalBasicSkillState();

            _randNum = Random.Range(0f, 1f);
            base.Awake();
        }

        protected override void Update()
        {
            if (health <= 0)
            {
                animator.SetFloat(EnemyAnimatorPara.Possibility.ToString(), _randNum);
            }
            base.Update();

            if (targetTrans == null) return;
            var dist = Vector3.Distance(targetTrans.position, transform.position);
            isBasicSkillSatisfied = dist <= attackRange * 2 && shield > 0 && dist > attackRange;
            isBasicAttackSatisfied = dist <= attackRange;
        }

        public override IEnumerator BasicAttack()
        {
            var thrustVfx = spearVfx; 
            if (!thrustVfx.activeSelf)
            { 
                thrustVfx.SetActive(true);
            }

            agent.speed = 0;
            
            yield return new WaitForSeconds(2f);
            agent.speed = speed;
            
            SwitchAttackingState(AttackingState.NonAttack);
        }

        public override IEnumerator BasicSkill()
        {
            agent.speed = speed * 10f;
            trailVfx.SetActive(true);

            yield return new WaitForSeconds(2f);

            agent.speed = speed;
            trailVfx.SetActive(false);
            
            SwitchAttackingState(AttackingState.NonAttack);
        }
    }
}
