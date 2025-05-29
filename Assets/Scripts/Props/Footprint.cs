using System;
using UnityEngine;

namespace Props
{
    public class Footprint : MonoBehaviour
    {
        private SpriteRenderer _spriteRenderer;
        public float fadeRate = 0.1f;

        private void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }

        private void FixedUpdate()
        {
            var color = _spriteRenderer.color;
            color.a -= fadeRate * Time.fixedDeltaTime; // 减少透明度
            _spriteRenderer.color = color;

            // 当透明度降到很低时销毁物体
            if (_spriteRenderer.color.a <= 0.05f)
            {
                Destroy(transform.parent.gameObject);
            }
        }
    }
}
