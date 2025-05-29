using System;
using System.Collections;
using DataSO;
using Props;
using Snowman;
using UISystem;
using UnityEngine;
using UnityEngine.Serialization;
using Utilities;
using EventHandler = Utilities.EventHandler;

namespace Player
{
    /*
     * Handle player's input
     */
    public class PlayerController : MonoBehaviour
    {
        [Header("Player Parameters")]
        public float rotationSpeed;
        public float stopAttackTimer;
        public float atkMovingSpeedFactor;
        public float dashTimer;
        public float dashSpeedFactor;
        public float dashCost;
        [Header("Player State")]
        public bool isRollingSnowball;
        public bool isAttacking;
        public bool canAttack;
        public bool hasRollingSnowball;
        public bool enableDash;
        [FormerlySerializedAs("isDashing")] public bool isDashPressed;
        [FormerlySerializedAs("isDashReady")] public bool isDashing;
        
        // private PlayerSO _playerSO;
        private InputControls _inputControls;
        private Vector2 _moveInput;
        private Vector2 _mousePosition;
        private Rigidbody _rb;
        private Camera _camera;
        private Coroutine _staminaCoroutine;
        private Coroutine _dashCoroutine;

        private ThrowSnowball _throwSnowballScript;
        private RollSnowball _rollSnowballScript;
        private PlayerAttribute _playerAttr;
        private SummonSnowman _summonSnowmanScript;
        private SkillPanel _skillPanelScript;
        private GameObject _currentInteractableObject;

        private float _initMovingSpeed;
        private float _movingSpeed;
        private int _panelAmount;

        public Animator animator;
        public PlayerSfxController sfxController;
        private Vector3 _moveDir, _faceDir;
        private bool _isDead;
        
        private void Awake()
        {
            // _playerSO = Resources.Load<PlayerSO>("DataSO/Player_SO");
            _inputControls = new InputControls();
            _rb = GetComponent<Rigidbody>();
            _throwSnowballScript = GetComponent<ThrowSnowball>();
            _rollSnowballScript = GetComponent<RollSnowball>();
            _playerAttr = GetComponent<PlayerAttribute>();
            _summonSnowmanScript = GetComponent<SummonSnowman>();
            _skillPanelScript = GameObject.FindWithTag("SkillPanel").GetComponent<SkillPanel>();
            _camera = Camera.main;

            // _initMovingSpeed = _playerAttr.speed;
            // _movingSpeed = _initMovingSpeed;

            _inputControls.Gameplay.Move.performed += context => _moveInput = context.ReadValue<Vector2>();
            _inputControls.Gameplay.Move.canceled += _ => _moveInput = Vector2.zero;
            _inputControls.Gameplay.MousePosition.performed += context => _mousePosition = context.ReadValue<Vector2>();
            _inputControls.Gameplay.ThrowSnowball.performed += _ => OnThrowingSnowballStart();
            _inputControls.Gameplay.ThrowSnowball.canceled += _ => OnThrowingSnowballEnd();
            _inputControls.Gameplay.RollSnowball.performed += _ => OnRollingSnowballStart();
            _inputControls.Gameplay.RollSnowball.canceled += _ => OnRollingSnowballEnd();
            _inputControls.Gameplay.SwitchSnowmanLeft.performed += _ => OnSwitchSnowmanLeft();
            _inputControls.Gameplay.SwitchSnowmanRight.performed += _ => OnSwitchSnowmanRight();
            _inputControls.Gameplay.SummonSnowman.performed += _ => OnSummonSnowman();
            _inputControls.Gameplay.Interact.performed += _ => OnPressInteractButton();
            _inputControls.Gameplay.Rush.performed += _ => OnPressDashButton();
            _inputControls.Gameplay.Rush.canceled += _ => OnReleaseDashButton();
            _inputControls.Gameplay.ThrowSnowballPress.performed += _=> OnPressingThrow();
            _inputControls.Gameplay.ThrowSnowballPress.canceled += _=> OnReleasingThrow();
            _inputControls.Gameplay.Retreat.performed += _ => EventHandler.RetreatSnowman();

            _rollSnowballScript.enabled = false;
            canAttack = true;
            enableDash = true;
        }

        private void OnEnable()
        {
            _inputControls.Enable();
            EventHandler.OnSetGameplayActionMap += SetGameplayActionMode;
            EventHandler.OnAllowInputControl += AllowInputControl;
            EventHandler.OnAllowMouseInput += AllowMouseInput;
            EventHandler.OnAddSnowman += AddSnowman;
        }

        private void OnDisable()
        {
            _inputControls.Disable();
            EventHandler.OnSetGameplayActionMap -= SetGameplayActionMode;
            EventHandler.OnAllowInputControl -= AllowInputControl;
            EventHandler.OnAllowMouseInput -= AllowMouseInput;
            EventHandler.OnAddSnowman -= AddSnowman;
        }

        private void Start()
        {
            _initMovingSpeed = _playerAttr.speed;
            _movingSpeed = _initMovingSpeed;
        }

        private void Update()
        {
            // canAttack = _playerAttr.stamina >= 10;

            if (isAttacking)
            {
                _movingSpeed = _initMovingSpeed * atkMovingSpeedFactor;
            }
            else
            {
                _movingSpeed = _initMovingSpeed;
            }

            if (!isDashing)
            {
                EndDash();
                // StartStaminaCoroutine();
            }

            var angle = Vector3.Angle(_moveDir, _faceDir);
            // animator.SetFloat("Speed", _moveDir);
            animator.SetFloat(PlayerAnimationPara.Angle.ToString(), angle);

            if (_playerAttr.health <= 0 && !_isDead)
            {
                animator.SetTrigger(PlayerAnimationPara.IsDead.ToString());
                _isDead = true;
            }

            if (_playerAttr.stamina < Mathf.Abs(_throwSnowballScript.stamina) && animator.GetBool(PlayerAnimationPara.IsPressing.ToString()))
            {
                OnReleasingThrow();
            }
        }

        private void FixedUpdate()
        {
            var moveDirection = new Vector3(_moveInput.x, 0, _moveInput.y).normalized;
            var currentVerticalVelocity = _rb.velocity.y;
            _moveDir = moveDirection;

            if (!isDashPressed) _rb.velocity = new Vector3(moveDirection.x * _movingSpeed, currentVerticalVelocity, moveDirection.z * _movingSpeed);
            // Debug.Log(_rb.velocity + " speed " + _movingSpeed);
            RotateTowardsMouse();

            _throwSnowballScript.aimingLineRenderer.enabled = (_playerAttr.isInCombat || isAttacking) && !isRollingSnowball;
            if (!isRollingSnowball)
            {
                // RotateTowardsMouse();
                _throwSnowballScript.UpdateAimingLine();
            }
            else
            {
                // if (moveDirection != Vector3.zero)
                // {
                //     RotateTowardMovingDirection(moveDirection);
                // }
                _rollSnowballScript.UpdateSnowball(moveDirection);
            }

            if (_playerAttr.stamina < _playerAttr.maxStamina && !isAttacking)
            {
                var deltaStamina = Time.deltaTime * _playerAttr.staminaRecovery;
                _playerAttr.stamina += deltaStamina;
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Chest"))
            {
                _currentInteractableObject = other.gameObject;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.CompareTag("Chest"))
            {
                _currentInteractableObject = null;
            }
        }

        public static void PlayerDies()
        {
            EventHandler.PlayerDie();
        }

        /*
         * Rotate player towards mouse direction
         */
        private void RotateTowardsMouse()
        {
            var ray = _camera.ScreenPointToRay(_mousePosition);
            var layerMask = 1 << LayerMask.NameToLayer("Base") | 1 << LayerMask.NameToLayer("UI");

            if (!Physics.Raycast(ray, out var hit, Mathf.Infinity, layerMask)) return;
            var target = hit.point;
            var trans = transform.position;
            target.y = trans.y;
            var direction = (target - trans).normalized;
            _faceDir = direction;
            var lookRotation = Quaternion.LookRotation(direction);
            _rb.rotation = Quaternion.Slerp(_rb.rotation, lookRotation, Time.fixedDeltaTime * rotationSpeed);
        }
        
        /*
         * Rotate player towards moving direction
         */
        private void RotateTowardMovingDirection(Vector3 moveDir)
        {
            var targetRotation = Quaternion.LookRotation(moveDir);
            _rb.rotation = Quaternion.Slerp(_rb.rotation, targetRotation, Time.fixedDeltaTime * rotationSpeed);
        }

        /*
         * Start throwing snowball
         */
        private void OnThrowingSnowballStart()
        {
            if (!canAttack) return;
            if (_playerAttr.stamina < Mathf.Abs(_throwSnowballScript.stamina)) return;
            StopStaminaCoroutine();
            // _throwSnowballScript.Attack();
            isAttacking = true;
            animator.SetTrigger(PlayerAnimationPara.IsThrowing.ToString());
            canAttack = false;
        }

        /*
         * Stop throwing snowball
         */
        private void OnThrowingSnowballEnd()
        {
            StartStaminaCoroutine();
            canAttack = true;
            // animator.SetBool(PlayerAnimationPara.IsThrowing.ToString(), false);
        }

        private void OnPressingThrow()
        {
            // if (!canAttack) return;
            if (_playerAttr.stamina < Mathf.Abs(_throwSnowballScript.stamina)) return;
            StopStaminaCoroutine();
            // _throwSnowballScript.Attack();
            isAttacking = true;
            animator.SetBool(PlayerAnimationPara.IsPressing.ToString(), true);
            canAttack = false;
        }

        private void OnReleasingThrow()
        {
            StartStaminaCoroutine();
            animator.SetBool(PlayerAnimationPara.IsPressing.ToString(), false);
            // canAttack = true;
        }

        public void ThrowingSnowballAttack()
        {
            _throwSnowballScript.Attack();
            sfxController.PlayAudio(PlayerSfxType.Throw);
        }

        /*
         * Start rolling snowball
         */
        private void OnRollingSnowballStart()
        {
            _throwSnowballScript.enabled = false;
            _rollSnowballScript.enabled = true;
            if (!canAttack) return;
            if (_playerAttr.stamina < Mathf.Abs(_rollSnowballScript.stamina)) return;
            StopStaminaCoroutine();
            
            _rollSnowballScript.showRollingLine = true;
            isRollingSnowball = true;
            
            
            // isRollingSnowball = true;
            // _rollSnowballScript.CreateSnowball();

            isAttacking = true;
            
            animator.SetBool(PlayerAnimationPara.IsRolling.ToString(), true);
        }

        /*
         * Stop rolling snowball
         */
        public void OnRollingSnowballEnd()
        {
            
            _rollSnowballScript.Attack();
            _rollSnowballScript.showRollingLine = false;
            
            _throwSnowballScript.enabled = true;
            _rollSnowballScript.enabled = false;
            canAttack = true;
            hasRollingSnowball = false;
            isRollingSnowball = false;
            
            StartStaminaCoroutine();
            
            animator.SetBool(PlayerAnimationPara.IsRolling.ToString(), false);
        }

        public void CreateRollingSnowball()
        {
            if (hasRollingSnowball) return;
            _rollSnowballScript.CreateSnowball();
            hasRollingSnowball = true;
        }

        /*
         * Recover stamina after a specific time
         */
        private IEnumerator StopAttackAfterSeconds()
        {
            yield return new WaitForSeconds(stopAttackTimer);
            // while (_playerAttr.stamina < _playerSO.maxStamina)
            // {
            //     // _playerAttr.stamina += _playerSO.staminaRecovery * Time.deltaTime;
            //     isAttacking = false;
            //     yield return null;
            // }
            isAttacking = false;
        }

        /*
         * Start stamina recovery coroutine
         */
        private void StartStaminaCoroutine()
        {
            _staminaCoroutine ??= StartCoroutine(StopAttackAfterSeconds());
        }

        /*
         * Stop stamina recovery coroutine
         */
        private void StopStaminaCoroutine()
        {
            if (_staminaCoroutine != null)
            {
                StopCoroutine(_staminaCoroutine);
                _staminaCoroutine = null;
            }
        }

        /*
         * Summon snowman
         */
        private void OnSummonSnowman()
        {
            _summonSnowmanScript.SummonCurrentSnowman();
            sfxController.PlayAudio(PlayerSfxType.Summon);
        }

        /*
         * Press Q to switch snowman
         */
        private void OnSwitchSnowmanLeft()
        {
            //moving direction is reverse to switch direction
            if (_skillPanelScript.isMoving) return;
            
            // _skillPanelScript.MoveIconsRight();
            sfxController.PlayAudio(PlayerSfxType.Switch);
            _summonSnowmanScript.SwitchSnowmanRight();
            _skillPanelScript.UpdateSkill();
        }
        
        /*
         * Press E to switch snowman
         */
        private void OnSwitchSnowmanRight()
        {
            //moving direction is reverse to switch direction
            if (_skillPanelScript.isMoving) return;
            
            // _skillPanelScript.MoveIconsLeft();
            sfxController.PlayAudio(PlayerSfxType.Switch);
            _summonSnowmanScript.SwitchSnowmanLeft();
            _skillPanelScript.UpdateSkill();
        }

        /*
         * Press F to interact with interactable objects
         */
        private void OnPressInteractButton()
        {
            if (_currentInteractableObject == null || _playerAttr.isInCombat) return;
            
            if (_currentInteractableObject.CompareTag("Chest"))
            {
                var chestScript = _currentInteractableObject.GetComponent<TreasureChest>();
                if (!chestScript.canOpen) return;
                // _skillPanelScript.ResetIconsPosition();
                chestScript.OpenChest();
                // _summonSnowmanScript.currentIndex = 0;
                // _summonSnowmanScript.LoadSnowmanPrefab();
                // // _skillPanelScript.UpdateSkill();
                // EventHandler.UpdateSkillPanel();
                // EventHandler.OpenSnowmanObtainedPrompt(chestScript.snowman);
            }
        }

        private void AddSnowman(SnowmanTypeAndLevel snowman)
        {
            _summonSnowmanScript.currentIndex = 0;
            _summonSnowmanScript.LoadSnowmanPrefab();
            // _skillPanelScript.UpdateSkill();
            EventHandler.UpdateSkillPanel();
            EventHandler.OpenSnowmanObtainedPrompt(snowman);
        }

        private void OnPressDashButton()
        {
            if (!enableDash) return;
            if (isDashPressed || _playerAttr.stamina < dashCost) return;
            isDashPressed = true;
            // isAttacking = true;
            _playerAttr.stamina -= dashCost;
            sfxController.PlayAudio(PlayerSfxType.Dash);
            StartDash();
            // StopStaminaCoroutine();
        }

        private void OnReleaseDashButton()
        {
            // StartStaminaCoroutine();
            isDashPressed = false;
        }

        private void StartDash()
        {
            _dashCoroutine ??= StartCoroutine(Dash());
        }

        private void EndDash()
        {
            if (_dashCoroutine == null) return;
            StopCoroutine(_dashCoroutine);
            _dashCoroutine = null;
        }

        private IEnumerator Dash()
        {
            isDashing = true;
            var startTime = Time.time;

            while (Time.time < startTime + dashTimer)
            {
                var moveDirection = new Vector3(_moveInput.x, 0, _moveInput.y).normalized;
                // var moveDirection = transform.forward.normalized;
                _rb.velocity = moveDirection * (_initMovingSpeed * dashSpeedFactor);
                yield return null;
            }
            
            _rb.velocity = Vector3.zero;
            // isDashing = false;
            isDashing = false;
        }

        private void SetGameplayActionMode(bool isActive)
        {
            if (isActive) _panelAmount--;
            else _panelAmount++;
            
            if (_panelAmount < 1)_inputControls.Enable();
            else _inputControls.Disable();
        }

        private void AllowInputControl(bool allow)
        {
            if (allow) _inputControls.Enable();
            else _inputControls.Disable();
        }

        private void AllowMouseInput(bool allow)
        {
            if (allow)
            {
                _inputControls.Gameplay.ThrowSnowball.Enable();
                _inputControls.Gameplay.RollSnowball.Enable();
            }
            else
            {
                _inputControls.Gameplay.ThrowSnowball.Disable();
                _inputControls.Gameplay.RollSnowball.Disable();
            }
        }
    }
}
