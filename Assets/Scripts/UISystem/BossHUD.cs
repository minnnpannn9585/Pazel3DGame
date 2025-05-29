using System;
using System.Text.RegularExpressions;
using Enemy;
using TMPro;
using UnityEngine;
using EventHandler = Utilities.EventHandler;
using Image = UnityEngine.UI.Image;

namespace UISystem
{
    public class BossHUD : MonoBehaviour
    {
        public GameObject hudPanel;
        public Image healthBar;
        public Image shieldBar;
        public TextMeshProUGUI bossName;
        public BaseEnemy enemy;

        private void Awake()
        {
            hudPanel.SetActive(false);
        }

        private void OnEnable()
        {
            EventHandler.OnShowBossHud += ShowBossHud;
        }

        private void OnDisable()
        {
            EventHandler.OnShowBossHud -= ShowBossHud;
        }

        private void Update()
        {
            if (enemy == null) return;
            healthBar.fillAmount = Mathf.Lerp(healthBar.fillAmount, enemy.health/enemy.maxHealth, 5f * Time.deltaTime);
            shieldBar.fillAmount = Mathf.Lerp(shieldBar.fillAmount, enemy.shield/enemy.maxShield, 5f * Time.deltaTime);
        }

        private void ShowBossHud(BaseEnemy myEnemy, bool result)
        {
            if (result)
            {
                enemy = myEnemy;
                bossName.text = Regex.Replace(enemy.gameObject.name, @"\s*\(\s*Clone\s*\)\s*", "");
                hudPanel.SetActive(true);
            }
            else
            {
                enemy = null;
                bossName.text = string.Empty;
                hudPanel.SetActive(false);
            }
        }
    }
}
