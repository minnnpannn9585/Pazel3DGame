using System;
using DataSO;
using Snowman;
using UnityEngine;
using EventHandler = Utilities.EventHandler;

namespace Player
{
    /*
     * The action of summoning snowmen
     */
    public class SummonSnowman : MonoBehaviour
    {
        public GameObject currentSnowman;

        public Transform startPosition;
        public float summoningCost;

        public int currentIndex;
        public GameObject summonVfx;

        // private PlayerSO _playerSO;
        private PlayerAttribute _playerAttr;

        private void Awake()
        {
            _playerAttr = GetComponent<PlayerAttribute>();
            // _playerSO = Resources.Load<PlayerSO>("DataSO/Player_SO");
            currentIndex = 0;
        }

        private void Start()
        {
            LoadSnowmanPrefab();
        }

        /*
         * Press E to plus currentIndex and reload snowman prefab
         */
        public void SwitchSnowmanLeft()
        {
            if (currentIndex < _playerAttr.snowmanList.Count-1) currentIndex++;
            else currentIndex = 0;
            
            LoadSnowmanPrefab();
        }
        
        /*
         * Press Q to minus currentIndex and reload snowman prefab
         */
        public void SwitchSnowmanRight()
        {
            if (currentIndex > 0) currentIndex--;
            else currentIndex = _playerAttr.snowmanList.Count-1;
            
            LoadSnowmanPrefab();
        }

        /*
         * Summon a new snowman and replace the existed one
         */
        public void SummonCurrentSnowman()
        {
            if (_playerAttr.snowmanList.Count < 1) return;
            var snowman = _playerAttr.snowmanList[currentIndex];
            if (!snowman.canBeSummoned || _playerAttr.mana < snowman.summoningCost) return;
            EventHandler.DestroyExistedSnowman();
            Instantiate(summonVfx, startPosition.position, startPosition.rotation);
            var snowmanGO = Instantiate(currentSnowman, startPosition.position, startPosition.rotation);
            snowmanGO.GetComponent<BaseSnowman>().SetLevel(snowman.level);
            snowman.canBeSummoned = false;
            _playerAttr.mana -= snowman.summoningCost;
            snowman.cooldownTimer = snowman.cooldown;
        }

        /*
         * Load snowman prefab by currentIndex
         */
        public void LoadSnowmanPrefab()
        {
            if (_playerAttr.snowmanList.Count < 1) return;
            var snowmanType = _playerAttr.snowmanList[currentIndex].type;
            currentSnowman = Resources.Load<SnowmanSO>("DataSO/SnowmanSO/" + snowmanType + "_SO").prefab;

            if (currentSnowman == null) return;
            summoningCost = currentSnowman.GetComponent<BaseSnowman>().manaCost;
        }
    }
}
