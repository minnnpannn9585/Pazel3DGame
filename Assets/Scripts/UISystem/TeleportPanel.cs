using System;
using TMPro;
using UnityEngine;
using Utilities;
using EventHandler = Utilities.EventHandler;

namespace UISystem
{
    public class TeleportPanel : MonoBehaviour
    {
        public string nextLevel;
        public string prompt;
        public TextMeshProUGUI promptText;
        public GameObject asyncSceneLoader;

        private void Update()
        {
            promptText.text = prompt;
        }

        private void OnEnable()
        {
            EventHandler.AllowMouseInput(false);
        }

        private void OnDisable()
        {
            EventHandler.AllowMouseInput(true);
        }

        public void GoToNextScene()
        {
            asyncSceneLoader.SetActive(true);
            asyncSceneLoader.GetComponent<AsyncSceneLoader>().LoadSceneAsync(nextLevel);
        }
    }
}
