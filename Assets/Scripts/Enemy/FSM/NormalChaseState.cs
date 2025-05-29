using UnityEngine;
using Utilities;

namespace Enemy.FSM
{
    // public class NormalChaseState : BaseState
    // {
    //     public override void OnEnter(BaseEnemy enemy)
    //     {
    //         CurrentEnemy = enemy;
    //         EventHandler.AddEnemyToCombatList(CurrentEnemy.gameObject);
    //         
    //         if (CurrentEnemy.animator == null) return; 
    //         CurrentEnemy.animator.SetBool(EnemyAnimatorPara.IsMoving.ToString(), true);
    //     }
    //
    //     public override void OnUpdate()
    //     {
    //         if (CurrentEnemy.targetTrans == null)
    //         {
    //             Debug.Log("Chase state tar null");
    //             CurrentEnemy.SetChaseTarget();
    //         }
    //         
    //         CurrentEnemy.StartMoving();
    //         CurrentEnemy.MoveTowardsTarget();
    //     }
    //
    //     public override void OnFixedUpdate()
    //     {
    //         
    //     }
    //
    //     public override void OnExist()
    //     {
    //         EventHandler.RemoveEnemyToCombatList(CurrentEnemy.gameObject);
    //     }
    // }
}
