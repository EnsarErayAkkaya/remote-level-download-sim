using System.Threading.Tasks;
using UnityEngine;

namespace EEA.BaseServices.LevelServices
{
    public abstract class BaseLevelConfig : ScriptableObject
    {
        public abstract BaseLevelData GetLevelData();
        public abstract Task LoadLevel();
        public abstract Task UnloadLevel();
    }
}
