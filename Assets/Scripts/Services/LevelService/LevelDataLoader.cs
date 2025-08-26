using EEA.LoggerService;
using EEA.Utilities;
using EEA.Web;
using UnityEngine;

namespace EEA.LevelServices
{
    public class LevelDataLoader
    {
        public BaseLevelData LoadLevel(int index, ref BaseLevelData levelData)
        {
            try
            {
                // check if this level exists in updated levels
                var downloadedLevel = ServiceManager.RemoteLevelService.GetDownloadedLevelData(index + 1, ref levelData);

                if (downloadedLevel != null)
                    return downloadedLevel;

                // if not, load the level from the resources folder
                TextAsset levelDataText = Resources.Load<TextAsset>(GetLevelName(index + 1));

                if (levelDataText != null)
                {
                    JsonUtility.FromJsonOverwrite(levelDataText.text, levelData);

                    Resources.UnloadAsset(levelDataText); // Unload the asset to free memory

                    return levelData;
                }
            }
            catch (System.Exception e)
            {
                EEALogger.Log($"Load Level Failed: {e.ToString()}");
            }

            return null;
        }

        public string GetLevelName(int index) => $"Level_{index}";
    }
}