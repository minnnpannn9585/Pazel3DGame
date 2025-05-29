using UnityEngine;

namespace Player
{
    /*
     * Super class for handle snowball attack
     */
    public class SnowballAttack : MonoBehaviour
    {
        [Header("SnowballAttack Component Settings")]
        public GameObject snowballPrefab;
        public Transform startPosition;
        public LineRenderer aimingLineRenderer;
        public int lineSegmentCount = 20;
        [Header("SnowballAttack Parameters")]
        public float force;
        public float stamina;

        protected GameObject SnowballInstance;
        protected PlayerAttribute PlayerAttr;
        
        private Rigidbody _snowballRb;

        protected virtual void Awake()
        {
            PlayerAttr = GetComponent<PlayerAttribute>();    
        }

        /*
         * Create snowball
         */
        public virtual void CreateSnowball()
        {
            SnowballInstance = Instantiate(snowballPrefab, startPosition.position, startPosition.rotation);
            _snowballRb = SnowballInstance.GetComponent<Rigidbody>();
        }

        /*
         * Make snowball move forward and clean cache
         */
        public virtual void Attack()
        {
            var forceDir = startPosition.forward;
            _snowballRb.AddForce(forceDir * force, ForceMode.Impulse);
            
            CleanCache();
        }

        /*
         * Clean snowball instance and rigidbody caches
         */
        private void CleanCache()
        {
            SnowballInstance = null;
            _snowballRb = null;
        }

        /*
         * Update aiming line
         */
        public virtual void UpdateAimingLine(){}
    }
}
