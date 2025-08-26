using System.Collections.Generic;
using UnityEngine;

namespace EEA.BaseSceneControllers
{
    public enum SceneType
    {
        InitialScene, MenuScene, GameScene
    }

    public class SceneController : MonoBehaviour
    {
        private Dictionary<SceneType, GameObject> loadedScenes = new Dictionary<SceneType, GameObject>();

        private const string InitialSceneKey = "initial_scene";
        private const string MenuSceneKey = "menu_scene";
        private const string GameSceneKey = "game_scene";

        public static SceneController Instance { get; private set; }


        private void Start()
        {
            Instance = this;
        }

        private void OnDestroy()
        {
            foreach (var scenes in loadedScenes)
            {
                Destroy(scenes.Value);
            }
        }

        public GameObject LoadScene(SceneType scene)
        {
            // Find prefab
            var sceneGameobject = LoadSceneResource(scene);

            if (sceneGameobject == null)
            {
                Debug.LogError($"Scene '{name}' not found!");
                return null;
            }

            // Instantiate
            var currentScene = Instantiate(sceneGameobject);
            loadedScenes.Add(scene, currentScene);

            return currentScene;
        }

        public void UnloadScene(SceneType scene)
        {
            if (loadedScenes.TryGetValue(scene, out var go))
            {
                Destroy(go);
                loadedScenes.Remove(scene);
            }
        }

        private GameObject LoadSceneResource(SceneType sceneType)
        {
            if (sceneType == SceneType.InitialScene)
            {
                return Resources.Load<GameObject>(InitialSceneKey);
            }
            else if (sceneType == SceneType.MenuScene)
            {
                return Resources.Load<GameObject>(MenuSceneKey);
            }
            else if (sceneType == SceneType.GameScene)
            {
                return Resources.Load<GameObject>(GameSceneKey);
            }
            else
            {
                return null;
            }
        }
    }
}