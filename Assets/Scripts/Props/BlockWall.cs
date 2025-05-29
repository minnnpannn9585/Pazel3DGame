using System;
using DataSO;
using UnityEngine;

namespace Props
{
    public class BlockWall: MonoBehaviour
    {
        public string id;
        public bool isVisible;
        private GameSO _gameSO;
        private LevelSO _levelSO;

        private void Awake()
        {
            id = gameObject.name;
            _gameSO = Resources.Load<GameSO>("DataSO/Game_SO");
            _levelSO = _gameSO.currentGameData.levelSo;
            
            LoadData();
            gameObject.SetActive(isVisible);
        }

        private void OnEnable()
        {
            isVisible = true;
            SaveData();
        }

        private void OnDisable()
        {
            // isVisible = false;
            SaveData();
        }

        private void LoadData()
        {
            var wall = _levelSO.blockWalls.Find(blockWall => blockWall.id == id);
            if (wall == null)
            {
                _levelSO.blockWalls.Add(new BlockWallData
                {
                    id = this.id,
                    isVisible = this.isVisible
                });
            }
            else
            {
                isVisible = wall.isVisible;
            }
        }

        private void SaveData()
        {
            var wall = _levelSO.blockWalls.Find(blockWall => blockWall.id == id);
            if (wall == null) return;
            wall.isVisible = isVisible;
        }

        public void OnDisableGameObject()
        {
            isVisible = false;
            SaveData();
            gameObject.SetActive(false);
        }
    }
}