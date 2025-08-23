using Cysharp.Threading.Tasks;
using EEA.BaseServices.LevelServices;
using System;

namespace EEA.BaseServices.RemoteLevelServices
{
    public interface IRemoteLevelService
    {
        public DownloadLog DownloadLog { get; }
        UniTask BatchDownloadLevels(int fromIndex, int toIndex);
        UniTask<BaseLevelData> DownloadLevelAsync(int levelIndex, Action<BaseLevelData> onSuccess = null, Action<string> onError = null);

        UniTask<BaseLevelData> GetDownloadedLevelDataAsync(int levelIndex);
        BaseLevelData GetDownloadedLevelData(int levelIndex);

        string GetLevelFileName(int levelIndex);
    }
}