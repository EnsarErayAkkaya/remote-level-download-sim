using UnityEngine;

namespace EEA.BaseServices.LevelServices
{
    [CreateAssetMenu(fileName = "LevelServiceSettings", menuName = "Services/Level Service/Level Service Settings", order = 1)]
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