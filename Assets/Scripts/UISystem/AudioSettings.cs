using System;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using Utilities;

namespace UISystem
{
    public class AudioSettings : MonoBehaviour
    {
        public AudioMixer audioMixer;
        private Slider _slider;
        public AudioGroup audioGroup;

        private void Awake()
        {
            _slider = GetComponent<Slider>();
            audioMixer.GetFloat(audioGroup.ToString(), out var volume);
            _slider.value = Mathf.Pow(10, volume / 20);
        }

        public void SetVolume()
        {
            if (audioMixer == null || _slider == null) return;
            audioMixer.SetFloat(audioGroup.ToString(), Mathf.Log10(_slider.value) * 20);
        }
    }
}
