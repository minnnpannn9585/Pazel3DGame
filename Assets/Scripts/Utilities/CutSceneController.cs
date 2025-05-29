using System;
using DataSO;
using Props;
using UISystem;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Events;
using UnityEngine.Playables;

namespace Utilities
{
    public class CutSceneController : MonoBehaviour
    {
        public string id;
        public bool hasWatched;
        private GameObject[] _canvas = new GameObject[]{};
        private PlayableDirector _director;
        private GameSO _gameSO;
        private LevelSO _levelSO;
        public BgmType nextBgmType;
        public CutsceneTransition transition;
        public bool notSwitchBgm;
        public UnityEvent onFinishedEvent;

        private void Awake()
        {
            id = gameObject.name;
            _director = GetComponent<PlayableDirector>();
            
            _gameSO = Resources.Load<GameSO>("DataSO/Game_SO");
            _levelSO = _gameSO.currentGameData.levelSo;
            
            LoadData();
            // gameObject.SetActive(!hasWatched);
            if (hasWatched) Destroy(gameObject);
            
            if (_director.playOnAwake && !hasWatched) OnPlay(_director);
        }

        private void OnEnable()
        {
            _director.played += OnPlay;
            _director.stopped += OnStop;
        }

        private void OnDisable()
        {
            _director.played -= OnPlay;
            _director.stopped -= OnStop;
        }

        private void OnPlay(PlayableDirector director)
        {
            _canvas = GameObject.FindGameObjectsWithTag("Canvas");
            foreach (var canvas in _canvas)
            {
                canvas.SetActive(false);
            }
            
            transition.gameObject.SetActive(true);
            transition.TransitionIn(true);
            EventHandler.SwitchBgm(!notSwitchBgm ? BgmType.Cutscene : BgmType.BossBGM);
            EventHandler.ShowInteractableSign(false, "talk");
            EventHandler.AllowInputControl(false);
            EventHandler.EnableDialogueInputControls(false);
            Time.timeScale = 0;  
        }

        private void OnStop(PlayableDirector director)
        {
            foreach (var canvas in _canvas)
            {
                canvas.SetActive(true);
            }
            Time.timeScale = 1;
            hasWatched = true;
            SaveData();
            _director.gameObject.SetActive(false);
            if (!notSwitchBgm) EventHandler.SwitchBgm(nextBgmType);
            EventHandler.AllowInputControl(true);
            EventHandler.EnableDialogueInputControls(true);
            onFinishedEvent?.Invoke();
            Destroy(gameObject);
        }
        
        private void LoadData()
        {
            var cg = _levelSO.cutScenes.Find(cutScene => cutScene.id == id);
            if (cg == null)
            {
                _levelSO.cutScenes.Add(new BlockWallData
                {
                    id = this.id,
                    isVisible = this.hasWatched
                });
            }
            else
            {
                hasWatched = cg.isVisible;
            }
        }
        
        private void SaveData()
        {
            var cg = _levelSO.cutScenes.Find(cutScene => cutScene.id == id);
            if (cg == null) return;
            cg.isVisible = hasWatched;
        }
    }
}
