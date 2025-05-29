using System;
using Player;
using UnityEngine;
using Utilities;

namespace Snowman.Skills
{
    public class Alchemy : MonoBehaviour
    {
        public GameObject spikeVfx;
        private bool _isAdvanced;
        private float _attack;
        private ShieldBreakEfficiency _efficiency;
        public float timer = 20f;

        private void FixedUpdate()
        {
            if (timer > 0) timer -= Time.fixedDeltaTime;
            else Destroy(gameObject);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Player")) return;

            other.GetComponent<PlayerAttribute>().stamina += _attack * 3;

            if (_isAdvanced)
            {
                var rPlayer = other.transform.rotation;
                // var rotation = new Quaternion(rPlayer.x, rPlayer.y + 90, rPlayer.z, rPlayer.w);
                var spike = Instantiate(spikeVfx, other.transform.position, rPlayer); 
                spike.GetComponent<Spike>().SetSpike(_attack, _efficiency);
            }
            
            Destroy(gameObject);
        }

        public void SetAlchemy(bool isAdvanced, float attack, ShieldBreakEfficiency efficiency)
        {
            _isAdvanced = isAdvanced;
            _attack = attack;
            _efficiency = efficiency;
        }
    }
}
