using DataSO;
using Snowman;
using UnityEngine;

namespace UISystem
{
    public class SnowmanHUD : NpcHUD
    {
        /*
         * Snowman's HUD
         */
        public BaseSnowman snowmanAttr;
        public SnowmanSO snowmanSO;

        protected override void Awake()
        {
            base.Awake();
            snowmanSO = Resources.Load<SnowmanSO>("DataSO/SnowmanSO/" + snowmanAttr.type + "_SO");
        }

        protected override void Update()
        {
            FillPercentage1 = snowmanAttr.health / snowmanSO.health;
            FillPercentage2 = (snowmanSO.summonDuration - snowmanAttr.summonTimer) / snowmanSO.summonDuration;
            
            base.Update();
        }
    }
}
