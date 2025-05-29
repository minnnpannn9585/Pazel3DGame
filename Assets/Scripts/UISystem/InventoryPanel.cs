using System;
using System.Collections.Generic;
using DataSO;
using Player;
using Snowman;
using UnityEngine;
using Utilities;
using EventHandler = Utilities.EventHandler;

namespace UISystem
{
    public class InventoryPanel : MonoBehaviour
    {
        public List<SnowmanCell> cells;
        public int currentIndex;

        private GameSO _gameSO;
        private PlayerSO _playerSO;
        private readonly Dictionary<SnowmanType, SnowmanLevel> _snowmenPlayerHas = new();
        private InputControls _inputControls;

        private void Awake()
        {
            _inputControls = new InputControls();
            _gameSO = Resources.Load<GameSO>("DataSO/Game_SO");
            _playerSO = _gameSO.currentGameData.playerSo;

            _inputControls.UI.Up.performed += _ => UpPressed();
            _inputControls.UI.Down.performed += _ => DownPressed();
            
            // UpdateSnowmanCells();
        }

        private void OnEnable()
        {
            Time.timeScale = 0;
            _inputControls.Enable();
            
            foreach (var snowman in _playerSO.snowmanList)
            {
                _snowmenPlayerHas.Add(snowman.type, snowman.level);
            }

            foreach (var cell in cells)
            {
                if (_snowmenPlayerHas.ContainsKey(cell.type))
                {
                    cell.ShowInformation(_snowmenPlayerHas[cell.type]);
                }
                else
                {
                    cell.HideInformation();
                }
            }
            
            UpdateSnowmanCells();
        }

        private void OnDisable()
        {
            Time.timeScale = 1;
            _inputControls.Disable();
            _snowmenPlayerHas.Clear();
        }

        private void UpPressed()
        {
            if (currentIndex > 0)
            {
                currentIndex--;
                UpdateSnowmanCells();
            }
        }

        private void DownPressed()
        {
            if (currentIndex < cells.Count - 1)
            {
                currentIndex++;
                UpdateSnowmanCells();
            }
        }

        public void UpdateSnowmanCells()
        {
            for (var i = 0; i < cells.Count; i++)
            {
                if (i == currentIndex)
                {
                    cells[i].isSelected = true;
                    
                    var snowman = new SnowmanTypeAndLevel
                    {
                        type = cells[i].type,
                        level = cells[i].level
                    };
                    EventHandler.ShowSnowmanDetail(snowman, cells[i].isUnlocked);
                }
                else cells[i].isSelected = false;
            }
        }

        public void UpdateSnowmanCellsByClick(int index)
        {
            currentIndex = index;
            UpdateSnowmanCells();
        }
    }
}
