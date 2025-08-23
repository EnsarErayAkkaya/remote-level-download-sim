using System.Collections.Generic;
using System.Threading.Tasks;

namespace EEA.BaseServices.SaveServices
{
    public interface ISaveService
    {
        // ASYNC FUNCTIONS
        Task<string> LoadDataAsync(string saveKey);
        Task<float> LoadFloatDataAsync(string saveKey);

        Task<long> LoadLongDataAsync(string saveKey);

        Task<double> LoadDoubleDataAsync(string saveKey);

        Task<int> LoadIntDataAsync(string saveKey);

        Task<bool> LoadBoolDataAsync(string saveKey);

        Task<bool> SaveDataAsync(string saveKey, string saveData);
        Task<bool> AppendDataAsync(string saveKey, string saveData);

        // SYNC FUNCTIONS
        string LoadData(string saveKey);

        float LoadFloatData(string saveKey);

        long LoadLongData(string saveKey);

        double LoadDoubleData(string saveKey);

        int LoadIntData(string saveKey);

        bool LoadBoolData(string saveKey);

        bool SaveData(string saveKey, string saveData);
        bool AppendData(string saveKey, string saveData);

        bool CheckKeyExist(string saveKey);
    }
}