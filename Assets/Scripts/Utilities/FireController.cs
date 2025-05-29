using System;
using Player;
using UnityEngine;

namespace Utilities
{
    public class FireController : MonoBehaviour
    {
        public ParticleSystem vfx;
        public GameObject fireLight;
        private PlayerAttribute _playerAttr;
        public AudioSource lightSfx;
        public AudioSource fireSfx;

        private void Awake() {
            vfx.Stop();
            fireLight.SetActive(false);
            _playerAttr = GameObject.FindWithTag("Player").GetComponent<PlayerAttribute>();
        }
        // private void OnTriggerEnter(Collider other) {
        //     if (other.CompareTag("Player"))
        //     {
        //         vfx.Play();
        //         fireLight.SetActive(true);
        //     }
        // }

        private void Update()
        {
            if (_playerAttr.isInCombat)
            {
                vfx.Stop();
                fireSfx.Stop();
                fireLight.SetActive(false); 
            }
        }

        // private void OnTriggerExit(Collider other) {
        //     if (other.CompareTag("Player"))
        //     {
        //         vfx.Stop();
        //         fireLight.SetActive(false);
        //     }
        // }

        public void LightFire()
        {
            if (vfx.isStopped)
            {
                vfx.Play();
                fireLight.SetActive(true);
                lightSfx.Play();
                fireSfx.PlayDelayed(1f);
            }
        }
    }
}
