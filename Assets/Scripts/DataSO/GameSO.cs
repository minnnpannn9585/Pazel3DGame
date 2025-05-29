using System;
using System.Collections.Generic;
using Player;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.Video;
using Utilities;
using EventHandler = Utilities.EventHandler;

namespace DataSO
{
    [CreateAssetMenu(menuName = "ScriptableObject/Create Game_SO", fileName = "Game_SO")]
    public class GameSO : ScriptableObject
    {
        public List<GameData> gameDataList;
        // public GameDataSet currentData;
        public GameData currentGameData;

        // private void OnEnable()
        // {
        //     gameData = currentData switch
        //     {
        //         GameDataSet.Data1 => gameDataList[1],
        //         GameDataSet.Data2 => gameDataList[2],
        //         GameDataSet.Data3 => gameDataList[3],
        //         _ => gameDataList[1]
        //     };
        // }

        private void OnEnable()
        {
            EventHandler.OnSavingDataAfterDialogue += SaveData;
        }

        private void OnDisable()
        {
            EventHandler.OnSavingDataAfterDialogue -= SaveData;
        }

        public void SwitchData(GameDataSet selectedData)
        {
            currentGameData = selectedData switch
            {
                GameDataSet.Data1 => gameDataList[1],
                GameDataSet.Data2 => gameDataList[2],
                GameDataSet.Data3 => gameDataList[3],
                _ => gameDataList[1]
            };
        }
        
        public void SaveData()
        {
            var playerAttr = GameObject.FindWithTag("Player").GetComponent<PlayerAttribute>();
            currentGameData.playerSo.currentHealth = playerAttr.health;
            currentGameData.playerSo.currentMana = playerAttr.mana;
            currentGameData.playerSo.currentStamina = playerAttr.stamina;
            currentGameData.levelSo.position = playerAttr.gameObject.transform.position;
            currentGameData.levelSo.sceneName = SceneManager.GetActiveScene().name;
            EventHandler.ShowSavingData();
        }
    }
}
