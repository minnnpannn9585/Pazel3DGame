using System;
using TMPro;
using UnityEngine;

namespace Utilities
{
    public class MiniMap : MonoBehaviour
    {
        private InputControls _inputControls;
        private RectTransform _rectTrans;
        private Transform _playerTrans;
        public Transform sceneTrans;
        public bool isLarge;
        public MiniMapCamera miniCamera;
        
        private bool _isScaling = false;
        private Vector2 _targetAnchoredPosition;
        private Vector2 _targetSizeDelta;
        private const float TransitionSpeed = 5f;

        private void Awake()
        {
            _rectTrans = GetComponent<RectTransform>();
            _playerTrans = GameObject.FindWithTag("Player").transform;
            miniCamera.SetCamera(_playerTrans, 50);
            
            _inputControls = new InputControls();
            _inputControls.Gameplay.Map.performed += _=> ScaleMap();
        }

        private void OnEnable()
        {
            _inputControls.Enable();
        }

        private void OnDisable()
        {
            _inputControls.Disable();
        }

        private void Update()
        {
            if (_isScaling)
            {
                // 使用Lerp逐渐改变位置和大小
                _rectTrans.anchoredPosition = Vector2.Lerp(_rectTrans.anchoredPosition, _targetAnchoredPosition, TransitionSpeed * Time.deltaTime);
                _rectTrans.sizeDelta = Vector2.Lerp(_rectTrans.sizeDelta, _targetSizeDelta, TransitionSpeed * Time.deltaTime);
                miniCamera.gameObject.SetActive(false);
                // 检查是否已接近目标值，如果是，则停止过渡
                if (Vector2.Distance(_rectTrans.anchoredPosition, _targetAnchoredPosition) < 1f && Vector2.Distance(_rectTrans.sizeDelta, _targetSizeDelta) < 1f)
                {
                    _rectTrans.anchoredPosition = _targetAnchoredPosition; // 确保最终值精确
                    _rectTrans.sizeDelta = _targetSizeDelta;
                    _isScaling = false;
                    miniCamera.gameObject.SetActive(true);
                    if (isLarge) miniCamera.SetCamera(sceneTrans, 100);
                    else miniCamera.SetCamera(_playerTrans, 50);
                }
            }
            
            // miniCamera.gameObject.SetActive(!isScaling);
        }

        private void ScaleMap()
        {
            // isLarge = !isLarge;
            // if (isLarge)
            // {
            //     _rectTrans.anchoredPosition = new Vector2(500, 0);
            //     _rectTrans.sizeDelta = new Vector2(1000, 1000);
            //     miniCamera.SetCamera(sceneTrans, 100);
            // }
            // else
            // {
            //     _rectTrans.anchoredPosition = new Vector2(0, 0);
            //     _rectTrans.sizeDelta = new Vector2(300, 300);
            //     miniCamera.SetCamera(_playerTrans, 50);
            // }
            
            isLarge = !isLarge;
            if (isLarge)
            {
                _targetAnchoredPosition = new Vector2(500, 0);
                _targetSizeDelta = new Vector2(1000, 1000);
                // miniCamera.SetCamera(sceneTrans, 100);
            }
            else
            {
                _targetAnchoredPosition = new Vector2(0, 0);
                _targetSizeDelta = new Vector2(300, 300);
                // miniCamera.SetCamera(_playerTrans, 50);
            }
            _isScaling = true;
        }
    }
}
