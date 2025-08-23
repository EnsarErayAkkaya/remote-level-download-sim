using UnityEngine;

namespace EEA.BaseServices.LevelServices
{
    public class LevelLoader
    {
        public BaseLevelData LoadLevel(int index)
        {
            try
            {
                // check if this level exists in updated levels
                var downloadedLevel = ServiceManager.RemoteLevelService.GetDownloadedLevelData(index + 1);

                if (downloadedLevel != null)
                    return downloadedLevel;

                // if not, load the level from the resources folder
                TextAsset levelData = Resources.Load<TextAsset>(GetLevelName(index + 1));

                if (levelData != null)
                {
                    BaseLevelData level = JsonUtility.FromJson<BaseLevelData>(levelData.text);

                    Resources.UnloadAsset(levelData); // Unload the asset to free memory

                    return level;
                }
            }
            catch (System.Exception e)
            {
                Debug.Log($"Load Level Failed: {e.ToString()}");
            }

            return null;
        }

        public string GetLevelName(int index) => $"Level_{index}";
    }
}