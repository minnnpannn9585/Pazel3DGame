using System;
using UnityEngine;
using Utilities;
using EventHandler = Utilities.EventHandler;

namespace Enemy
{
    public class CampDoor : MonoBehaviour
    {
        public EnemyCamp enemyCamp;
        public GameObject vfx;
        public bool switchBgm;

        private void Awake()
        {
            if (vfx != null) vfx.SetActive(false);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                if (enemyCamp.isPlayerInCamp) return;
                enemyCamp.NotifyEnemiesToChangeChasingState();
                enemyCamp.isPlayerInCamp = true;
                EventHandler.ChangePlayerBattleState(true);
                if (switchBgm) EventHandler.SwitchBgm(enemyCamp.isBossCamp ? BgmType.BossBGM : BgmType.BattleBGM);
                EventHandler.EnableInteract(false);
            }
        }
    }
}
