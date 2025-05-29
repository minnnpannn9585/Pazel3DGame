using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

namespace UISystem
{
    public class UiButton : MonoBehaviour
    {
        public AudioSource audioSource;

        public void PlayHoverSfx()
        {
            audioSource.Play();
        }
    }
}
