using UnityEngine;

namespace EEA.BaseServices.LevelServices
{
    [CreateAssetMenu(fileName = "LevelServiceSettings", menuName = "Base Scriptable Objects/Level Service/Level Service Settings", order = 0)]
    public class LevelServiceSettings : ScriptableObject
    {
        [SerializeField] private bool loadLevelIfFirstLevelNotCompleted;
        [SerializeField] private bool startLevelSelected;
        [SerializeField] private int startingLevel;

        public bool LoadLevelIfFirstLevelNotCompleted => loadLevelIfFirstLevelNotCompleted;
        public bool StartLevelSelected => startLevelSelected;
        public int StartingLevel => startingLevel;
    }
}