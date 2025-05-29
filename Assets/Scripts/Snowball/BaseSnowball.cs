using System;
using DataSO;
using Player;
using UnityEngine;
using Utilities;

namespace Snowball
{
    /*
     * Super class of snowball
     */
    public class BaseSnowball : MonoBehaviour
    {
        public float damage;
        public ShieldBreakEfficiency shieldBreakEfficiency;
        protected PlayerAttribute PlayerAttr;

        protected virtual void Awake()
        {
            PlayerAttr = GameObject.FindWithTag("Player").GetComponent<PlayerAttribute>();
        }

        public void SetAttack(float attack)
        {
            damage = attack;
        }
    }
}
