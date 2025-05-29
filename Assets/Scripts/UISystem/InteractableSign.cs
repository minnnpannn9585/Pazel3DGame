using System;
using TMPro;
using UnityEngine;
using EventHandler = Utilities.EventHandler;

namespace UISystem
{
    public sealed class InteractableSign : MonoBehaviour
    {
        public RectTransform hud;
        public TextMeshProUGUI hudText;
        public Transform trans;
        public Vector3 offset;
        private Camera _mainCamera;

        private void Awake()
        {
            _mainCamera = Camera.main;
        }

        private void OnEnable()
        {
            EventHandler.OnShowInteractableSign += SetHud;
        }

        private void OnDisable()
        {
            EventHandler.OnShowInteractableSign -= SetHud;
        }

        private void FixedUpdate()
        {
            var worldPosition = trans.position + offset;
            var screenPosition = _mainCamera.WorldToScreenPoint(worldPosition);
            
            hud.position = screenPosition;
        }

        private void SetHud(bool isActive, string text)
        {
            hud.gameObject.SetActive(isActive);
            hudText.text = text;
        }
    }
}
