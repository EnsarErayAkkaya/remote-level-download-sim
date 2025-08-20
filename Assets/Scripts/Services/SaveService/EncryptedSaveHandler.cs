using EEA.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace EEA.BaseServices.SaveServices
{
    public class EncryptedSaveHandler : ISaveHandler
    {
        public bool CanUpdatePartially => false;

        public async Task<string> LoadDataAsync(string saveKey)
        {
            string data = await FileUtilities.ReadFileAsync(Path.Join(Application.persistentDataPath, saveKey));

            return data;
        }

        public async Task SaveDataAsync(string saveKey, string saveData)
        {
            await FileUtilities.SaveFileAsync(Path.Join(Application.persistentDataPath, saveKey), saveData);
        }

        // SYNC METHODS
        public string LoadData(string saveKey)
        {
            string data = FileUtilities.ReadFile(Path.Join(Application.persistentDataPath, saveKey));

            return data;
        }

        public void SaveData(string saveKey, string saveData)
        {
            FileUtilities.SaveFile(Path.Join(Application.persistentDataPath, saveKey), saveData);
        }

        public bool CheckKeyExist(string saveKey)
        {

            string savePath = Path.Combine(Application.persistentDataPath, saveKey);

            return FileUtilities.CheckFileExist(savePath);
        }
    }
}