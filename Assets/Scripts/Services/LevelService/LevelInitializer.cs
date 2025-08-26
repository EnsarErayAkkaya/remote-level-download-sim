using EEA.BaseSceneControllers;

namespace EEA.LevelServices
{
    public static class LevelInitializer
    {
        public static void LoadLevel()
        {
            SceneController.Instance.LoadScene(SceneType.GameScene);
        }
        public static void UnloadLevel()
        {
            SceneController.Instance.UnloadScene(SceneType.GameScene);
            SceneController.Instance.LoadScene(SceneType.MenuScene);
        }

    }
}