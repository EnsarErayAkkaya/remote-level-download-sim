using Cysharp.Threading.Tasks;
using EEA.LevelServices;
using System;

namespace EEA.RemoteLevelServices
{
    public interface IRemoteLevelService
    {
        public DownloadLog DownloadLog { get; }
        UniTask BatchDownloadLevels(int fromIndex, int toIndex);
        UniTask SingleDownloadLevel(int index);

        UniTask<bool> DownloadLevelAsync(int levelIndex);

        UniTask<BaseLevelData> GetDownloadedLevelDataAsync(int levelIndex);
        BaseLevelData GetDownloadedLevelData(int levelIndex);

        string GetLevelFileName(int levelIndex);
    }
}