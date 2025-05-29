using System;
using System.Globalization;
using DataSO;
using Snowman;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utilities;
using EventHandler = Utilities.EventHandler;

namespace UISystem
{
    public class SnowmanDetail : MonoBehaviour
    {
        public Image icon;
        public TextMeshProUGUI type;
        public TextMeshProUGUI level;
        public TextMeshProUGUI power;
        public TextMeshProUGUI health;
        public TextMeshProUGUI duration;
        public TextMeshProUGUI mana;
        public TextMeshProUGUI breakEffect;
        public TextMeshProUGUI cooldown;
        public TextMeshProUGUI basicDescription;
        public TextMeshProUGUI advancedDescription;

        public GameObject unlocked;
        public GameObject locked;

        private SnowmanSO _snowmanSO;
        
        private void OnEnable()
        {
            EventHandler.OnShowSnowmanDetail += ShowDetail;
        }

        private void OnDisable()
        {
            EventHandler.OnShowSnowmanDetail -= ShowDetail;
        }

        private void ShowDetail(SnowmanTypeAndLevel typeAndLevel, bool isUnlocked)
        {
            if (isUnlocked)
            {
                unlocked.SetActive(true);
                locked.SetActive(false);
                _snowmanSO = Resources.Load<SnowmanSO>("DataSO/SnowmanSO/" + typeAndLevel.type + "_SO");
                type.text = typeAndLevel.type.ToString();
                level.text = typeAndLevel.level.ToString();
                icon.sprite = _snowmanSO.icon;
                power.text = _snowmanSO.attack.ToString(CultureInfo.InvariantCulture);
                health.text = _snowmanSO.health.ToString(CultureInfo.InvariantCulture);
                duration.text = _snowmanSO.summonDuration.ToString(CultureInfo.InvariantCulture);
                mana.text = _snowmanSO.manaCost.ToString(CultureInfo.InvariantCulture);
                breakEffect.text = _snowmanSO.shieldBreakEfficiency.ToString();
                cooldown.text = _snowmanSO.cooldown.ToString(CultureInfo.InvariantCulture);
                basicDescription.text = _snowmanSO.basicAbilities;
                
                advancedDescription.text = typeAndLevel.level == SnowmanLevel.Advanced ? _snowmanSO.advancedAbilities : "Unlocked";
            }
            else
            {
                unlocked.SetActive(false);
                locked.SetActive(true);
            }
        }
    }
}
