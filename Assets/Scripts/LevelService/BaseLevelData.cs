using UnityEngine;

namespace EEA.BaseServices.LevelServices
{
    [System.Serializable]
    public abstract class BaseLevelData
    {
        public string Id;
        
        public abstract string Serialize();
        public abstract T Deserialize<T>(string raw) where T : BaseLevelData;
    }
}