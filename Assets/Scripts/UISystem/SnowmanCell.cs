using System;
using DataSO;
using Snowman;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utilities;

namespace UISystem
{
    public class SnowmanCell : MonoBehaviour
    {
        public SnowmanType type;
        public SnowmanLevel level;
        public Image icon;
        public TextMeshProUGUI snowmanName;
        public GameObject highlight;
        public bool isSelected;
        public Sprite unknownIcon;
        public bool isUnlocked;
        
        private SnowmanSO _snowmanSO;

        private void Update()
        {
            highlight.SetActive(isSelected);
        }

        public void ShowInformation(SnowmanLevel snowmanLevel)
        {
            _snowmanSO = Resources.Load<SnowmanSO>("DataSO/SnowmanSO/" + type + "_SO");
            if (_snowmanSO == null) return;
            icon.sprite = _snowmanSO.icon;
            snowmanName.text = _snowmanSO.type.ToString();
            level = snowmanLevel;
            isUnlocked = true;
        }

        public void HideInformation()
        {
            icon.sprite = unknownIcon;
            snowmanName.text = "???";
            isUnlocked = false;
        }
    }
}
