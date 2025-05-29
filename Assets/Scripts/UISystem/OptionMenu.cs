using System;
using UnityEngine;

namespace UISystem
{
    /*
     * Control option menu
     */
    public class OptionMenu : MonoBehaviour
    {
        public GameObject controls;
        public GameObject settings;
        
        private void OnEnable()
        {
            Time.timeScale = 0;
        }

        private void OnDisable()
        {
            Time.timeScale = 1;
        }

        public void TurnOnSettings(bool turnOn)
        {
            if (turnOn)
            {
                controls.SetActive(false);
                settings.SetActive(true);
            }
            else
            {
                settings.SetActive(false);
                controls.SetActive(true);
            }
        }
    }
}
