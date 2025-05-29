using System;
using UnityEngine;
using Utilities;

namespace Snowman
{
    [Serializable]
    public class SnowmanInfo
    {
        public SnowmanType type;
        public SnowmanLevel level;
        public float cooldown;
        public float cooldownTimer;
        public float summoningCost;
        public bool canBeSummoned;
    }

    [Serializable]
    public class SnowmanTypeAndLevel
    {
        public SnowmanType type;
        public SnowmanLevel level;
    }
}
