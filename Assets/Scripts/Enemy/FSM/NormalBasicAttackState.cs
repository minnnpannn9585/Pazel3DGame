using System.Collections;
using UnityEngine;
using Utilities;

namespace Enemy.FSM
{
    public class NormalBasicAttackState : BaseState
    {
        public override void OnEnter(BaseEnemy enemy)
        {
            CurrentEnemy = enemy;
            // CurrentEnemy.StartCurrentCoroutine(CurrentEnemy.BasicAttackCoroutine, CurrentEnemy.BasicAttack);

            if (CurrentEnemy.animator == null)
            {
                CurrentEnemy.StartCurrentCoroutine(CurrentEnemy.BasicAttackCoroutine, CurrentEnemy.BasicAttack);
            }
            else
            {
                CurrentEnemy.animator.SetBool(EnemyAnimatorPara.IsBasicAttack.ToString(), true);
            }
            
            // CurrentEnemy.animator.SetBool(EnemyAnimatorPara.IsAttacking.ToString(), true);
        }

        public override void OnUpdate()
        {

        }

        public override void OnFixedUpdate()
        {

        }

        public override void OnExist()
        {
            CurrentEnemy.StopCurrentCoroutine(CurrentEnemy.BasicAttackCoroutine);
            var randRange = CurrentEnemy.basicAttackCooldown;
            CurrentEnemy.basicAttackTimer = Random.Range(randRange.x, randRange.y);
            
            if (CurrentEnemy.animator == null) return; 
            CurrentEnemy.animator.SetBool(EnemyAnimatorPara.IsBasicAttack.ToString(), false);
            // CurrentEnemy.animator.SetBool(EnemyAnimatorPara.IsAttacking.ToString(), false);
        }

        public override void OnCall()
        {
            CurrentEnemy.StartCurrentCoroutine(CurrentEnemy.BasicAttackCoroutine, CurrentEnemy.BasicAttack);
        }
    }
}
