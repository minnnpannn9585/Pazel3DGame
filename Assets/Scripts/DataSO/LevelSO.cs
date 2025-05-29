using System.Collections.Generic;
using Dialogue;
using Enemy;
using Props;
using UnityEngine;

namespace DataSO
{
    [CreateAssetMenu(menuName = "ScriptableObject/Create Level_SO", fileName = "Level_SO")]
    public class LevelSO : ScriptableObject
    {
        public List<CampData> enemyCamps;
        public List<TreasureChestData> treasureChests;
        public List<DialogueEventData> dialogueEvents;
        public List<BlockWallData> blockWalls;
        public List<BlockWallData> cutScenes;
        public string sceneName;
        public Vector3 position;
        public bool respawnAtThisPosition;
    }
}
