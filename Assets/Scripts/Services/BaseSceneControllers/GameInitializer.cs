using UnityEngine;

namespace EEA.BaseSceneControllers
{
    public class GameInitializer : MonoBehaviour
    {
        [SerializeField] private SceneController sceneController;
        private async void Start()
        {
            var initialScene = sceneController.LoadScene(SceneType.InitialScene);

            await initialScene.GetComponent<ServiceManager>().Init();

            sceneController.LoadScene(SceneType.MenuScene);
        }
    }
}