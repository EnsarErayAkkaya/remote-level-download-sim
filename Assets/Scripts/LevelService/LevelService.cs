using System;
using System.Threading.Tasks;
using UnityEngine;

namespace EEA.BaseServices.LevelServices
{
    public class LevelService : BaseService, ILevelService
    {
        private LevelServiceSettings settings;

        private BaseLevelConfig activeLevelConfig = null;
        private int activeLevelIndex = -1;

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
            return null;
        }

        public BaseLevelConfig GetLevelConfig(int index)
        {
            return null;
        }
        public async Task LoadNextLevel()
        {
            await LoadLevel(GetCurrentLevelIndex());
        }

        public async Task LoadLevel(int index)
        {
            BaseLevelConfig levelConfig = GetLevelConfig(index);

            if (levelConfig == null)
            {
                return;
            }

            activeLevelConfig = levelConfig;
            activeLevelIndex = index;

            await levelConfig.LoadLevel();

            OnLevelStarted?.Invoke(index);
        }

        public async Task UnloadLevel()
        {
            if (activeLevelConfig != null)
            {
                await activeLevelConfig.UnloadLevel();

                activeLevelConfig = null;
                activeLevelIndex = -1;
            }
        }

        public void LevelCompleted(bool isSuccessfull)
        {
            int currentLevelIndex = GetCurrentLevelIndex();

            if (isSuccessfull && activeLevelIndex == currentLevelIndex)
            {
                PlayerPrefs.SetInt(LevelSaveKey, ++currentLevelIndex);
            }

            OnLevelCompleted?.Invoke(activeLevelIndex, isSuccessfull);
            
        }
    }
}