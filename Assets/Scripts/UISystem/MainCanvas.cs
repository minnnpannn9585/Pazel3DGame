using System.Collections;
using Snowman;
using UnityEngine;
using EventHandler = Utilities.EventHandler;

namespace UISystem
{
    /*
     * Control main canvas
     */
    public class MainCanvas : MonoBehaviour
    {
        public GameObject gameOverPanel;
        public GameObject levelClearedPanel;
        public GameObject optionsMenu;
        public GameObject inventoryPanel;
        public GameObject snowmanObtainedPrompt;
        public GameObject teleportPanel;
        public GameObject savingData;
        public GameObject videoDisplay;
        public GameObject fullScreenVideoDisplay;
        public GameObject lowHealthOverlay;
        public GameObject sceneNameDisplay;

        private InputControls _inputControls;
        private Coroutine _showDataCoroutine;

        private void Awake()
        {
            gameOverPanel.SetActive(false);
            levelClearedPanel.SetActive(false);
            optionsMenu.SetActive(false);
            snowmanObtainedPrompt.SetActive(false);
            _inputControls = new InputControls();
            _inputControls.Global.OptionButton.performed += _=> HandlePanel(optionsMenu);
            _inputControls.Global.InventoryButton.performed += _ => HandlePanel(inventoryPanel);
            sceneNameDisplay.SetActive(true);
            sceneNameDisplay.GetComponent<LocationDisplay>().DisplaySceneName();
        }

        private void OnEnable()
        {
            EventHandler.OnPlayerDie += OpenGameOverPanel;
            EventHandler.OnOpenSnowmanObtainedPrompt += OpenSnowmanObtainedPrompt;
            EventHandler.OnOpenTeleportPanel += HandleTeleportPanel;
            EventHandler.OnShowSavingData += StartShowingData;
            EventHandler.OnPlayVideo += HandleVideoPlayer;
            EventHandler.OnHandleLowHeath += HandleLowHealthOverlay;
            EventHandler.OnHandleFullScreenVideo += HandleFullScreenVideo;
            _inputControls.Enable();
        }

        private void OnDisable()
        {
            EventHandler.OnPlayerDie -= OpenGameOverPanel;
            EventHandler.OnShowSnowmanObtainedPrompt -= OpenSnowmanObtainedPrompt;
            EventHandler.OnOpenTeleportPanel -= HandleTeleportPanel;
            EventHandler.OnShowSavingData -= StartShowingData;
            EventHandler.OnPlayVideo -= HandleVideoPlayer;
            EventHandler.OnHandleLowHeath -= HandleLowHealthOverlay;
            EventHandler.OnHandleFullScreenVideo -= HandleFullScreenVideo;
            _inputControls.Disable();
        }

        /*
         * Open game over panel
         */
        private void OpenGameOverPanel()
        {
            gameOverPanel.SetActive(true);
        }

        public void HandlePanel(GameObject panel)
        {
            if (panel.activeSelf)
            {
                panel.SetActive(false);
                EventHandler.SetGameplayActionMap(true);
            }
            else
            {
                panel.SetActive(true);
                EventHandler.SetGameplayActionMap(false);
            }

            if (panel == inventoryPanel)
            {
                inventoryPanel.GetComponent<InventoryPanel>().UpdateSnowmanCells();
            }
        }

        private void OpenSnowmanObtainedPrompt(SnowmanTypeAndLevel snowman)
        {
            if (snowmanObtainedPrompt == null)
            {
                // Debug.Log("prompt null");
                return;
            }
            snowmanObtainedPrompt.SetActive(true);
            EventHandler.ShowSnowmanObtainedPrompt(snowman);
        }

        private void HandleTeleportPanel(bool isOpen, string nextLevel, string prompt)
        {
            teleportPanel.SetActive(isOpen);
            var teleportScript = teleportPanel.GetComponent<TeleportPanel>();
            teleportScript.nextLevel = nextLevel;
            teleportScript.prompt = prompt;
        }

        private void StartShowingData()
        {
            _showDataCoroutine ??= StartCoroutine(ShowSavingData());
        }

        private void StopShowingData()
        {
            if (_showDataCoroutine == null) return;
            StopCoroutine(_showDataCoroutine);
            _showDataCoroutine = null;
        }

        private IEnumerator ShowSavingData()
        {
            if (savingData.activeSelf) yield return null;
            else
            {
                savingData.SetActive(true);

                yield return new WaitForSeconds(2f);
                
                savingData.SetActive(false);
                StopShowingData();
            }
        }

        private void HandleVideoPlayer(bool play)
        {
            videoDisplay.SetActive(play);
        }

        private void HandleLowHealthOverlay(bool lowHealth)
        {
            lowHealthOverlay.SetActive(lowHealth);
        }

        private void HandleFullScreenVideo(bool result)
        {
            fullScreenVideoDisplay.SetActive(result);
        }

        // private void HandleVideoDisplay(bool play)
        // {
        //     if (play) videoDisplay.SetActive(play);
        // }

        // private IEnumerator VideoDisplayAnim(bool turnOn)
        // {
        //     var displayHeight = videoDisplay.sizeDelta.y;
        //     Debug.Log("video display");
        //     if (turnOn)
        //     {
        //         Debug.Log("turn on");
        //         videoDisplay.gameObject.SetActive(true);
        //         while (displayHeight < 720f)
        //         {
        //             displayHeight += Time.fixedDeltaTime * 10;
        //             yield return null;
        //         }
        //     }
        //     else
        //     {
        //         while (displayHeight > 0f)
        //         {
        //             displayHeight -= Time.fixedDeltaTime * 10;
        //             yield return null;
        //         }
        //         videoDisplay.gameObject.SetActive(false);
        //     }
        // }
    }
}
