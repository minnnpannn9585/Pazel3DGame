using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UISystem
{
    public class LocationDisplay : MonoBehaviour
    {
        public TextMeshProUGUI locationText;
        
        public float maxHeight = 200f;
        public float animationDuration = 1f;
        public float displayDuration = 2f;
        private RectTransform _trans;
        private AudioSource _promptSfx;
        
        private void Awake()
        {
            _trans = GetComponent<RectTransform>();
            _promptSfx = GetComponent<AudioSource>();
        }

        public void DisplaySceneName()
        {
            locationText.text = SceneManager.GetActiveScene().name;
            StartCoroutine(AdjustHeight());
        }
        
        private IEnumerator AdjustHeight()
        {
            yield return new WaitForSecondsRealtime(1f);
            const float startHeight = 0; 
            float time = 0;
            _promptSfx.Play();
            
            while (time < animationDuration)
            {
                var currentHeight = Mathf.Lerp(startHeight, maxHeight, time / animationDuration);
                _trans.sizeDelta = new Vector2(_trans.sizeDelta.x, currentHeight);
                time += Time.unscaledDeltaTime;
                yield return null;
            }

            _trans.sizeDelta = new Vector2(_trans.sizeDelta.x, maxHeight);
            
            yield return new WaitForSecondsRealtime(displayDuration);
            
            time = 0;
            while (time < animationDuration)
            {
                var currentHeight = Mathf.Lerp(maxHeight, 0, time / animationDuration);
                _trans.sizeDelta = new Vector2(_trans.sizeDelta.x, currentHeight);
                time += Time.unscaledDeltaTime;
                yield return null;
            }

            _trans.sizeDelta = new Vector2(_trans.sizeDelta.x, 0);

            _trans.gameObject.SetActive(false);
        }
    }
}
