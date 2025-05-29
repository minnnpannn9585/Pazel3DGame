using System;
using Player;
using Snowman;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.VFX;

namespace Enemy
{
    public class FireRay : MonoBehaviour
    {
        public FlameRays rays;
        private float _attack;
        private float _duration;
        [FormerlySerializedAs("_vfx")] public VisualEffect vfx;

        // private void Awake()
        // {
        //     _attack = rays.attack;
        //     _duration = rays.duration;
        //     vfx.SetFloat("Duration", _duration);
        // }

        private void Start()
        {
            _attack = rays.attack;
            _duration = rays.duration;
            vfx.SetFloat("Duration", _duration);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                other.GetComponent<PlayerAttribute>().TakeDamage(_attack);
            }

            if (other.CompareTag("Snowman"))
            {
                other.GetComponent<SnowmanTakeDamage>().TakeDamage(_attack);
            }
        }
    }
}