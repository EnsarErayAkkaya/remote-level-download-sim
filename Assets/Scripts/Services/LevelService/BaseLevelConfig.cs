using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace EEA.BaseServices.LevelServices
{
    public class BaseLevelConfig
    {
        public BaseLevelData LevelData { get; private set; }

        public BaseLevelConfig(BaseLevelData _levelData)
        {
            this.LevelData = _levelData;
        }

        public async UniTask LoadLevel()
        {
            await SceneManager.LoadSceneAsync(2, LoadSceneMode.Additive);
        }
        public async UniTask UnloadLevel()
        {
            AsyncOperation op = SceneManager.UnloadSceneAsync(2);
            AsyncOperation op1 = SceneManager.LoadSceneAsync(1, LoadSceneMode.Additive);

            await UniTask.WhenAll(op.ToUniTask(), op1.ToUniTask());
        }
    }
}
