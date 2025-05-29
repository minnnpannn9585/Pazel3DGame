using System;
using UnityEngine;

namespace Snowball
{
    public class ThrowingVfx : MonoBehaviour
    {
        public ParticleSystem vfx;
        public AudioClip hit;
        public AudioClip miss;
        private AudioSource _audioSource;

        private void Awake()
        {
            _audioSource = GetComponent<AudioSource>();
        }

        private void Update()
        {
            if (vfx.isStopped) Destroy(gameObject);
        }

        public void PlaySfx(bool isHit)
        {
            if (_audioSource == null) return;
            _audioSource.clip = isHit ? hit : miss;
            _audioSource.Play();
        }
    }
}
