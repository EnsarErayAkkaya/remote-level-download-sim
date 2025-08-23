using Cysharp.Threading.Tasks;
using System;
using UnityEngine;

namespace EEA.BaseServices.LevelServices
{
    public class LevelService : BaseService, ILevelService
    {
        private LevelServiceSettings settings;

        private BaseLevelConfig activeLevelConfig = null;
        private int activeLevelIndex = -1;

        private LevelLoader levelLoader = new LevelLoader();

        private const string LevelSaveKey = "current_level_index";

        #region EVENTS
        public event Action<int> OnLevelStarted;
        public event Action<int, bool> OnLevelCompleted;
        #endregion

        #region GETTERS
        public BaseLevelConfig ActiveLevelConfig => activeLevelConfig;
        public int ActiveLevelIndex => activeLevelIndex;
        public LevelServiceSettings Settings => settings;
        #endregion

        public LevelService(LevelServiceSettings _settings)
        {
            this.settings = _settings;
        }

        public int GetCurrentLevelIndex()
        {
            return PlayerPrefs.GetInt(LevelSaveKey, 0);
        }

        public BaseLevelConfig GetCurrentLevelConfig()
        {
            return GetLevelConfig(GetCurrentLevelIndex());
        }

        public BaseLevelConfig GetLevelConfig(int _index)
        {
            var levelData = levelLoader.LoadLevel(_index);

            return new BaseLevelConfig(levelData);
        }

        public async UniTask LoadNextLevel()
        {
            await LoadLevel(GetCurrentLevelIndex());
        }

        public async UniTask LoadLevel(int _index)
        {
            activeLevelConfig = GetLevelConfig(_index);

            if (activeLevelConfig == null)
            {
                return;
            }

            activeLevelIndex = _index;

            await activeLevelConfig.LoadLevel();

            OnLevelStarted?.Invoke(_index);
        }

        public async UniTask UnloadLevel()
        {
            if (activeLevelConfig != null)
            {
                await activeLevelConfig.UnloadLevel();

                activeLevelConfig = null;
                activeLevelIndex = -1;
            }
        }

        public void LevelCompleted(bool _isSuccessfull)
        {
            int currentLevelIndex = GetCurrentLevelIndex();

            if (_isSuccessfull && activeLevelIndex == currentLevelIndex)
            {
                PlayerPrefs.SetInt(LevelSaveKey, ++currentLevelIndex);
            }

            OnLevelCompleted?.Invoke(activeLevelIndex, _isSuccessfull);
            
        }
    }
}