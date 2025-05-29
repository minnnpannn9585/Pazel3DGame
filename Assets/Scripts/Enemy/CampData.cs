using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Enemy
{
    [Serializable]
    public class CampData
    {
        public string id;
        public bool isCleared;
    }
}
