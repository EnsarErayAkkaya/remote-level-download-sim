using EEA.Utilities;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;

namespace EEA.SaveServices
{
    public class SaveHandler : ISaveHandler
    {
        public async Task<string> LoadDataAsync(string saveKey)
        {
            string data = await FileUtilities.ReadFileAsync(Path.Join(Application.persistentDataPath, saveKey));

            return data;
        }

        public async Task<bool> SaveDataAsync(string saveKey, string saveData)
        {
            return await FileUtilities.SaveFileAsync(Path.Join(Application.persistentDataPath, saveKey), saveData);
        }

        public async Task<bool> AppendDataAsync(string saveKey, string saveData)
        {
            return await FileUtilities.AppendFileAsync(Path.Join(Application.persistentDataPath, saveKey), saveData);
        }

        // SYNC METHODS
        public string LoadData(string saveKey)
        {
            string data = FileUtilities.ReadFile(Path.Join(Application.persistentDataPath, saveKey));

            return data;
        }

        public bool SaveData(string saveKey, string saveData)
        {
            return FileUtilities.SaveFile(Path.Join(Application.persistentDataPath, saveKey), saveData);
        }

        public bool AppendData(string saveKey, string saveData)
        {
            return FileUtilities.AppendFile(Path.Join(Application.persistentDataPath, saveKey), saveData);
        }

        public bool CheckKeyExist(string saveKey)
        {

            string savePath = Path.Combine(Application.persistentDataPath, saveKey);

            return FileUtilities.CheckFileExist(savePath);
        }
    }
}