using System;
using System.Diagnostics;
using Cinemachine;
using Player;
using UnityEngine;

namespace Utilities
{
    public class CameraController : MonoBehaviour
    {
        public CinemachineVirtualCamera virCamera;
        private Transform _playerTrans;
        private PlayerAttribute _playerAttr;
        public float normalFOV = 30f; 
        public float combatFOV = 45f;
        public float narrativeFOV = 30f;
        private const float LerpSpeed = 2f;
        private FovType _fovType;
        private float _currentFov;

        private void Awake()
        {
            var player = GameObject.FindWithTag("Player");
            _playerTrans = player.transform;
            _playerAttr = player.GetComponent<PlayerAttribute>();
            virCamera.Follow = _playerTrans;
            _currentFov = normalFOV;
        }

        private void OnEnable()
        {
            EventHandler.OnChangeFOV += ChangeFov;
        }

        private void OnDisable()
        {
            EventHandler.OnChangeFOV -= ChangeFov;
        }

        private void Update()
        {
            // if (_playerAttr.isInCombat) _fovType = FovType.Battle;
            // else _fovType = FovType.Normal;
            // var targetFOV = _playerAttr.isInCombat ? combatFOV : normalFOV;
            virCamera.m_Lens.FieldOfView = Mathf.Lerp(virCamera.m_Lens.FieldOfView, _currentFov, LerpSpeed * Time.deltaTime);
        }

        private void ChangeFov(FovType fov)
        {
            _currentFov = fov switch
            {
                FovType.Normal => normalFOV,
                FovType.Battle => combatFOV,
                FovType.Narrative => narrativeFOV,
                _ => normalFOV
            };
        }
    }
}
