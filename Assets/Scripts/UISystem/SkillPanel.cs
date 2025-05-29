using System.Collections;
using System.Collections.Generic;
using Player;
using Snowman;
using UnityEngine;
using EventHandler = Utilities.EventHandler;

namespace UISystem
{
    /*
     * Control skill panel
     */
    public class SkillPanel : MonoBehaviour
    {
        public List<GameObject> skillIcons;
        
        private SummonSnowman _summonSnowmanScript;
        private PlayerAttribute _playerAttr;
        
        public bool isMoving;
        private const float MoveTime = 0.5f;
        private readonly List<Vector2> _targetPositions = new();
        private const float CenterThreshold = 10f;
        

        private void Awake()
        {
            var player = GameObject.FindWithTag("Player");
            _summonSnowmanScript = player.GetComponent<SummonSnowman>();
            _playerAttr = player.GetComponent<PlayerAttribute>();
        }

        private void Start()
        {
            UpdateSkill();
        }

        private void OnEnable()
        {
            EventHandler.OnUpdateSkillPanel += UpdateSkill;
        }

        private void OnDisable()
        {
            EventHandler.OnUpdateSkillPanel -= UpdateSkill;
        }

        private void Update()
        {
            // if (!isMoving)
            // {
            //     foreach (var icon in skillIcons)
            //     {
            //         var rectTransform = icon.GetComponent<RectTransform>();
            //         UpdateIconScale(rectTransform);
            //     }
            // }
        }

        /*
         * Update skill icons and information
         */
        public void UpdateSkill()
        {
            if (_playerAttr.snowmanList.Count < 1) return;
            var currentIndex = _summonSnowmanScript.currentIndex;
            for (var i = 0; i < 3; i++)
            {
                var index = currentIndex + i - 1;
                if (index < 0) index = _playerAttr.snowmanList.Count-1;
                if (index > _playerAttr.snowmanList.Count - 1) index = 0;
                skillIcons[i].GetComponent<Skill>().snowmanInfo = _playerAttr.snowmanList[index];
                // skillIcons[i].GetComponent<Skill>().UpdateSkillIcon();
                StartCoroutine(SwitchSkillCooldown());
                StartCoroutine(skillIcons[i].GetComponent<Skill>().UpdateIcon());
            }
        }

        private IEnumerator SwitchSkillCooldown()
        {
            isMoving = true;
            yield return new WaitForSeconds(0.5f);
            isMoving = false;
        }
        

        /*
         * Move icon objects left
         */
        // public void MoveIconsLeft()
        // {
        //     if (isMoving) return;
        //     
        //     _targetPositions.Clear();
        //
        //     foreach (var icon in skillIcons)
        //     {
        //         var rectTransform = icon.GetComponent<RectTransform>();
        //         var currentPos = rectTransform.anchoredPosition;
        //         var targetPos = new Vector2(currentPos.x - 150, currentPos.y);
        //         if (targetPos.x < -400) targetPos.x = 300;
        //         else if (targetPos.x > 400) targetPos.x = -300;
        //         _targetPositions.Add(targetPos);
        //     }
        //
        //     StartCoroutine(MoveIcons());
        //
        //     isMoving = true;
        // }
        //
        /*
         * Move icon objects right
         */
        // public void MoveIconsRight()
        // {
        //     if (isMoving) return;
        //     
        //     _targetPositions.Clear();
        //
        //     foreach (var icon in skillIcons)
        //     {
        //         var rectTransform = icon.GetComponent<RectTransform>();
        //         var currentPos = rectTransform.anchoredPosition;
        //         var targetPos = new Vector2(currentPos.x + 150, currentPos.y);
        //         if (targetPos.x < -400) targetPos.x = 300;
        //         else if (targetPos.x > 400) targetPos.x = -300;
        //         _targetPositions.Add(targetPos);
        //     }
        //
        //     StartCoroutine(MoveIcons());
        //     
        //     isMoving = true;
        // }
        
        /*
         * Make the action of switching snowman has a short cooldown, and also make the transition smooth
         */
        // private IEnumerator MoveIcons()
        // {
        //     float elapsedTime = 0;
        //
        //     while (elapsedTime < MoveTime)
        //     {
        //         elapsedTime += Time.deltaTime;
        //         var t = elapsedTime / MoveTime;
        //
        //         for (var i = 0; i < skillIcons.Count; i++)
        //         {
        //             var rectTransform = skillIcons[i].GetComponent<RectTransform>();
        //             
        //             rectTransform.anchoredPosition = Vector2.Lerp(rectTransform.anchoredPosition, _targetPositions[i], t);
        //         
        //             UpdateIconScale(rectTransform);
        //         }
        //         
        //         yield return null;
        //     }
        //
        //     isMoving = false;
        //     UpdateSideIcons();
        // }
        
        /*
         * Scale snowman icon
         */
        private static void SetIconScale(GameObject icon, float scale)
        {
            var rectTransform = icon.GetComponent<RectTransform>();
            rectTransform.localScale = new Vector3(scale, scale, 1);
        }
        
        /*
         * Scale up the icon which is placed in the middle
         */
        private static void UpdateIconScale(RectTransform rectTransform)
        {
            var distance = Mathf.Abs(rectTransform.anchoredPosition.x);
            var scale = distance <= CenterThreshold ? 1.5f : 1.0f;
            SetIconScale(rectTransform.gameObject, scale);
        }
        
        /*
         * Update the most left and right icons after switched snowman
         */
        private void UpdateSideIcons()
        {
            var snowmanListCount = _playerAttr.snowmanList.Count;
            var offsetIndex = snowmanListCount switch
            {
                < 3 => 0,
                < 4 => -1,
                _ => -2
            };

            foreach (var icon in skillIcons)
            {
                var rectTransform = icon.GetComponent<RectTransform>();
                if (Mathf.Abs(rectTransform.anchoredPosition.x - 300) < CenterThreshold)
                {
                    icon.GetComponent<Skill>().snowmanInfo = UpdateSnowmanBuffers(offsetIndex, snowmanListCount);
                    icon.GetComponent<Skill>().UpdateSkillIcon();
                }
                else if (Mathf.Abs(rectTransform.anchoredPosition.x - (-300)) < CenterThreshold)
                {
                    icon.GetComponent<Skill>().snowmanInfo = UpdateSnowmanBuffers(-offsetIndex, snowmanListCount);
                    icon.GetComponent<Skill>().UpdateSkillIcon();
                }
            }
        }
        
        /*
         * Get the snowman information from player attributes
         */
        private SnowmanInfo UpdateSnowmanBuffers(int i, int snowmanListCount)
        {
            if (_playerAttr.snowmanList.Count < 1) return null;
            var sum = _summonSnowmanScript.currentIndex + i;
            if (_playerAttr.snowmanList.Count < 2)
            {
                sum = 0;
            }
            else
            {
                if (sum < 0)
                {
                    sum += snowmanListCount;
                }
                else if (sum > snowmanListCount - 1)
                {
                    sum -= snowmanListCount;
                }
            }
            return _playerAttr.snowmanList[sum];
        }

        /*
         * Reset position of all icon objects
         */
        // public void ResetIconsPosition()
        // {
        //     for (var i = 0; i < skillIcons.Count; i++)
        //     {
        //         var xPos = -300 + 150*i;
        //         var icon = skillIcons[i];
        //         var iconPos = icon.GetComponent<RectTransform>().anchoredPosition;
        //         icon.GetComponent<RectTransform>().anchoredPosition = new Vector2(xPos,iconPos.y);
        //     }
        // }
    }
}
