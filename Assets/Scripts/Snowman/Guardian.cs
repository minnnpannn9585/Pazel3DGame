using Player;
using UnityEngine;
using Utilities;

namespace Snowman
{
    public class Guardian : BaseSnowman
    {
        public float forceFieldRange;
        public float healingFactor;
        public GameObject forceFieldPrefab;

        private SphereCollider _sphereCollider;
        private PlayerAttribute _playerAttr;
        
        protected override void Awake()
        {
            base.Awake();
            _sphereCollider = GetComponent<SphereCollider>();
            _sphereCollider.radius = forceFieldRange;
            CreateForceField();
            _playerAttr = GameObject.FindWithTag("Player").GetComponent<PlayerAttribute>();
        }

        protected override void OnTriggerEnter(Collider other)
        {
            base.OnTriggerEnter(other);
            if (!other.CompareTag("Player")) return;
            _playerAttr.isInvincible = true;
        }
        
        protected override void OnTriggerExit(Collider other)
        {
            base.OnTriggerExit(other);
            if (!other.CompareTag("Player")) return;
            _playerAttr.isInvincible = false;
        }

        private void CreateForceField()
        {
            var forceFieldGO = Instantiate(forceFieldPrefab, transform.position, Quaternion.identity, transform);
            forceFieldGO.transform.localScale = new Vector3(forceFieldRange * 2, forceFieldRange * 2, forceFieldRange * 2);
        }

        protected override void DestroyMe()
        {
            _playerAttr.isInvincible = false;
            base.DestroyMe();
        }

        public override void TakeDamage(float damage)
        {
            base.TakeDamage(damage);
            if (level != SnowmanLevel.Advanced || !_playerAttr.isInvincible) return;
            _playerAttr.ReceiveHealing(damage * healingFactor);
        }
    }
}
