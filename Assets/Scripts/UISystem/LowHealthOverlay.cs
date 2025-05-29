using System;
using UnityEngine;
using UnityEngine.UI;

namespace UISystem
{
    public class LowHealthOverlay : MonoBehaviour
    {
        private Image _image;
        public float blinkSpeed = 2f;

        private void Awake()
        {
            _image = GetComponent<Image>();
        }

        private void Update()
        {
            var alpha = (Mathf.Sin(Time.time * blinkSpeed) * 0.4f);
            _image.color = new Color(1, 0, 0, alpha+0.6f);
        }
    }
}
