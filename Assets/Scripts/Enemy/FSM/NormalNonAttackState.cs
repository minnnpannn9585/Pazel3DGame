using System.Collections;
using UnityEngine;
using Utilities;

namespace Enemy.FSM
{
    public class NormalNonAttackState : BaseState
    {
        private float _timer;
        public override void OnEnter(BaseEnemy enemy)
        {
            CurrentEnemy = enemy;
            
            if (CurrentEnemy.animator == null) return;

            _timer = 1;
            CurrentEnemy.animator.SetBool(EnemyAnimatorPara.IsAttacking.ToString(), false);
        }

        public override void OnUpdate()
        {
            if (_timer > 0 || (CurrentEnemy.isBoss && CurrentEnemy.shield <= 0)) return;
            if (CurrentEnemy.isAdvancedSkillReady) CurrentEnemy.SwitchAttackingState(AttackingState.AdvancedSkill);
            else if (CurrentEnemy.isBasicSkillReady) CurrentEnemy.SwitchAttackingState(AttackingState.BasicSkill);
            else if (CurrentEnemy.isBasicAttackReady) CurrentEnemy.SwitchAttackingState(AttackingState.BasicAttack);
            // }
        }

        public override void OnFixedUpdate()
        {
            if (_timer > 0) _timer -= Time.fixedDeltaTime;
        }

        public override void OnExist()
        {
            if (CurrentEnemy.animator == null) return; 
            CurrentEnemy.animator.SetBool(EnemyAnimatorPara.IsAttacking.ToString(), true);
        }

        // private IEnumerator NextAttack()
        // {
        //     yield return new WaitForSeconds(1f);
        //     if (CurrentEnemy.isAdvancedSkillReady) CurrentEnemy.SwitchAttackingState(AttackingState.AdvancedSkill);
        //     else if (CurrentEnemy.isBasicAttackReady) CurrentEnemy.SwitchAttackingState(AttackingState.BasicAttack);
        //     else if (CurrentEnemy.isBasicSkillReady) CurrentEnemy.SwitchAttackingState(AttackingState.BasicSkill);
        // }
    }
}
