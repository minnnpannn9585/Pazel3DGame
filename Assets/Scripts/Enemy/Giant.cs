using System.Collections;
using System.Collections.Generic;
using Enemy.FSM;
using UnityEngine;
using Utilities;

namespace Enemy
{
    public class Giant : BaseEnemy
    {
        public GameObject smashVfx;
        public GameObject swingVfx;
        public GameObject rock;
        public Transform throwPoint;
        public float throwAngle = 45.0f;
        public float gravity = -Physics.gravity.y;
        public GameObject giantModel;
        
        protected override void Awake()
        {
            NonAttackState = new NormalNonAttackState();
            BasicAttackState = new NormalBasicAttackState();
            if (isBoss) BasicSkillState = new NormalBasicSkillState();
            AdvancedSkillState = new NormalAdvancedSkillState();
            base.Awake();
        }
        
        protected override void Update()
        {
            base.Update();

            if (targetTrans == null) return;
            var dist = Vector3.Distance(targetTrans.position, transform.position);
            
            //swing
            isBasicAttackSatisfied = dist <= attackRange;
            if (isBoss)
            {
                if (shield <= 0) AfterShieldBreaking();
                //throw rock
                if (currentStage >= StageTwo) isBasicSkillSatisfied = dist <= attackRange*3;
                //smash
                if (currentStage >= StageThree) isAdvancedSkillSatisfied = dist <= attackRange * 2;
            }
            else
            {
                isAdvancedSkillSatisfied = dist <= attackRange * 2 && shield > 0;
            }
        }

        public override IEnumerator BasicAttack()
        {
            //Swing Trunk
            swingVfx.SetActive(true);
            var startTime = Time.time;
            while (Time.time - startTime <= 5f)
            {
                giantModel.transform.Rotate(0f, -360f * Time.deltaTime, 0f);
                yield return null;
            }
    
            swingVfx.SetActive(false);
            
            SwitchAttackingState(AttackingState.NonAttack);
        }

        public override IEnumerator BasicSkill()
        {
            //Throw Rock
            var rockGO = Instantiate(rock, throwPoint.position, Quaternion.identity);
            rockGO.GetComponent<ThrowingRock>().SetAttack(attackDamage);
            LaunchStone(rockGO);
            yield return new WaitForSeconds(1f);
            SwitchAttackingState(AttackingState.NonAttack);
        }

        public override IEnumerator AdvancedSkill()
        {
            //Giant Smash
            agent.speed = 0f;
            
            yield return new WaitForSeconds(0.3f);
            
            var smash = Instantiate(smashVfx, transform.position, Quaternion.identity);
            smash.GetComponent<Smash>().SetAttack(attackDamage);
            agent.speed = speed;
            
            SwitchAttackingState(AttackingState.NonAttack);
        }
        
        private void LaunchStone(GameObject stone)
        {
            var rb = stone.GetComponent<Rigidbody>();

            var targetDir = targetTrans.position - throwPoint.position; 
            var distance = targetDir.magnitude;
            var throwAngleRad = throwAngle * Mathf.Deg2Rad;
            
            var velocity = Mathf.Sqrt(distance * gravity / Mathf.Sin(2 * throwAngleRad));

            var velocityXZ = targetDir.normalized * velocity * Mathf.Cos(throwAngleRad);
            var velocityY = velocity * Mathf.Sin(throwAngleRad);

            var finalVelocity = new Vector3(velocityXZ.x, velocityY, velocityXZ.z);

            rb.velocity = finalVelocity;
        }
    }
}
