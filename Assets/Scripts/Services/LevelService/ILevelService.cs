using System;

namespace EEA.LevelServices
{
    public interface ILevelService
    {
        public BaseLevelData ActiveLevelData { get; }
        public int ActiveLevelIndex { get; }

        public event Action<int, bool> OnLevelCompleted;

        public int GetCurrentLevelIndex();
        public BaseLevelData GetCurrentLevelData();
        public void LoadLevel(int index);
        public void LoadNextLevel();
        public void UnloadLevel();
        public void LevelCompleted(bool isSuccessfull);
    }
}