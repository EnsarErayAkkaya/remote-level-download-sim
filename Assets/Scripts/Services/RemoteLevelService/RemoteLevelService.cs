using Cysharp.Threading.Tasks;
using EEA.LevelServices;
using EEA.LoggerService;
using EEA.Utilities;
using EEA.Web;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace EEA.RemoteLevelServices
{
    public class RemoteLevelService : IRemoteLevelService
    {
        private const int DownloadBatchCount = 5;

        private readonly string LevelSavePath;

        private List<int> succesfullyDownloadedIndexes = new();

        public DownloadLog DownloadLog { get; private set; }

        public RemoteLevelService()
        {
            LevelSavePath = Path.Join(Directory.GetParent(Application.dataPath).FullName, "Levels");
            DownloadLog = new DownloadLog();

            ServiceManager.LevelService.OnLevelCompleted += OnLevelCompleted;
        }

        private void OnLevelCompleted(int levelIndex, bool isSuccessfull)
        {
            // every 25 level download next 50 level
            if (((levelIndex + 1) % 25) == 0)
            {
                // for example if level Index is 24 it means 25 level complted including level 0,
                // so download from 26 to 76 (excluding) -- 50 levels

                int fromIndex = levelIndex + 2;
                int toIndex = fromIndex + 50;

                try
                {
                    _ = Backoff.DoAsync(
                            action: () => BatchDownloadLevels(fromIndex, toIndex),
                            validateResult: () => IsAllLevelsDownloadedInRange(fromIndex, toIndex),
                            maxRetries: 3);
                }
                catch (Exception ex)
                {
                    EEALogger.LogError(ex.ToString());
                }
            }
        }

        public async UniTask BatchDownloadLevels(int fromIndex, int toIndex)
        {
            // mobile networks my have difficulties with large batch downloads
            // so we will download 5 levels at once
            for (int i = fromIndex; i < toIndex; i += DownloadBatchCount)
            {
                int batchEnd = Mathf.Min(i + DownloadBatchCount, toIndex);
                await DownloadLevelRange(i, batchEnd);
            }
        }

        public async UniTask SingleDownloadLevel(int index)
        {
            var isDownloaded = await DownloadLevelAsync(index);

            if (isDownloaded)
            {
                await DownloadLog.AppendDownloadedLevel(index);
            }
        }

        public bool IsAllLevelsDownloadedInRange(int fromIndex, int toIndex)
        {
            for (int i = fromIndex; i < toIndex; i++)
            {
                if (DownloadLog.IsLevelDownloaded(i) == false)
                {
                    if (ServiceManager.Instance.Settings.debugLog)
                        EEALogger.Log($"Level {i} not downloaded in range {fromIndex}-{toIndex - 1}");
                    return false;
                }
            }

            if (ServiceManager.Instance.Settings.debugLog)
                EEALogger.Log($"All Level downloaded in range {fromIndex}-{toIndex - 1}");

            return true;
        }

        private async UniTask<bool[]> DownloadLevelRange(int startIndex, int endIndex)
        {
            List<UniTask<bool>> downloadTasks = new();

            for (int i = startIndex; i < endIndex; i++)
            {
                downloadTasks.Add(DownloadLevelAsync(i));
            }

            bool[] downloadResult = await UniTask.WhenAll(downloadTasks);

            succesfullyDownloadedIndexes.Clear();

            for (int i = 0; i < downloadResult.Length; i++)
            {
                if (downloadResult[i])
                {
                    succesfullyDownloadedIndexes.Add(startIndex + i);
                }
            }

            await DownloadLog.AppendDownloadedLevelRange(succesfullyDownloadedIndexes);

            return downloadResult;
        }
        public async UniTask<bool> DownloadLevelAsync(int levelIndex)
        {
            try
            {
                if (!DownloadLog.IsLevelDownloaded(levelIndex))
                {
                    string url = Path.Join(LevelSavePath, GetLevelFileName(levelIndex));

                    // DOWNLOAD LEVEL
                    Response response = await RequestSender.GetAsync(url);

                    if (response.IsSuccess)
                    {
                        // check if level integrity valid
                        BaseLevelData levelData = JsonUtility.FromJson<BaseLevelData>(response.Data); // TODO: MESSAGE PACK

                        if (levelData != null && levelData.b.Length == 16)
                        {
                            bool isLevelSaveSuccessfull = await ServiceManager.SaveService.SaveDataAsync(GetLevelFileName(levelIndex), response.Data);

                            if (isLevelSaveSuccessfull)
                            {
                                if (ServiceManager.Instance.Settings.debugLog)
                                    EEALogger.Log($"Level Downloaded {levelIndex}");

                                return true;
                            }
                            else
                            {
                                response.Error = "Downloading and parsing was successful but couldn't save file.";
                            }
                        }
                        else
                        {
                            response.Error = "Downloading was successful but couldn't parse json.";
                        }
                    }

                    EEALogger.LogError($"Failed to download level {levelIndex}: {response.Error}");

                    return false;
                }
                else
                {
                    if (ServiceManager.Instance.Settings.debugLog)
                        EEALogger.Log($"Level {levelIndex} Already Downloaded");

                    return false;
                }
            }
            catch (Exception e)
            {
                EEALogger.LogError($"Failed to download level {levelIndex}: {e.ToString()}");
            }

            return false;
        }

        public async UniTask<BaseLevelData> GetDownloadedLevelDataAsync(int levelIndex)
        {
            if (DownloadLog.IsLevelDownloaded(levelIndex))
            {
                var data = await ServiceManager.SaveService.LoadDataAsync(GetLevelFileName(levelIndex));

                BaseLevelData levelData = JsonUtility.FromJson<BaseLevelData>(data);

                return levelData;
            }

            return null;
        }

        public BaseLevelData GetDownloadedLevelData(int levelIndex)
        {
            if (DownloadLog.IsLevelDownloaded(levelIndex))
            {
                var data = ServiceManager.SaveService.LoadData(GetLevelFileName(levelIndex));

                BaseLevelData levelData = JsonUtility.FromJson<BaseLevelData>(data);

                return levelData;
            }

            return null;
        }

        public string GetLevelFileName(int levelIndex)
        {
            return $"level_{levelIndex}_updated";
        }
    }
}
