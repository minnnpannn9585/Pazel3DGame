using System;
using System.Collections.Generic;
using Cinemachine;
using DataSO;
using Snowman;
using UnityEngine;
using Utilities;
using EventHandler = Utilities.EventHandler;

namespace Player
{
    /*
     * Store and handle player's attributes
     */
    public class PlayerAttribute : MonoBehaviour
    {
        private PlayerSO _playerSO;
        private GameSO _gameSO;

        public float maxHealth;
        public float maxStamina;
        public float maxMana;
        public float speed;
        public float staminaRecovery;
        
        public float health;
        public float stamina;
        public float mana;
        public float attack;
        public float manaRecovery;
        public bool isLowHealth;

        public List<SnowmanInfo> snowmanList;

        public bool isInvincible;

        public List<GameObject> enemiesInCombat;
        public bool isInCombat;
        private CinemachineImpulseSource _hurtImpulseSource;
        private PlayerController _playerController;

        private void Awake()
        {
            _gameSO = Resources.Load<GameSO>("DataSO/Game_SO");
            _playerSO = _gameSO.currentGameData.playerSo;
            _hurtImpulseSource = GetComponent<CinemachineImpulseSource>();
            _playerController = GetComponent<PlayerController>();
            // maxHealth = _playerSO.maxHealth;
            // maxStamina = _playerSO.maxStamina;
            // maxMana = _playerSO.maxMana;
            //
            // health = _playerSO.currentHealth;
            // stamina = _playerSO.currentStamina;
            // mana = _playerSO.currentMana;
            // // mana = 0;
            // attack = _playerSO.attack;
            // manaRecovery = _playerSO.manaRecovery;
            // staminaRecovery = _playerSO.staminaRecovery;
            // speed = _playerSO.speed;
            InitializeAttributes(_playerSO);
            
            LoadSnowmanList();
            
            // _playerSO.SaveData();
            if (_gameSO.currentGameData.levelSo.respawnAtThisPosition)
            {
                transform.position = _gameSO.currentGameData.levelSo.position;
                _gameSO.currentGameData.levelSo.respawnAtThisPosition = false;
            }
            
            _gameSO.SaveData();
        }

        public void InitializeAttributes(PlayerSO playerSO)
        {
            maxHealth = playerSO.maxHealth;
            maxStamina = playerSO.maxStamina;
            maxMana = playerSO.maxMana;
            
            health = playerSO.currentHealth;
            stamina = playerSO.currentStamina;
            mana = playerSO.currentMana;
            // mana = 0;
            attack = playerSO.attack;
            manaRecovery = playerSO.manaRecovery;
            staminaRecovery = playerSO.staminaRecovery;
            speed = playerSO.speed;
        }

        private void OnEnable()
        {
            EventHandler.OnOpenSnowmanChest += AddSnowmanToPlayer;
            EventHandler.OnAddEnemyToCombatList += enemy => enemiesInCombat.Add(enemy);
            EventHandler.OnRemoveEnemyToCombatList += enemy => enemiesInCombat.Remove(enemy);
            EventHandler.OnChangePlayerBattleState += HandleBattleState;
        }
        
        private void OnDisable()
        {
            EventHandler.OnOpenSnowmanChest -= AddSnowmanToPlayer;
            EventHandler.OnChangePlayerBattleState -= HandleBattleState;
        }

        private void Update()
        {
            stamina = Mathf.Clamp(stamina, 0, maxStamina);
            health = Mathf.Clamp(health, 0, maxHealth);
            mana = Mathf.Clamp(mana, 0, maxMana);

            if (!isLowHealth && health <= maxHealth * 0.3f)
            {
                isLowHealth = true;
                EventHandler.HandleLowHealth(true);
            }

            if (isLowHealth && health > maxHealth * 0.3f)
            {
                isLowHealth = false;
                EventHandler.HandleLowHealth(false);
            }
            // if (health <= 0) EventHandler.PlayerDie();

            // switch (isInCombat)
            // {
            //     case false when enemiesInCombat.Count > 0:
            //         EventHandler.ChangeFOV(FovType.Battle);
            //         isInCombat = true;
            //         break;
            //     case true when enemiesInCombat.Count < 1:
            //         EventHandler.ChangeFOV(FovType.Normal);
            //         isInCombat = false;
            //         break;
            // }
            // isInCombat = enemiesInCombat.Count > 0;
        }

        private void HandleBattleState(bool isInBattle)
        {
            if (isInBattle)
            {
                EventHandler.ChangeFOV(FovType.Battle);
                isInCombat = true;
            }
            else
            {
                EventHandler.ChangeFOV(FovType.Normal);
                isInCombat = false;
            }
        }

        private void FixedUpdate()
        {
            foreach (var snowman in snowmanList)
            {
                if (!snowman.canBeSummoned)
                {
                    if (snowman.cooldownTimer > 0) snowman.cooldownTimer -= Time.fixedDeltaTime;
                    else
                    {
                        snowman.canBeSummoned = true;
                    }
                }

                snowman.cooldownTimer = Mathf.Clamp(snowman.cooldownTimer, 0, snowman.cooldown);
            }
        }

        /*
         * Load and fresh snowman list from player scriptable object
         */
        private void LoadSnowmanList()
        {
            snowmanList.Clear();
            for (var i = 0; i < _playerSO.snowmanList.Count; i++)
            {
                var snowmanTypeAndLevel = _playerSO.snowmanList[i];
                var snowmanSO = Resources.Load<SnowmanSO>("DataSO/SnowmanSO/" + snowmanTypeAndLevel.type + "_SO");
                
                snowmanList.Add(new SnowmanInfo());
                snowmanList[i].type = snowmanTypeAndLevel.type;
                snowmanList[i].level = snowmanTypeAndLevel.level;
                if (snowmanSO == null) continue;
                snowmanList[i].cooldown = snowmanSO.cooldown;
                snowmanList[i].cooldownTimer = 0;
                snowmanList[i].canBeSummoned = true;
                snowmanList[i].summoningCost = snowmanSO.manaCost;
            }
        }

        /*
         * when player opened a snowman chest, add the snowman into snowman list in player scriptable object, and notice skill panel to update icons
         */
        private void AddSnowmanToPlayer(SnowmanTypeAndLevel snowman)
        {
            // foreach (var item in snowmanTypes)
            // {
            var foundItem = _playerSO.snowmanList.Find(x => x.type == snowman.type);
            if (foundItem != null) 
            { 
                foundItem.level = SnowmanLevel.Advanced;
            }
            else 
            { 
                _playerSO.snowmanList.Add(snowman);
            }
            // }
            _gameSO.SaveData();
            LoadSnowmanList();
            // EventHandler.UpdateSkillPanel();
        }

        public void TakeDamage(float damage)
        {
            if (isInvincible) return;
            health -= damage;
            _hurtImpulseSource.GenerateImpulseWithForce(0.5f);
            _playerController.sfxController.PlayAudio(PlayerSfxType.Hurt);
        }

        public void ReceiveHealing(float healing)
        {
            health += healing;
        }
    }
}
