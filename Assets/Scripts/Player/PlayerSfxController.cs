using System;
using UnityEngine;
using Utilities;

namespace Player
{
    public class PlayerSfxController : MonoBehaviour
    {
        public AudioSource throwSfx;
        public AudioSource dashSfx;
        public AudioSource summonSfx;
        public AudioSource switchSfx;
        public AudioSource rollSfx;
        public AudioSource stepSfx;
        public AudioSource hurtSfx;
        
        public void PlayAudio(PlayerSfxType type)
        {
            switch (type)
            {
                case PlayerSfxType.Throw:
                    throwSfx.Play();
                    break;
                case PlayerSfxType.Dash:
                    dashSfx.Play();
                    break;
                case PlayerSfxType.Summon:
                    summonSfx.Play();
                    break;
                case PlayerSfxType.Switch:
                    switchSfx.Play();
                    break;
                case PlayerSfxType.Roll:
                    rollSfx.Play();
                    break;
                case PlayerSfxType.Step:
                    stepSfx.Play();
                    break;
                case PlayerSfxType.Hurt:
                    hurtSfx.Play();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }

        public void PauseAudio(PlayerSfxType type)
        {
            switch (type)
            {
                case PlayerSfxType.Roll:
                    rollSfx.Pause();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }
        
        public void StopAudio(PlayerSfxType type)
        {
            switch (type)
            {
                case PlayerSfxType.Roll:
                    rollSfx.Stop();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }

        public bool GetAudioState(PlayerSfxType type)
        {
            if (type == PlayerSfxType.Roll)
            {
                return rollSfx.isPlaying;
            }

            return false;
        }
    }
}
