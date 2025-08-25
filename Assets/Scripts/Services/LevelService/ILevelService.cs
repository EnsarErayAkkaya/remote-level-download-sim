using System;

namespace EEA.LevelServices
{
    public interface ILevelService
    {
        public BaseLevelConfig ActiveLevelConfig { get; }
        public int ActiveLevelIndex { get; }

        public event Action<int> OnLevelStarted;
        public event Action<int, bool> OnLevelCompleted;

        public int GetCurrentLevelIndex();
        public BaseLevelConfig GetCurrentLevelConfig();
        public void LoadLevel(int index);
        public void LoadNextLevel();
        public void UnloadLevel();
        public void LevelCompleted(bool isSuccessfull);
    }
}