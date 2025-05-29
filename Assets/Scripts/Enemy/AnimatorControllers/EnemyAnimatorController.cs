using UnityEngine;
using Utilities;

namespace Enemy.AnimatorControllers
{
    public class EnemyAnimatorController : MonoBehaviour
    {
        public BaseEnemy enemy;

        public void Attack()
        {
            enemy.CurrentAttackingState.OnCall();
        }

        public void Death()
        {
            enemy.Death();
        }

        public void DestroyParent()
        {
            Destroy(transform.parent.gameObject);
        }

        public void DeathVfx(GameObject vfx)
        {
            Instantiate(vfx, transform.position, Quaternion.identity);
        }
    }
}
