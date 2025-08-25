using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace EEA.RemoteLevelServices
{
    public class DownloadLog
    {
        private static readonly SemaphoreSlim semaphore = new SemaphoreSlim(1, 1);

        private StringBuilder appendFileStringBuilder = new StringBuilder();

        public const string DownloadedLevelsSaveKey = "downloaded_level_indexes";

        public HashSet<int> downloadedLevels;

        public DownloadLog()
        {
            string data = ServiceManager.SaveService.LoadData(DownloadedLevelsSaveKey);

            if (!string.IsNullOrEmpty(data))
            {
                var parsed = data.Split(new[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries).Select(int.Parse);
                downloadedLevels = new HashSet<int>(parsed);
            }
            else
            {
                downloadedLevels = new();
            }
        }

        public async UniTask AppendDownloadedLevel(int levelIndex)
        {
            await semaphore.WaitAsync();

            try
            {
                bool appendResult = await ServiceManager.SaveService.AppendDataAsync(DownloadedLevelsSaveKey, levelIndex + "\n");

                if (appendResult)
                {
                    downloadedLevels.Add(levelIndex);
                }
            }
            finally
            {
                semaphore.Release(); // let the next writer in
            }
        }

        public async UniTask AppendDownloadedLevelRange(IEnumerable<int> indexes)
        {
            await semaphore.WaitAsync();

            try
            {
                appendFileStringBuilder.Clear();

                foreach (int index in indexes)
                {
                    appendFileStringBuilder.Append(index);
                    appendFileStringBuilder.AppendLine();
                }

                bool appendResult = await ServiceManager.SaveService.AppendDataAsync(DownloadedLevelsSaveKey, appendFileStringBuilder.ToString());

                if (appendResult)
                {
                    foreach (var index in indexes)
                    {
                        downloadedLevels.Add(index);
                    }
                }
            }
            finally
            {
                semaphore.Release(); // let the next writer in
            }
        }

        public bool IsLevelDownloaded(int levelIndex) => downloadedLevels.Contains(levelIndex);
    }
}