using System;
using DataSO;
using Snowman;
using UnityEngine;
using EventHandler = Utilities.EventHandler;

namespace Props
{
    /*
     * Store item data in the chest and handle player interaction
     */
    public class TreasureChest : MonoBehaviour
    {
        public string id;
        public bool canOpen;
        public GameObject unlockingVFX;
        public SnowmanTypeAndLevel snowman;
        private GameSO _gameSO;
        private LevelSO _levelSO;

        private void Awake()
        {
            id = gameObject.name;
            _gameSO = Resources.Load<GameSO>("DataSO/Game_SO");
            _levelSO = _gameSO.currentGameData.levelSo;
            LoadData();
        }

        private void Update()
        {
            if (unlockingVFX == null) return;
            unlockingVFX.SetActive(!canOpen);
        }
        
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player")) EventHandler.ShowInteractableSign(true, "Open");
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player")) EventHandler.ShowInteractableSign(false, "Open");
        }

        /*
         * Open chest and destroy this game object
         */
        public void OpenChest()
        {
            EventHandler.OpenSnowmanChest(snowman);
            EventHandler.ShowInteractableSign(false, "Open");
            EventHandler.AddSnowman(snowman);
            // Destroy(gameObject);
            // gameObject.SetActive(false);
            SaveDataAfterOpened();
        }

        public void SaveCanOpenState()
        {
            var chest = _levelSO.treasureChests.Find(chest => chest.id == id);
            if (chest == null) return;
            chest.canOpen = canOpen;
        }

        private void SaveDataAfterOpened()
        {
            var chest = _levelSO.treasureChests.Find(chest => chest.id == id);
            if (chest == null) return;
            chest.isOpened = true;
            // _gameSO.SaveData();
            canOpen = false;
            chest.canOpen = false;
            gameObject.SetActive(false);
        }

        private void LoadData()
        {
            var chest = _levelSO.treasureChests.Find(chest => chest.id == id);
            if (chest == null)
            {
                _levelSO.treasureChests.Add(new TreasureChestData
                {
                    id = this.id,
                    isOpened = false,
                    canOpen = false
                });
            }
            else
            {
                canOpen = chest.canOpen;
                gameObject.SetActive(!chest.isOpened);
            }
        }
    }
}
