using System;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;

namespace EEA.Utilities
{
    public static class FileUtilities
    {
        public static bool SaveFile(string savePath, string saveData)
        {
            try
            {
                // create the directory the file will be written to if it doesn't exist
                var dir = Path.GetDirectoryName(savePath);

                if (!string.IsNullOrEmpty(dir))
                    Directory.CreateDirectory(dir);

                File.WriteAllText(savePath, saveData);
                return true;
            }
            catch (Exception e)
            {
                Debug.LogError("Error occured when trying to save event data: " + e);

                return false;
            }
        }

        public static bool AppendFile(string savePath, string saveData)
        {
            try
            {
                // create the directory the file will be written to if it doesn't exist
                Directory.CreateDirectory(Path.GetDirectoryName(savePath));

                File.AppendAllText(savePath, saveData);

                return true;
            }
            catch (Exception ex)
            {
                Debug.LogError($"Failed to append file at {savePath}: {ex}");
                return false;
            }
        }

        public static string ReadFile(string savePath)
        {
            string dataToLoad = "";

            if (File.Exists(savePath))
            {
                try
                {
                    if (File.Exists(savePath))
                    {
                        string data = File.ReadAllText(savePath);
                        return data;
                    }
                }
                catch (Exception e)
                {
                    Debug.LogError("Error occured when trying to save event data: " + e);
                }
            }

            return dataToLoad;
        }


        public static async Task<bool> SaveFileAsync(string savePath, string saveData)
        {
            try
            {
                // create the directory the file will be written to if it doesn't exist
                Directory.CreateDirectory(Path.GetDirectoryName(savePath));

                await File.WriteAllTextAsync(savePath, saveData);

                return true;
            }
            catch (Exception ex)
            {
                UnityEngine.Debug.LogError($"Failed to save file at {savePath}: {ex}");
                return false;
            }
        }

        public static async Task<bool> AppendFileAsync(string savePath, string saveData)
        {
            try
            {
                // create the directory the file will be written to if it doesn't exist
                Directory.CreateDirectory(Path.GetDirectoryName(savePath));

                await File.AppendAllTextAsync(savePath, saveData);

                return true;
            }
            catch (Exception ex)
            {
                UnityEngine.Debug.LogError($"Failed to append file at {savePath}: {ex}");
                return false;
            }
        }

        public static async Task<string> ReadFileAsync(string savePath)
        {
            if (File.Exists(savePath))
            {
                string data = await File.ReadAllTextAsync(savePath);
                return data;
            }

            return "";
        }

        public static bool CheckFileExist(string savePath)
        {
            return File.Exists(savePath);
        }
    }
}