using System;
using UnityEngine;
using UnityEngine.UI;

namespace UISystem
{
    /*
     * Control NPC's HUD
     */
    public class NpcHUD : MonoBehaviour
    {        
        public Transform trans;
        public Vector3 offset;
        public RectTransform hudPanel;
        public Image fillImage1;
        public Image fillImage2;
        public float lerpSpeed = 5f;
        
        protected float FillPercentage1;
        protected float FillPercentage2;
        
        private Camera _mainCamera;

        protected virtual void Awake()
        {
            _mainCamera = Camera.main;
            SetPosition();
        }

        protected virtual void Update()
        {
            if (fillImage1 == null || fillImage2 == null) return;
            fillImage1.fillAmount = Mathf.Lerp(fillImage1.fillAmount, FillPercentage1, lerpSpeed * Time.deltaTime);
            fillImage2.fillAmount = Mathf.Lerp(fillImage2.fillAmount, FillPercentage2, lerpSpeed * Time.deltaTime);
        }

        private void FixedUpdate()
        {
            SetPosition();
        }

        private void SetPosition()
        {
            var worldPosition = trans.position + offset;
            var screenPosition = _mainCamera.WorldToScreenPoint(worldPosition);
            
            hudPanel.position = screenPosition;
        }
    }
}
