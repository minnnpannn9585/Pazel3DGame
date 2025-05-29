using System;
using Dialogue;
using Enemy;
using Snowman;
using UnityEngine;

namespace Utilities
{
    /*
     * Observer pattern
     */
    public static class EventHandler
    {
        /*
         * Notice existed snowman to destroy itself
         */
        public static event Action OnDestroyExistedSnowman;

        public static void DestroyExistedSnowman()
        {
            OnDestroyExistedSnowman?.Invoke();
        }
        
        /*
         * Notice player a snowman chest has been opened
         */
        public static event Action<SnowmanTypeAndLevel> OnOpenSnowmanChest;

        public static void OpenSnowmanChest(SnowmanTypeAndLevel snowman)
        {
            OnOpenSnowmanChest?.Invoke(snowman);
        }

        /*
         * Notice skill panel to update ui display
         */
        public static event Action OnUpdateSkillPanel;

        public static void UpdateSkillPanel()
        {
            OnUpdateSkillPanel?.Invoke();
        }

        /*
         * Notice game player has been died
         */
        public static event Action OnPlayerDie;

        public static void PlayerDie()
        {
            OnPlayerDie?.Invoke();
        }

        public static event Action<bool> OnSetGameplayActionMap;

        public static void SetGameplayActionMap(bool isActive)
        {
            OnSetGameplayActionMap?.Invoke(isActive);
        }

        public static event Action<SnowmanTypeAndLevel, bool> OnShowSnowmanDetail;

        public static void ShowSnowmanDetail(SnowmanTypeAndLevel typeAndLevel, bool isUnlocked)
        {
            OnShowSnowmanDetail?.Invoke(typeAndLevel, isUnlocked);
        }

        public static event Action<SnowmanTypeAndLevel> OnShowSnowmanObtainedPrompt;

        public static void ShowSnowmanObtainedPrompt(SnowmanTypeAndLevel typeAndLevel)
        {
            OnShowSnowmanObtainedPrompt?.Invoke(typeAndLevel);
        }
        
        public static event Action<SnowmanTypeAndLevel> OnOpenSnowmanObtainedPrompt;

        public static void OpenSnowmanObtainedPrompt(SnowmanTypeAndLevel typeAndLevel)
        {
            OnOpenSnowmanObtainedPrompt?.Invoke(typeAndLevel);
        }
        
        public static event Action<DialoguePiece> OnShowDialoguePiece;

        public static void ShowDialoguePiece(DialoguePiece dialoguePiece)
        {
            OnShowDialoguePiece?.Invoke(dialoguePiece);
        }

        public static event Action<bool> OnAllowInputControl;

        public static void AllowInputControl(bool allow)
        {
            OnAllowInputControl?.Invoke(allow);
        }

        public static event Action<bool, string> OnShowInteractableSign;

        public static void ShowInteractableSign(bool isActive, string text)
        {
            OnShowInteractableSign?.Invoke(isActive, text);
        }

        public static event Action<bool, string, string> OnOpenTeleportPanel;

        public static void OpenTeleportPanel(bool isOpen, string nextLevel, string prompt)
        {
            OnOpenTeleportPanel?.Invoke(isOpen, nextLevel, prompt);
        }
        
        public static event Action<bool> OnAllowMouseInput;

        public static void AllowMouseInput(bool allow)
        {
            OnAllowMouseInput?.Invoke(allow);
        }

        public static event Action<GameObject> OnAddEnemyToCombatList;

        public static void AddEnemyToCombatList(GameObject enemy)
        {
            OnAddEnemyToCombatList?.Invoke(enemy);
        }
        
        public static event Action<GameObject> OnRemoveEnemyToCombatList;

        public static void RemoveEnemyToCombatList(GameObject enemy)
        {
            OnRemoveEnemyToCombatList?.Invoke(enemy);
        }

        public static event Action OnShowSavingData;

        public static void ShowSavingData()
        {
            OnShowSavingData?.Invoke();
        }

        public static event Action<FovType> OnChangeFOV;

        public static void ChangeFOV(FovType fov)
        {
            OnChangeFOV?.Invoke(fov);
        }

        public static event Action OnSavingDataAfterDialogue;

        public static void SaveDataAfterDialogue()
        {
            OnSavingDataAfterDialogue?.Invoke();
        }

        public static event Action<bool> OnPlayVideo;

        public static void PlayVideo(bool play)
        {
            OnPlayVideo?.Invoke(play);
        }

        public static event Action<bool> OnChangePlayerBattleState;

        public static void ChangePlayerBattleState(bool isInBattle)
        {
            OnChangePlayerBattleState?.Invoke(isInBattle);
        }

        public static event Action<BgmType> OnSwitchBgm;

        public static void SwitchBgm(BgmType type)
        {
            OnSwitchBgm?.Invoke(type);
        }

        public static event Action<SnowmanTypeAndLevel> OnAddSnowman;

        public static void AddSnowman(SnowmanTypeAndLevel snowman)
        {
            OnAddSnowman?.Invoke(snowman);
        }

        public static event Action<BaseEnemy, bool> OnShowBossHud;

        public static void ShowBossHud(BaseEnemy enemy, bool result)
        {
            OnShowBossHud?.Invoke(enemy, result);
        }

        public static event Action OnRetreatSnowman;

        public static void RetreatSnowman()
        {
            OnRetreatSnowman?.Invoke();
        }

        public static event Action<bool> OnEnableInteract;

        public static void EnableInteract(bool result)
        {
            OnEnableInteract?.Invoke(result);
        }

        public static event Action<bool> OnEnableDialogueInputControls;

        public static void EnableDialogueInputControls(bool result)
        {
            OnEnableDialogueInputControls?.Invoke(result);
        }

        public static event Action<bool> OnHandleLowHeath;

        public static void HandleLowHealth(bool lowHealth)
        {
            OnHandleLowHeath?.Invoke(lowHealth);
        }

        public static event Action<bool> OnHandleFullScreenVideo;

        public static void HandleFullScreenVideo(bool result)
        {
            OnHandleFullScreenVideo?.Invoke(result);
        }

        // public static event Action OnUpdateSkillIcon;
        //
        // public static void UpdateSkillIcon()
        // {
        //     OnUpdateSkillIcon?.Invoke();
        // }
    }
}
