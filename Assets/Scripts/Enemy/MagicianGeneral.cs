using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Enemy.FSM;
using UnityEngine;
using Utilities;

namespace Enemy
{
    public class MagicianGeneral : BaseEnemy
    {
        public GameObject flameRays;
        public GameObject fireBallLarge;
        public List<Transform> throwPoints;
        public GameObject smashVfx;
        public List<Transform> smashTransList;
        public CinemachineImpulseSource fireRayImpulse;
        public CinemachineImpulseSource fireRingImpulse;
        public AudioSource audioSource;
        
        protected override void Awake()
        {
            // IdleState = new NormalIdleState();
            // ChaseState = new NormalChaseState();
            // RetreatState = new NormalRetreatState();
            
            NonAttackState = new NormalNonAttackState();
            BasicAttackState = new NormalBasicAttackState();
            BasicSkillState = new NormalBasicSkillState();
            AdvancedSkillState = new NormalAdvancedSkillState();
            base.Awake();
        }
        
        protected override void Update()
        {
            base.Update();

            if (targetTrans == null) return;
            var dist = Vector3.Distance(targetTrans.position, transform.position);
            isBasicAttackSatisfied = dist <= attackRange * 2f;
            if (currentStage >= StageTwo) isBasicSkillSatisfied = dist <= attackRange;
            if (currentStage >= StageThree) isAdvancedSkillSatisfied = dist <= attackRange;
            
            if (shield <= 0) AfterShieldBreaking();
        }

        public override IEnumerator BasicAttack()
        {
            //Fire Ball
            audioSource.Play();
            var i = 0;
            while (i < throwPoints.Count)
            {
                var fireball = Instantiate(fireBallLarge, throwPoints[i].position, Quaternion.identity);
                fireball.GetComponent<FireBall>().SetFireBall(transform.forward, attackDamage);
                i++;
                yield return null;
            }
            
            yield return new WaitForSeconds(1f);
            SwitchAttackingState(AttackingState.NonAttack);
        }

        public override IEnumerator BasicSkill()
        {
            //Fire Ring
            agent.speed = 0f;
            
            yield return new WaitForSeconds(1f);
            var i = 0;
            while (i < smashTransList.Count)
            {
                var shockwave = Instantiate(smashVfx, smashTransList[i].position, Quaternion.identity);
                shockwave.GetComponent<FireRing>().SetFireRing(attackDamage);
                i++;
                fireRingImpulse.GenerateImpulseWithForce(0.5f);
                yield return new WaitForSeconds(0.2f);
            }
            // var smash = Instantiate(smashVfx, transform.position, Quaternion.identity);
            // smash.GetComponent<Smash>().SetAttack(attackDamage);
            agent.speed = speed;
            SwitchAttackingState(AttackingState.NonAttack);
        }

        public override IEnumerator AdvancedSkill()
        {
            //Fire Rays
            var raysGO = Instantiate(flameRays, transform.position, Quaternion.identity);
            var raysScript = raysGO.GetComponent<FlameRays>();
            raysScript.SetFlameRays(transform, attackDamage, 5f);
            fireRayImpulse.GenerateImpulseWithForce(0.5f);
            yield return new WaitForSeconds(5f);
            // flameRays.SetActive(false);
            raysScript.DestroyMe();
            SwitchAttackingState(AttackingState.NonAttack);
        }
    }
}
