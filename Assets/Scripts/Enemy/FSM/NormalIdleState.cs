using UnityEngine;
using Utilities;

namespace Enemy.FSM
{
    // public class NormalIdleState : BaseState
    // {
    //     public override void OnEnter(BaseEnemy enemy)
    //     {
    //         CurrentEnemy = enemy;
    //         
    //         if (CurrentEnemy.animator == null) return; 
    //         CurrentEnemy.animator.SetBool(EnemyAnimatorPara.IsMoving.ToString(), false);
    //     }
    //
    //     public override void OnUpdate()
    //     {
    //         if (CurrentEnemy.targetTrans != null)
    //         {
    //             CurrentEnemy.SwitchMovingState(MovingState.Chase);
    //         }
    //     }
    //
    //     public override void OnFixedUpdate()
    //     {
    //         // if (CurrentEnemy.isChasing) return;
    //         // if (CurrentEnemy.health < CurrentEnemy.maxHealth) CurrentEnemy.health += 5 * Time.fixedDeltaTime;
    //         // if (CurrentEnemy.shield < CurrentEnemy.maxShield) CurrentEnemy.shield += 5 * Time.fixedDeltaTime;
    //     }
    //
    //     public override void OnExist()
    //     {
    //
    //     }
    // }
}
