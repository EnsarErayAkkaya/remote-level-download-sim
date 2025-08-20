using System.Threading.Tasks;
using UnityEngine.SceneManagement;
using EEA.Utilities;
using UnityEngine;

namespace EEA.BaseServices.LevelServices
{
    public class BaseLevelConfig
    {
        public BaseLevelData LevelData { get; private set; }

        public BaseLevelConfig(BaseLevelData _levelData)
        {
            this.LevelData = _levelData;
        }

        public async Task LoadLevel()
        {
            AsyncOperation op = SceneManager.LoadSceneAsync(2, LoadSceneMode.Additive);
            await op.AsTask();
        }
        public async Task UnloadLevel()
        {
            AsyncOperation op = SceneManager.UnloadSceneAsync(2);
            AsyncOperation op1 = SceneManager.LoadSceneAsync(1, LoadSceneMode.Additive);

            await Task.WhenAll(op.AsTask(), op1.AsTask());
        }
    }
}
