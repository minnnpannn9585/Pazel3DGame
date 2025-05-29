using System;
using System.Collections;
using System.Collections.Generic;
using DataSO;
using UnityEngine;
using UnityEngine.Events;
using Utilities;
using EventHandler = Utilities.EventHandler;

namespace Dialogue
{
    public class DialogueController : MonoBehaviour
    {
        public string id;
        // public UnityEvent onFinishEvent;
        public DialogueType type;
        public bool isAppeared = true;
        public int dialogueIndex;
        public List<DialogueList> dialogueLists;
        public bool isTalking;
        public bool canTalk;
        public GameObject respawnVfx;

        private InputControls _inputControls;
        private GameSO _gameSO;
        private LevelSO _levelSO;

        private Stack<DialoguePiece> _dialogueStack;
        private string _signText;
        private AudioSource _audioSource;
        private SphereCollider _sphereCollider;

        private void Awake()
        {
            id = gameObject.name;
            _inputControls = new InputControls();
            _inputControls.Gameplay.Interact.performed += _ => { if (canTalk && !isTalking) StartCoroutine(DialogueRoutine()); };
            _inputControls.Gameplay.Skip.performed += _ => EndDialogue();
            
            _gameSO = Resources.Load<GameSO>("DataSO/Game_SO");
            _levelSO = _gameSO.currentGameData.levelSo;
            LoadData();
            FillDialogueStack();
            gameObject.SetActive(isAppeared);

            _signText = type switch
            {
                DialogueType.Npc => "Talk",
                DialogueType.EmberFire => "Reignite",
                DialogueType.TreasureChest => "Open",
                _ => "Interact"
            };

            _audioSource = GetComponent<AudioSource>();
            _sphereCollider = GetComponent<SphereCollider>();
        }

        private void OnEnable()
        {
            _inputControls.Enable();
            EventHandler.OnSavingDataAfterDialogue += SaveDialogueData;
            EventHandler.OnEnableInteract += EnableInteract;
            EventHandler.OnEnableDialogueInputControls += AllowInputControls;
            isAppeared = true;
            if (respawnVfx != null) Instantiate(respawnVfx, transform.position, Quaternion.identity);
            SaveDialogueData();
        }

        private void OnDisable()
        {
            _inputControls.Disable();
            EventHandler.OnSavingDataAfterDialogue -= SaveDialogueData;
            EventHandler.OnEnableInteract -= EnableInteract;
            EventHandler.OnEnableDialogueInputControls -= AllowInputControls;
            EventHandler.ShowInteractableSign(false, "talk");
            // isAppeared = false;
        }

        private void Update()
        {
            dialogueIndex = Mathf.Clamp(dialogueIndex, 0, dialogueLists.Count);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player") && dialogueLists.Count > 0)
            {
                canTalk = true;
                EventHandler.ShowInteractableSign(true, _signText);
            }
        }
        
        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                canTalk = false;
                EventHandler.ShowInteractableSign(false, _signText);
            }
        }

        private void FillDialogueStack()
        {
            _dialogueStack = new Stack<DialoguePiece>();
            if (dialogueLists.Count < 1) return;
            var list = dialogueLists[dialogueIndex].dialogueList;
            for (var i = list.Count - 1; i > -1; i--)
            {
                list[i].isDone = false;
                _dialogueStack.Push(list[i]);
            }
        }

        private IEnumerator DialogueRoutine()
        {
            isTalking = true;
            // {
                if (_dialogueStack.TryPop(out var result))
                {
                    result.beforeTalkEvent?.Invoke();
                    EventHandler.ShowDialoguePiece(result);
                    yield return new WaitUntil(() => result.isDone);
                    result.afterTalkEvent?.Invoke();
                    isTalking = false;
                }
                else
                {
                    EndDialogue();
                    // EventHandler.ShowDialoguePiece(null);
                    // FillDialogueStack();
                    // isTalking = false;
                    //
                    // onFinishEvent?.Invoke();
                    _inputControls.Disable();
                    yield return new WaitForSeconds(1f);
                    _inputControls.Enable();
                    // canTalk = true;
                }
            // }
        }

        private void EndDialogue()
        {
            if (!canTalk || dialogueLists.Count < 1) return;
            var index = dialogueIndex;
            
            EventHandler.ShowDialoguePiece(null);
            FillDialogueStack();
            isTalking = false;
            dialogueLists[index].onFinishEvent?.Invoke();
            EventHandler.SaveDataAfterDialogue();
        }

        public void DestroyMe()
        {
            EventHandler.ShowInteractableSign(false, _signText);
            Destroy(gameObject);
        }

        public void ChangeDialogueIndex(int index)
        {
            dialogueIndex = index;
            FillDialogueStack();
        }
        
        public void ChangeNextDialogueIndex(int index)
        {
            dialogueIndex = index;
        }

        private void LoadData()
        {
            var dialogue = _levelSO.dialogueEvents.Find(dialogue => dialogue.id == id);
            if (dialogue == null)
            {
                _levelSO.dialogueEvents.Add(new DialogueEventData
                {
                    id = this.id,
                    isAppeared = this.isAppeared,
                    dialogueIndex = 0
                });
            }
            else
            {
                isAppeared = dialogue.isAppeared;
                dialogueIndex = dialogue.dialogueIndex;
            }
        }

        private void SaveDialogueData()
        {
            var dialogue = _levelSO.dialogueEvents.Find(dialogue => dialogue.id == id);
            if (dialogue == null) return;

            dialogue.dialogueIndex = dialogueIndex;
            dialogue.isAppeared = isAppeared;
        }

        public void OnDisableGameObject()
        {
            isAppeared = false;
            SaveDialogueData();
            gameObject.SetActive(false);
        }

        public void PlayDialogueVoice(AudioClip clip)
        {
            _audioSource.clip = clip;
            _audioSource.Play();
        }

        private void EnableInteract(bool result)
        {
            _sphereCollider.enabled = result;
        }

        private void AllowInputControls(bool result)
        {
            if (result) _inputControls.Enable();
            else _inputControls.Disable();
        }
    }
}
