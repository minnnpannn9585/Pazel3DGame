using System.Collections;
using System.Reflection;
using BTFrame;
using Enemy.FSM;
using UnityEngine;

namespace Enemy
{
    /*
     * Normal enemy extends from base enemy
     */
    public class NormalEnemy : BaseEnemy
    {
        [Header("Normal Enemy Settings")]
        public GameObject fireRing;

        // protected override void Awake()
        // {
        //     IdleState = new NormalIdleState();
        //     ChaseState = new NormalChaseState();
        //     RetreatState = new NormalRetreatState();
        //     base.Awake();
        // }
    }
}