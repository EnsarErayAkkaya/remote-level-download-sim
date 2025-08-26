using EEA.BaseSceneControllers;
using System;
using UnityEngine;

namespace EEA.LevelServices
{
    public class LevelService : BaseService, ILevelService
    {
        private LevelDataLoader levelLoader = new LevelDataLoader();

        private const string LevelSaveKey = "current_level_index";

        #region EVENTS
        public event Action<int, bool> OnLevelCompleted;
        #endregion

        #region GETTERS
        public BaseLevelData ActiveLevelData
        {
            get
            {
                if (ActiveLevelIndex == -1)
                    return null;

                return levelData;
            }
        }
        public int ActiveLevelIndex { get; private set; }
        #endregion

        private BaseLevelData levelData = new();

        public int GetCurrentLevelIndex()
        {
            return PlayerPrefs.GetInt(LevelSaveKey, 0) % 500; // limit to 500 fo this case
        }

        public BaseLevelData GetCurrentLevelData()
        {
            return GetLevelData(GetCurrentLevelIndex());
        }

        public BaseLevelData GetLevelData(int _index)
        {
            levelData = levelLoader.LoadLevel(_index, ref levelData);

            return levelData;
        }

        public void LoadNextLevel()
        {
            LoadLevel(GetCurrentLevelIndex());
        }

        public void LoadLevel(int _index)
        {
            GetLevelData(_index);

            ActiveLevelIndex = _index;

            if (ActiveLevelData == null)
            {
                ActiveLevelIndex = -1;
                return;
            }

            LevelInitializer.LoadLevel();
        }

        public void UnloadLevel()
        {
            if (ActiveLevelIndex != -1)
            {
                LevelInitializer.UnloadLevel();

                ActiveLevelIndex = -1;
            }
        }

        public void LevelCompleted(bool _isSuccessfull)
        {
            int currentLevelIndex = GetCurrentLevelIndex();

            if (_isSuccessfull && ActiveLevelIndex == currentLevelIndex)
            {
                PlayerPrefs.SetInt(LevelSaveKey, ++currentLevelIndex);
            }

            OnLevelCompleted?.Invoke(ActiveLevelIndex, _isSuccessfull);

        }
    }
}