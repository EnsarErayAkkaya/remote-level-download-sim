using EEA.BaseSceneControllers;

namespace EEA.LevelServices
{
    public class BaseLevelConfig
    {
        public BaseLevelData LevelData { get; private set; }

        public BaseLevelConfig(BaseLevelData _levelData)
        {
            this.LevelData = _levelData;
        }

        public void LoadLevel()
        {
            SceneController.Instance.LoadScene(SceneType.GameScene);
        }
        public void UnloadLevel()
        {
            SceneController.Instance.UnloadScene(SceneType.GameScene);
            SceneController.Instance.LoadScene(SceneType.MenuScene);
        }
    }
}
