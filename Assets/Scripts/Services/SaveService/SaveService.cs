using System.Globalization;
using System.Threading.Tasks;

namespace EEA.BaseServices.SaveServices
{
    public class SaveService : BaseService, ISaveService
    {
        private ISaveHandler saveHandler;

        public SaveService(ISaveHandler _saveHandler)
        {
            saveHandler = _saveHandler;
        }
        // ASYNC FUNCTIONS
        public async Task<string> LoadDataAsync(string saveKey)
        {
            return await saveHandler.LoadDataAsync(saveKey);
        }

        public async Task<float> LoadFloatDataAsync(string saveKey)
        {
            string data = await LoadDataAsync(saveKey);

            // Using InvariantCulture for consistent float parsing
            if (float.TryParse(data, NumberStyles.Float, CultureInfo.InvariantCulture, out float value))
            {
                return value;
            }

            return 0f;
        }

        public async Task<long> LoadLongDataAsync(string saveKey)
        {
            string data = await LoadDataAsync(saveKey);

            // Using InvariantCulture for consistent long parsing
            if (long.TryParse(data, NumberStyles.Integer, CultureInfo.InvariantCulture, out long value))
            {
                return value;
            }

            return 0L;
        }

        public async Task<double> LoadDoubleDataAsync(string saveKey)
        {
            string data = await LoadDataAsync(saveKey);

            // Using InvariantCulture for consistent double parsing
            if (double.TryParse(data, NumberStyles.Float, CultureInfo.InvariantCulture, out double value))
            {
                return value;
            }

            return 0d;
        }

        public async Task<int> LoadIntDataAsync(string saveKey)
        {
            string data = await LoadDataAsync(saveKey);

            // Using InvariantCulture for consistent integer parsing
            if (int.TryParse(data, NumberStyles.Integer, CultureInfo.InvariantCulture, out int value))
            {
                return value;
            }

            return 0;
        }

        public async Task<bool> LoadBoolDataAsync(string saveKey)
        {
            string data = await LoadDataAsync(saveKey);

            // No need for InvariantCulture here, as bool parsing isn't affected by culture.
            if (bool.TryParse(data, out bool value))
            {
                return value;
            }

            return false;
        }

        public async Task<bool> SaveDataAsync(string saveKey, string saveData)
        {
            return await saveHandler.SaveDataAsync(saveKey, saveData);
        }

        public async Task<bool> AppendDataAsync(string saveKey, string saveData)
        {
            return await saveHandler.AppendDataAsync(saveKey, saveData);
        }

        // SYNC FUNCTIONS
        public string LoadData(string saveKey)
        {
            return saveHandler.LoadData(saveKey);
        }

        public float LoadFloatData(string saveKey)
        {
            string data = LoadData(saveKey);

            // Using InvariantCulture for consistent float parsing
            if (float.TryParse(data, NumberStyles.Float, CultureInfo.InvariantCulture, out float value))
            {
                return value;
            }

            return 0f;
        }

        public long LoadLongData(string saveKey)
        {
            string data = LoadData(saveKey);

            // Using InvariantCulture for consistent long parsing
            if (long.TryParse(data, NumberStyles.Integer, CultureInfo.InvariantCulture, out long value))
            {
                return value;
            }

            return 0L;
        }

        public double LoadDoubleData(string saveKey)
        {
            string data = LoadData(saveKey);

            // Using InvariantCulture for consistent double parsing
            if (double.TryParse(data, NumberStyles.Float, CultureInfo.InvariantCulture, out double value))
            {
                return value;
            }

            return 0d;
        }

        public int LoadIntData(string saveKey)
        {
            string data = LoadData(saveKey);

            // Using InvariantCulture for consistent integer parsing
            if (int.TryParse(data, NumberStyles.Integer, CultureInfo.InvariantCulture, out int value))
            {
                return value;
            }

            return 0;
        }

        public bool LoadBoolData(string saveKey)
        {
            string data = LoadData(saveKey);

            if (bool.TryParse(data, out bool value))
            {
                return value;
            }

            return false;
        }

        public bool SaveData(string saveKey, string saveData)
        {
            return saveHandler.SaveData(saveKey, saveData);
        }
        
        public bool AppendData(string saveKey, string saveData)
        {
            return saveHandler.AppendData(saveKey, saveData);
        }

        public bool CheckKeyExist(string saveKey)
        {
            return saveHandler.CheckKeyExist(saveKey);
        }

    }
}
