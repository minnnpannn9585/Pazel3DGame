using System;
using System.Collections;
using System.Collections.Generic;
using DataSO;
using Enemy;
using Player;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Utilities
{
    /*
     * Control game and level states
     */
    public class LevelController : MonoBehaviour
    {
        public List<EnemyCamp> camps;
        public GameObject levelClearedPanel;
        public GameObject asyncSceneLoader;
        // public Slider progressBar;

        private PlayerSO _playerSO;
        private PlayerSO _playerDefaultSO;
        private PlayerAttribute _playerAttr;
        private LevelSO _levelSO;
        private GameSO _gameSO;

        private void Awake()
        {
            _gameSO = Resources.Load<GameSO>("DataSO/Game_SO");
            _playerSO = _gameSO.currentGameData.playerSo;
            _playerDefaultSO = Resources.Load<PlayerSO>("DataSO/PlayerDefault_SO");
            if (GameObject.FindWithTag("Player"))
                _playerAttr = GameObject.FindWithTag("Player").GetComponent<PlayerAttribute>();
            _levelSO = _gameSO.currentGameData.levelSo;
        }

        private void Update()
        {
            if (levelClearedPanel == null) return;
            CheckClearedCamp();
        }

        /*
         * Quit game
         */
        public void QuitGame()
        {
            Application.Quit();
        }

        /*
         * Display level cleared panel after all enemies have been killed
         */
        private void CheckClearedCamp()
        {
            var clearedCount = 0;
            foreach (var camp in camps)
            {
                if (camp.isCleared) clearedCount++;
            }
            levelClearedPanel.SetActive(clearedCount == camps.Count);
        }

        /*
         * When click the start game button, initialize player's attributes
         */
        public void ResetPlayerAttributes()
        {
            _playerSO.snowmanList.Clear();
            _playerSO.maxHealth = _playerDefaultSO.maxHealth;
            _playerSO.maxMana = _playerDefaultSO.maxMana;
            _playerSO.maxStamina = _playerDefaultSO.maxStamina;
            _playerSO.attack = _playerDefaultSO.attack;
            _playerSO.staminaRecovery = _playerDefaultSO.staminaRecovery;

            _playerSO.currentHealth = _playerSO.maxHealth;
            _playerSO.currentMana = _playerSO.maxMana;
            _playerSO.currentStamina = _playerSO.maxStamina;
            
            _levelSO.position = Vector3.zero;
            _levelSO.sceneName = string.Empty;
            _levelSO.enemyCamps.Clear();
            _levelSO.blockWalls.Clear();
            _levelSO.dialogueEvents.Clear();
            _levelSO.treasureChests.Clear();
            _levelSO.cutScenes.Clear();
        }

        public void StartAsyncSceneLoader(string sceneName)
        {
            asyncSceneLoader.SetActive(true);
            asyncSceneLoader.GetComponent<AsyncSceneLoader>().LoadSceneAsync(sceneName);
        }

        public void ReloadCurrentScene()
        {
            asyncSceneLoader.SetActive(true);
            asyncSceneLoader.GetComponent<AsyncSceneLoader>().LoadSceneAsync(SceneManager.GetActiveScene().name);
            _levelSO.respawnAtThisPosition = true;
        }

        public void LoadCurrentData()
        {
            asyncSceneLoader.SetActive(true);
            asyncSceneLoader.GetComponent<AsyncSceneLoader>().LoadSceneAsync(_levelSO.sceneName);
            _levelSO.respawnAtThisPosition = true; 
        }

        public void HealPlayer()
        {
            _playerAttr.health = _playerAttr.maxHealth;
            _playerAttr.stamina = _playerAttr.maxStamina;
            _playerAttr.mana = _playerAttr.maxMana;
        }

        public void HealPlayerWithSavingData()
        {
            _playerSO.currentHealth = _playerSO.maxHealth;
            _playerSO.currentStamina = _playerSO.maxStamina;
            _playerSO.currentMana = _playerSO.maxMana;
        }

        public void EnhancePlayer()
        {
            _playerSO.maxHealth += 50;
            _playerSO.maxStamina += 50;
            _playerSO.attack += 15;
            _playerAttr.InitializeAttributes(_playerSO);
            HealPlayer();
        }

        public void EnhanceAttack()
        {
            _playerAttr.attack += 50;
        }
    }
}
