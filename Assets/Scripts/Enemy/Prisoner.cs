using System.Collections;
using Enemy.FSM;
using UnityEngine;
using UnityEngine.AI;
using Utilities;

namespace Enemy
{
    public class Prisoner : BaseEnemy
    {
        public SphereCollider sphereCollider;
        public GameObject model;
        
        protected override void Awake()
        {
            NonAttackState = new NormalNonAttackState();
            BasicAttackState = new NormalBasicAttackState();
            base.Awake();
            isChasing = true;
            hudCanvas.SetActive(false);
            // agent = GetComponentInParent<NavMeshAgent>();
        }
        
        protected override void Update()
        {
            base.Update();
            if (!model.CompareTag("Enemy"))
            {
                model.tag = "Enemy";
                EventHandler.ShowInteractableSign(false, "talk");
                sphereCollider.enabled = false;
                hudCanvas.SetActive(true);
            }
            if (targetTrans == null) return;
            var dist = Vector3.Distance(targetTrans.position, transform.position);
            isBasicAttackSatisfied = dist <= attackRange * 2;
        }
        
        public override IEnumerator BasicAttack()
        {
            animator.SetTrigger("AttackTrigger");
            yield return new WaitForSeconds(1f);
            SwitchAttackingState(AttackingState.NonAttack);
        }
    }
}
