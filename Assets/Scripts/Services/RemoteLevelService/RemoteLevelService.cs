using Cysharp.Threading.Tasks;
using EEA.BaseServices.LevelServices;
using EEA.Utilities;
using EEA.Web;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace EEA.BaseServices.RemoteLevelServices
{
    public class RemoteLevelService : IRemoteLevelService
    {
        private const int DownloadBatchCount = 5;

        private readonly string LevelSavePath;

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
                    Backoff backoff = new Backoff();
                    _ = backoff.DoAsync(
                            action: () => BatchDownloadLevels(fromIndex, toIndex),
                            validateResult: () => IsAllLevelsDownloadedInRange(fromIndex, toIndex),
                            maxRetries: 3);
                }
                catch (Exception ex)
                {
                    Debug.LogError(ex.ToString());
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

        public bool IsAllLevelsDownloadedInRange(int fromIndex, int toIndex)
        {
            for (int i = fromIndex; i < toIndex; i++)
            {
                if (DownloadLog.IsLevelDownloaded(i) == false)
                {
                    Debug.Log($"Level {i} not downloaded in range {fromIndex}-{toIndex - 1}");
                    return false;
                }
            }

            Debug.Log($"All Level downloaded in range {fromIndex}-{toIndex - 1}");
            return true;
        }

        private async UniTask<BaseLevelData[]> DownloadLevelRange(int startIndex, int endIndex)
        {
            List<UniTask<BaseLevelData>> levelDataTasks = new();

            for (int i = startIndex; i < endIndex; i++)
            {
                levelDataTasks.Add(DownloadLevelAsync(i));
            }

            BaseLevelData[] levelDatas = await UniTask.WhenAll(levelDataTasks);

            return levelDatas;
        }

        public async UniTask<BaseLevelData> DownloadLevelAsync(int levelIndex, Action<BaseLevelData> onSuccess = null, Action<string> onError = null)
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
                        BaseLevelData levelData = JsonUtility.FromJson<BaseLevelData>(response.Data);

                        if (levelData != null)
                        {
                            // level is valid, even saving fails return result
                            onSuccess?.Invoke(levelData);

                            bool isLevelSaveSuccessfull = await ServiceManager.SaveService.SaveDataAsync(GetLevelFileName(levelIndex), response.Data);

                            if (isLevelSaveSuccessfull)
                            {
                                await DownloadLog.AppendDownloadedLevel(levelIndex);

                                Debug.Log($"Level Downloaded {levelIndex}");

                                return levelData;
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

                    Debug.LogError($"Failed to download level {levelIndex}: {response.Error}");
                    onError?.Invoke(response.Error);

                    return null;
                }
                else
                {
                    Debug.Log($"Level {levelIndex} Already Downloaded");

                    return await GetDownloadedLevelDataAsync(levelIndex);
                }
            }
            catch (Exception e)
            {
                Debug.LogError($"Failed to download level {levelIndex}: {e.ToString()}");
            }

            return null;
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
