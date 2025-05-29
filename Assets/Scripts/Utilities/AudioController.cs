using System;
using System.Collections;
using System.Collections.Generic;
using Player;
using UnityEngine;
using UnityEngine.Audio;

namespace Utilities
{
    public class AudioController : MonoBehaviour
    {
        public AudioMixer audioMixer;
        public float transitionTime = 1.0f;
        [Header("Snapshot")]
        public AudioMixerSnapshot sceneSnapshot;
        public AudioMixerSnapshot battleSnapshot;
        public AudioMixerSnapshot bossSnapshot;
        public AudioMixerSnapshot cutsceneSnapshot;
        [Header("Audio Source")]
        public AudioSource sceneBGM;
        public AudioSource battleBGM;
        public AudioSource bossBGM;
        // private PlayerAttribute _playerAttr;

        private Coroutine _bgmCoroutine;

        private void Awake()
        {
            // _playerAttr = GameObject.FindWithTag("Player").GetComponent<PlayerAttribute>();
            // SwitchBgm(BgmType.SceneBGM);
        }

        private void OnEnable()
        {
            EventHandler.OnSwitchBgm += SwitchBgm;
        }

        private void OnDisable()
        {
            EventHandler.OnSwitchBgm -= SwitchBgm;
        }

        private void SwitchBgm(BgmType type)
        {
            StopBgmCoroutine();
            _bgmCoroutine = type switch
            {
                // _bgmCoroutine = StartCoroutine(enumerator());
                BgmType.SceneBGM => StartCoroutine(SwitchToSceneBGM()),
                BgmType.BattleBGM => StartCoroutine(SwitchToBattleBGM()),
                BgmType.BossBGM => StartCoroutine(SwitchToBossBGM()),
                BgmType.Cutscene => StartCoroutine(SwitchToCutscene()),
                _ => _bgmCoroutine
            };
        }

        public void PlayBossBgm()
        {
            _bgmCoroutine = StartCoroutine(SwitchToBossBGM());
        }

        public void PlayCutsceneBgm()
        {
            _bgmCoroutine = StartCoroutine(SwitchToCutscene());
        }

        // private void Update()
        // {
        //     if (_playerAttr.isInCombat)
        //     {
        //         if (battleBGM.isPlaying == false) battleBGM.Play();
        //         battleSnapshot.TransitionTo(transitionTime);
        //         bossBGM.Stop();
        //         sceneBGM.Stop();
        //     }
        //     else
        //     {
        //         if (sceneBGM.isPlaying == false) sceneBGM.Play();
        //         sceneSnapshot.TransitionTo(transitionTime);
        //         bossBGM.Stop();
        //         battleBGM.Stop();
        //     }
        // }
        private void StopBgmCoroutine()
        {
            if (_bgmCoroutine != null)
            {
                StopCoroutine(_bgmCoroutine);
                _bgmCoroutine = null;
            }
        }

        private IEnumerator SwitchToSceneBGM()
        {
            sceneBGM.Play();
            sceneSnapshot.TransitionTo(transitionTime);
            yield return new WaitForSecondsRealtime(transitionTime);
            bossBGM.Stop();
            battleBGM.Stop();
            
            StopBgmCoroutine();
        }
        
        private IEnumerator SwitchToBattleBGM()
        {
            battleBGM.Play();
            battleSnapshot.TransitionTo(transitionTime);
            yield return new WaitForSecondsRealtime(transitionTime);
            bossBGM.Stop();
            sceneBGM.Stop();
            
            StopBgmCoroutine();
        }
        
        private IEnumerator SwitchToBossBGM()
        {
            bossBGM.Play();
            bossSnapshot.TransitionTo(transitionTime);
            yield return new WaitForSecondsRealtime(transitionTime);
            battleBGM.Stop();
            sceneBGM.Stop();
            
            StopBgmCoroutine();
        }
        
        private IEnumerator SwitchToCutscene()
        {
            // bossBGM.Play();
            Debug.Log("cut scene audio");
            cutsceneSnapshot.TransitionTo(transitionTime);
            yield return new WaitForSecondsRealtime(transitionTime);
            StopBgmCoroutine();
        }
    }
}
