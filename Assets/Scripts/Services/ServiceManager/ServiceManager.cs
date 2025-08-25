using Cysharp.Threading.Tasks;
using EEA.LevelServices;
using EEA.LoggerService;
using EEA.RemoteLevelServices;
using EEA.SaveServices;
using EEA.Utilities;
using System;
using UnityEngine;

namespace EEA
{
    public class ServiceManager : MonoBehaviour
    {
        [SerializeField] private ServiceManagerSettings settings;

        private RemoteLevelService remoteLevelService;
        private LevelService levelService;
        private SaveService saveService;

        #region GETTERS
        public ServiceManagerSettings Settings => settings;
        public static ILevelService LevelService => Instance.levelService;
        public static IRemoteLevelService RemoteLevelService => Instance.remoteLevelService;
        public static ISaveService SaveService => Instance.saveService;
        public static ServiceManager Instance { get; private set; }
        #endregion

        public async UniTask Init()
        {
            Instance = this;
            saveService = new SaveService(new SaveHandler());

            levelService = new LevelService();

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
                EEALogger.LogError(ex.ToString());
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
                EEALogger.LogError(ex.ToString());
            }
        }
    }
}