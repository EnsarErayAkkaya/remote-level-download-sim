using System;
using System.Threading.Tasks;

namespace EEA.BaseServices.LevelServices
{
    public interface ILevelService
    {
        public LevelServiceSettings Settings { get; }

        public BaseLevelConfig ActiveLevelConfig { get; }
        public int ActiveLevelIndex { get; }

        public event Action<int> OnLevelStarted;
        public event Action<int, bool> OnLevelCompleted;

        public int GetCurrentLevelIndex();
        public BaseLevelConfig GetCurrentLevelConfig();
        public Task LoadLevel(int index);
        public Task LoadNextLevel();
        public Task UnloadLevel();
        public void LevelCompleted(bool isSuccessfull);
    }
}