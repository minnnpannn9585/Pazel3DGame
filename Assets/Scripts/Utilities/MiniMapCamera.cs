using System;
using UnityEngine;

namespace Utilities
{
    public class MiniMapCamera : MonoBehaviour
    {
        // private Transform _playerTrans;
        public Transform tarTrans;
        private Camera _myCamera;
        private float _targetOrthographicSize;
        private const float TransitionSpeed = 2f;

        private void Awake()
        {
            _myCamera = GetComponent<Camera>();
        }

        // private void FixedUpdate()
        // {
        //     transform.position = tarTrans.position;
        // }
        
        private void FixedUpdate()
        {
            if (tarTrans != null)
            {
                // 使用Lerp逐渐改变位置
                transform.position = Vector3.Lerp(transform.position, tarTrans.position, TransitionSpeed * Time.fixedDeltaTime);
            }
        }

        private void Update()
        {
            if (_myCamera != null)
            {
                // 使用Lerp逐渐改变相机的orthographicSize
                _myCamera.orthographicSize = Mathf.Lerp(_myCamera.orthographicSize, _targetOrthographicSize, TransitionSpeed * Time.deltaTime);
            }
        }

        public void SetCamera(Transform tar, float distance)
        {
            // tarTrans = tar;
            // _myCamera.orthographicSize = distance;
            
            tarTrans = tar;
            _targetOrthographicSize = distance;
        }
    }
}
