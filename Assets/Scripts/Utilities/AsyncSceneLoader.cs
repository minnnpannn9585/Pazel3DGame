using System;
using System.Collections;
using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Utilities
{
    public class AsyncSceneLoader : MonoBehaviour
    {
        public Image progressBar;
        public bool showProgressBar;
        public GameObject progressDisplay;
        // public TextMeshProUGUI progressText;

        private void Awake()
        {
            progressDisplay.SetActive(showProgressBar); 
        }

        public void LoadSceneAsync(string sceneName)
        {
            StartCoroutine(LoadSceneCoroutine(sceneName));
        }

        private IEnumerator LoadSceneCoroutine(string sceneName)
        {
            // 异步加载场景
            var asyncLoad = SceneManager.LoadSceneAsync(sceneName);

            // 等待加载完成
            while (!asyncLoad.isDone)
            {
                // 更新UI进度条
                if (progressBar != null && showProgressBar)
                {
                    progressBar.fillAmount = asyncLoad.progress;
                    // progressText.text = (asyncLoad.progress * 100).ToString("F0", CultureInfo.InvariantCulture) + "%";
                }

                // 等待下一帧
                yield return null;
                // yield return new WaitForSeconds(10f);
            }

            // yield return new WaitForSeconds(10f);
            
            gameObject.SetActive(false);
        }
    }
}
