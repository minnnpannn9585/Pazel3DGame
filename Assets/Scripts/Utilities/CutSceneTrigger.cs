using System;
using UnityEngine;
using UnityEngine.Events;

namespace Utilities
{
    public class CutSceneTrigger : MonoBehaviour
    {
        public UnityEvent triggerEvent;
        public GameObject vfx;

        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Player")) return;
            triggerEvent?.Invoke();
            if (vfx != null) Instantiate(vfx, transform.position, transform.rotation, transform.parent);
            Destroy(gameObject);
        }
    }
}
