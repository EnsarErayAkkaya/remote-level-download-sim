using EEA.BaseServices.LevelServices;
using EEA.BaseServices.RemoteLevelServices;
using EEA.BaseServices.SaveServices;
using EEA.Utilities;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace EEA.BaseServices
{
    public class ServiceManager : MonoBehaviour
    {
        [SerializeField] private ServiceManagerSettings settings;

        public Action OnServicesReady { get; set; }

        private RemoteLevelService remoteLevelService;
        private LevelService levelService;
        private SaveService saveService;

        private static ServiceManager instance;

        #region GETTERS
        public ServiceManagerSettings Settings => settings;
        public static ILevelService LevelService => instance.levelService;
        public static IRemoteLevelService RemoteLevelService => instance.remoteLevelService;
        public static ISaveService SaveService => instance.saveService;
        public static ServiceManager Instance => instance;
        #endregion

        private void Awake()
        {
            if (instance != null && instance != this)
            {
                Destroy(instance);
            }

            instance = this;
        }

        private async void Start()
        {
            saveService = new SaveService(new EncryptedSaveHandler());

            levelService = new LevelService(settings.levelServiceSettings);

            remoteLevelService = new RemoteLevelService();

            int currentLevelIndex = levelService.GetCurrentLevelIndex();

            try
            {
                int startingLevel = currentLevelIndex + 1;

                await Backoff.DoAsync(
                        action: () => remoteLevelService.SingleDownloadLevel(startingLevel),
                        validateResult: () => remoteLevelService.DownloadLog.IsLevelDownloaded(startingLevel),
                        maxRetries: 3);
            }
            catch (Exception ex)
            {
                Debug.LogError(ex.ToString());
            }


            try
            {
                int levelRangeFrom = (currentLevelIndex - (currentLevelIndex % 25)) + 1;
                int levelRangeTo = levelRangeFrom + 50;

                _ = Backoff.DoAsync(
                    action: () => remoteLevelService.BatchDownloadLevels(levelRangeFrom, levelRangeTo),
                    validateResult: () => remoteLevelService.IsAllLevelsDownloadedInRange(levelRangeFrom, levelRangeTo),
                    maxRetries: 3);

            }
            catch (Exception ex)
            {
                Debug.LogError(ex.ToString());
            }

            OnServicesReady?.Invoke();

            SceneManager.LoadScene(1, LoadSceneMode.Additive);
        }
    }
}