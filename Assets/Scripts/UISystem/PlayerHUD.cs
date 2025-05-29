using DataSO;
using Player;
using UnityEngine;
using UnityEngine.UI;

namespace UISystem
{
    /*
     * Control player's HUD
     */
    public class PlayerHUD : MonoBehaviour
    {
        public Image healthBar;
        public Image staminaBar;
        // public Image energyBar;
        public float lerpSpeed = 5f;
        
        // private PlayerSO _playerSO;
        private PlayerAttribute _playerAttr;
        // private SummonSnowman _summonSnowman;
        private float _targetHealthFill;
        private float _targetStaminaFill;
        // private float _targetEnergyFill;

        private void Start()
        {
            var player = GameObject.FindWithTag("Player");
            _playerAttr = player.GetComponent<PlayerAttribute>();
            // _summonSnowman = player.GetComponent<SummonSnowman>();
            // _playerSO = Resources.Load<PlayerSO>("DataSO/Player_SO");
        }

        private void Update()
        {
            _targetHealthFill = _playerAttr.health / _playerAttr.maxHealth;
            _targetStaminaFill = _playerAttr.stamina / _playerAttr.maxStamina;
            // _targetEnergyFill = _playerAttr.mana / _playerAttr.snowmanList[_summonSnowman.currentIndex].summoningCost;
            
            healthBar.fillAmount = Mathf.Lerp(healthBar.fillAmount, _targetHealthFill, lerpSpeed * Time.deltaTime);
            staminaBar.fillAmount = Mathf.Lerp(staminaBar.fillAmount, _targetStaminaFill, lerpSpeed * Time.deltaTime);
            // energyBar.fillAmount = Mathf.Lerp(energyBar.fillAmount, _targetEnergyFill, lerpSpeed * Time.deltaTime);
        }
    }
}
