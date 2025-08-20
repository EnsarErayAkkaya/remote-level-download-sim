using UnityEngine;

namespace EEA.BaseServices.LevelServices
{
    public enum LevelDifficulty
    {
        Easy = 0, Medium = 1, Hard = 2
    }
    public class BaseLevelData
    {
        public int Index;
        public string LevelId;
        public LevelDifficulty Difficulty;
        public string BoardHex;
    }
}