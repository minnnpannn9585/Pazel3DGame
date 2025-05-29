using System;
using System.Collections;
using System.Collections.Generic;
using DataSO;
using Snowman;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utilities;
using EventHandler = Utilities.EventHandler;

namespace UISystem
{
    public class SnowmanObtainedPrompt : MonoBehaviour
    {
        public Image icon;
        public TextMeshProUGUI type;
        public TextMeshProUGUI prompt;
        private PlayerSO _playerSO;
        private GameSO _gameSO;
        private SnowmanSO _snowmanSO;
        private readonly Dictionary<SnowmanType, SnowmanLevel> _snowmenPlayerHas = new();
        private RectTransform _trans;
        public float maxHeight = 200f;
        public float animationDuration = 1f;
        public float displayDuration = 2f;
        public AudioSource promptSfx;

        private void Awake()
        {
            _trans = GetComponent<RectTransform>();
        }

        private void OnEnable()
        {
            EventHandler.OnShowSnowmanObtainedPrompt += ShowSnowmanObtainedPrompt;
            promptSfx.Play();
        }

        private void OnDisable()
        {
            EventHandler.OnShowSnowmanObtainedPrompt -= ShowSnowmanObtainedPrompt;
        }

        private void ShowSnowmanObtainedPrompt(SnowmanTypeAndLevel typeAndLevel)
        {
            _gameSO = Resources.Load<GameSO>("DataSO/Game_SO");
            _playerSO = _gameSO.currentGameData.playerSo;
            _snowmanSO = Resources.Load<SnowmanSO>("DataSO/SnowmanSO/" + typeAndLevel.type + "_SO");
            icon.sprite = _snowmanSO.icon;
            type.text = _snowmanSO.type.ToString();
            foreach (var snowman in _playerSO.snowmanList)
            {
                _snowmenPlayerHas.Add(snowman.type, snowman.level);
            }

            if (_snowmenPlayerHas.TryGetValue(typeAndLevel.type, out var level))
            {
                prompt.text = level == SnowmanLevel.Basic ? "New snowman unlocked!" : "Snowman upgraded!";
            }

            StartCoroutine(AdjustHeight());
        }

        private IEnumerator AdjustHeight()
        {
            _snowmenPlayerHas.Clear();
            const float startHeight = 0; 
            float time = 0;

            while (time < animationDuration)
            {
                var currentHeight = Mathf.Lerp(startHeight, maxHeight, time / animationDuration);
                _trans.sizeDelta = new Vector2(_trans.sizeDelta.x, currentHeight);
                time += Time.deltaTime;
                yield return null;
            }

            _trans.sizeDelta = new Vector2(_trans.sizeDelta.x, maxHeight);
            
            yield return new WaitForSeconds(displayDuration);
            
            time = 0;
            while (time < animationDuration)
            {
                var currentHeight = Mathf.Lerp(maxHeight, 0, time / animationDuration);
                _trans.sizeDelta = new Vector2(_trans.sizeDelta.x, currentHeight);
                time += Time.deltaTime;
                yield return null;
            }

            _trans.sizeDelta = new Vector2(_trans.sizeDelta.x, 0);

            _trans.gameObject.SetActive(false);
        }
    }
}
