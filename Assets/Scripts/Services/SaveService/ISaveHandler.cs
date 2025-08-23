using System.Threading.Tasks;

namespace EEA.BaseServices.SaveServices
{
    public interface ISaveHandler
    {
        // ASYNC
        Task<bool> SaveDataAsync(string saveKey, string saveData);
        Task<bool> AppendDataAsync(string saveKey, string saveData);

        Task<string> LoadDataAsync(string fileName);

        // SYNC
        public string LoadData(string saveKey);
        public bool SaveData(string saveKey, string saveData);
        bool AppendData(string saveKey, string saveData);

        public bool CheckKeyExist(string saveKey);
    }
}