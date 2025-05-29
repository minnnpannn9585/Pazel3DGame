using System;
using Snowball;
using UnityEngine;
using Utilities;

namespace Player
{
    /*
     * The action of rolling snowball
     */
    public class RollSnowball : SnowballAttack
    {
        [Header("RollSnowball Component Settings")]
        public GameObject rollingLine;
        public float attackBonusFactor;
        [Header("RollSnowball Parameters")]
        public float scaleFactor;
        public float staminaIncrease;
        public bool showRollingLine;
        private float _attackBonus;
        private PlayerController _playerController;
        
        private RollingSnowball _rollingSnowballScript;

        protected override void Awake()
        {
            base.Awake();
            _playerController = GetComponent<PlayerController>();
        }

        private void Update()
        {
            if (showRollingLine) UpdateRollingLine();
        }


        private void OnDisable()
        {
            rollingLine.SetActive(false);
        }

        /*
         * Create rolling snowball
         */
        public override void CreateSnowball()
        {
            if (PlayerAttr.stamina < Mathf.Abs(stamina)) return;
            
            base.CreateSnowball();
            
            _rollingSnowballScript = SnowballInstance.GetComponent<RollingSnowball>();
            PlayerAttr.stamina += stamina;
            _rollingSnowballScript.SetAttack(PlayerAttr.attack + _attackBonus);
        }

        /*
         * Make snowball move forward
         */
        public override void Attack()
        {
            if (SnowballInstance == null)
            {
                // Debug.Log("snowball null");
                // _rollingSnowballScript.SetAttack(PlayerAttr.attack + _attackBonus);
                enabled = false;
                return;
            }
            
            // _rollingSnowballScript.SetAttack(PlayerAttr.attack + _attackBonus);
            _rollingSnowballScript.SetAttack(PlayerAttr.attack + _attackBonus);
            _rollingSnowballScript.SetReleasingState();
            base.Attack();
            _attackBonus = 0;
            _playerController.sfxController.StopAudio(PlayerSfxType.Roll);
            // Debug.Log(PlayerAttr.stamina);
        }

        /*
         * Scale snowball and cost stamina, also update the aiming line
         */
        public void UpdateSnowball(Vector3 moveDir)
        {
            if (SnowballInstance == null)
            {
                // enabled = false;
                _playerController.sfxController.StopAudio(PlayerSfxType.Roll);
                return;
            }
            // UpdateRollingLine();
            if (moveDir != Vector3.zero)
            {
                var scaleIncrease = new Vector3(scaleFactor, scaleFactor, scaleFactor) * Time.fixedDeltaTime;
                SnowballInstance.transform.localScale += scaleIncrease;
                _attackBonus += (attackBonusFactor * PlayerAttr.attack * Time.fixedDeltaTime);
                // _rollingSnowballScript.SetAttack(PlayerAttr.attack + _attackBonus);
                if (!_playerController.sfxController.GetAudioState(PlayerSfxType.Roll)) 
                    _playerController.sfxController.PlayAudio(PlayerSfxType.Roll);

                if (PlayerAttr.stamina >= Mathf.Abs(staminaIncrease * Time.fixedDeltaTime))
                {
                    PlayerAttr.stamina += staminaIncrease * Time.fixedDeltaTime;
                }
                else
                {
                    Attack();
                }
            }
            else
            {
                if (_playerController.sfxController.GetAudioState(PlayerSfxType.Roll)) 
                    _playerController.sfxController.PauseAudio(PlayerSfxType.Roll);
            }

            if (SnowballInstance == null) return;
            SnowballInstance.transform.position = startPosition.position;
            SnowballInstance.transform.rotation = startPosition.rotation;
            
        }

        private void UpdateRollingLine()
        {
            if (!rollingLine.activeInHierarchy) rollingLine.SetActive(true);
            // const float distance = RollingSnowball.RollingDistance - 5f;
            rollingLine.transform.localScale = new Vector3(PlayerAttr.transform.localScale.x,1,10f);
            rollingLine.transform.position = startPosition.position;
            rollingLine.transform.rotation = startPosition.rotation;
            // Debug.Log("line");
        }
    }
}
