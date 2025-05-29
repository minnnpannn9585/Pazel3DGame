using System;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace Enemy
{
    public class FlameRays : MonoBehaviour
    {
        // public BaseEnemy enemyAttr;
        private Transform _enemyTrans;
        private float _rotateAngle;
        public float attack;
        public float duration;

        private void Awake()
        {
            if (Random.Range(0, 1) < 0.5) _rotateAngle = 60f;
            else _rotateAngle = -60f;
        }

        private void FixedUpdate()
        {
            // var yRotation = -enemyAttr.transform.rotation.y;
            // // transform.rotation = Quaternion.Euler(0f, yRotation, 0f);
            if (_enemyTrans == null) Destroy(gameObject);
            transform.position = _enemyTrans.position;
            transform.Rotate(0f, _rotateAngle * Time.fixedDeltaTime, 0f);
        }

        public void SetFlameRays(Transform followedTrans, float rayAttack, float rayDuration)
        {
            _enemyTrans = followedTrans;
            attack = rayAttack;
            duration = rayDuration;
        }

        public void DestroyMe()
        {
            Destroy(gameObject);
        }
    }
}
