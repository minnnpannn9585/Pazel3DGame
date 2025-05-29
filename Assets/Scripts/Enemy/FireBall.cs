using System;
using System.Collections.Generic;
using Player;
using Snowman;
using UnityEngine;

namespace Enemy
{
    public class FireBall : MonoBehaviour
    {
        public float speed;

        private Vector3 _direction;
        private float _attack;

        public bool isAdvanced;
        public List<Transform> transList;
        public GameObject fireball;
        public GameObject vfx;
        
        private void FixedUpdate()
        {
            transform.Translate(_direction * (speed * Time.fixedDeltaTime), Space.World);
            
            if (_direction != Vector3.zero)
            {
                // 创建一个旋转，使得物体的前方向朝向_direction
                var toRotation = Quaternion.LookRotation(_direction);
                // 可以直接设置旋转，或者使用Quaternion.Lerp或Quaternion.Slerp来平滑过渡
                transform.rotation = Quaternion.Lerp(transform.rotation, toRotation, 2f * Time.fixedDeltaTime);
            }
        }

        private void OnCollisionEnter(Collision other)
        {
            var otherGO = other.gameObject;
            if (otherGO.CompareTag("Projectile")) return;
            
            if (otherGO.CompareTag("Player"))
            {
                otherGO.GetComponent<PlayerAttribute>().TakeDamage(_attack);
            }

            if (otherGO.CompareTag("Snowman"))
            {
                otherGO.GetComponent<SnowmanTakeDamage>().TakeDamage(_attack);
            }

            if (isAdvanced)
            {
                foreach (var t in transList)
                {
                    var smallFireBall = Instantiate(fireball, t.position, Quaternion.identity);
                    smallFireBall.GetComponent<FireBall>().SetFireBall(t.forward, _attack*0.2f);
                }
            }

            Instantiate(vfx, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }

        public void SetFireBall(Vector3 direction, float attack)
        {
            _direction = direction;
            _attack = attack;
        }
    }
}
