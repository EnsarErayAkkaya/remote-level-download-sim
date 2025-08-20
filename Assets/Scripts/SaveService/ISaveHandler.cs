using System.Threading.Tasks;

namespace EEA.BaseServices.SaveServices
{
    public interface ISaveHandler
    {
        public bool CanUpdatePartially { get; }

        // ASYNC
        Task SaveDataAsync(string saveKey, string saveData);
        Task<string> LoadDataAsync(string fileName);

        // SYNC
        public string LoadData(string saveKey);
        public void SaveData(string saveKey, string saveData);

        public bool CheckKeyExist(string saveKey);
    }
}