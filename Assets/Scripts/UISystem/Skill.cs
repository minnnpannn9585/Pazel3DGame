using System.Collections;
using DataSO;
using Player;
using Snowman;
using UnityEngine;
using UnityEngine.UI;
using Utilities;

namespace UISystem
{
    /*
     * Control skill icon
     */
    public class Skill : MonoBehaviour
    {
        public SnowmanInfo snowmanInfo;
        public Sprite iconSprite;
        public Image cooldownMask;
        public Image mana;
        public GameObject advancedSign;
        
        private Image _skillIcon;
        private PlayerAttribute _playerAttr;
        

        private void Awake()
        {
            _skillIcon = GetComponent<Image>();
            _playerAttr = GameObject.FindWithTag("Player").GetComponent<PlayerAttribute>();
        }

        private void Update()
        {
            if (snowmanInfo == null) return;
            cooldownMask.fillAmount = snowmanInfo.cooldownTimer / snowmanInfo.cooldown;
            
            var manaFillAmount = _playerAttr.mana / snowmanInfo.summoningCost;
            mana.fillAmount = Mathf.Lerp(mana.fillAmount, manaFillAmount, 5f * Time.deltaTime);
        }

        /*
         * Update skill icon on skill panel
         */
        public void UpdateSkillIcon()
        {
            if (snowmanInfo == null) return;
            // iconSprite = Resources.Load<Sprite>("Images/" + snowmanInfor.type);
            iconSprite = Resources.Load<SnowmanSO>("DataSO/SnowmanSO/" + snowmanInfo.type + "_SO").icon;
            _skillIcon.sprite = iconSprite;
            advancedSign.SetActive(snowmanInfo.level == SnowmanLevel.Advanced);
        }

        public IEnumerator UpdateIcon()
        {
            while (transform.localScale.x > 0.1f)
            {
                var scale = transform.localScale.x - Time.fixedDeltaTime * 5;
                transform.localScale = new Vector3(scale, scale, scale);
                yield return null;
            }
            
            UpdateSkillIcon();
            
            while (transform.localScale.x < 1f)
            {
                var scale = transform.localScale.x + Time.fixedDeltaTime * 5;
                transform.localScale = new Vector3(scale, scale, scale);
                yield return null;
            }
        }
    }
}
