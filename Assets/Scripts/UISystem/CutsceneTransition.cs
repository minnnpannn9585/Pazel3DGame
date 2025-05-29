using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace UISystem
{
    public class CutsceneTransition : MonoBehaviour
    {
        private Coroutine _transitionCoroutine;
        private Image _image;
        public float duration;

        private void Awake()
        {
            _image = GetComponent<Image>();
        }

        public void TransitionIn(bool transitionIn)
        {
            // Debug.Log("transition: " + transitionIn);
            _transitionCoroutine ??= StartCoroutine(Transition(transitionIn));
        }

        private IEnumerator Transition(bool transitionIn)
        {
            // Debug.Log("transition coroutine start" );
            var values = new Vector2(1, 0);
            if (!transitionIn) values = new Vector2(0, 1);
            // var halfDuration = duration / 2f;
            var timer = 0f;

            // 从0变化到1
            while (timer <= duration)
            {
                var alpha = Mathf.Lerp(values.x, values.y, timer / duration);
                // Debug.Log("transition alpha: " + alpha);
                _image.color = new Color(0, 0, 0, alpha);
                timer += Time.unscaledDeltaTime; // 使用 Time.unscaledDeltaTime
                yield return null; // 等待下一帧
            }
            
            if (_transitionCoroutine == null) yield break;
            StopCoroutine(_transitionCoroutine);
            _transitionCoroutine = null;
            gameObject.SetActive(false);
        }
    }
}
