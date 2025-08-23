using Cysharp.Threading.Tasks;
using System;

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
        public UniTask LoadLevel(int index);
        public UniTask LoadNextLevel();
        public UniTask UnloadLevel();
        public void LevelCompleted(bool isSuccessfull);
    }
}