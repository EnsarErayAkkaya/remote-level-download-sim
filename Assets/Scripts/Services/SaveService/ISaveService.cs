using System.Collections.Generic;
using System.Threading.Tasks;

namespace EEA.BaseServices.SaveServices
{
    public interface ISaveService
    {
        // ASYNC FUNCTIONS
        public Task<string> LoadDataAsync(string saveKey);
        public Task<float> LoadFloatDataAsync(string saveKey);

        public Task<long> LoadLongDataAsync(string saveKey);

        public Task<double> LoadDoubleDataAsync(string saveKey);

        public Task<int> LoadIntDataAsync(string saveKey);

        public Task<bool> LoadBoolDataAsync(string saveKey);

        public Task SaveDataAsync(string saveKey, string saveData);

        // SYNC FUNCTIONS
        public string LoadData(string saveKey);

        public float LoadFloatData(string saveKey);

        public long LoadLongData(string saveKey);

        public double LoadDoubleData(string saveKey);

        public int LoadIntData(string saveKey);

        public bool LoadBoolData(string saveKey);

        public void SaveData(string saveKey, string saveData);

        public bool CheckKeyExist(string saveKey);
    }
}