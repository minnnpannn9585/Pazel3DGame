using System;
using UnityEngine;

namespace Props
{
    public class DestroyAfterStop : MonoBehaviour
    {
        public ParticleSystem vfx;

        private void Update()
        {
            if (vfx.isStopped) Destroy(gameObject);
        }
    }
}
