using System;
using UnityEngine;

namespace Enemy
{
    public class SquareFlame : MonoBehaviour
    {
        public ParticleSystem flameVfx;

        private void Update()
        {
            if (flameVfx.isStopped) Destroy(gameObject);
        }
    }
}
