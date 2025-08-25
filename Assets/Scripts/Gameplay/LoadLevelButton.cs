using EEA.BaseSceneControllers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace EEA.UI
{
    public class LoadLevelButton : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI levelText;
        [SerializeField] private Image buttonBackground;

        private void Start()
        {
            levelText.text = $"Level {ServiceManager.LevelService.GetCurrentLevelIndex() + 1}";

            var levelConfig = ServiceManager.LevelService.GetCurrentLevelConfig();

            switch (levelConfig.LevelData.d)
            {
                case LevelServices.LevelDifficulty.Easy:
                    buttonBackground.color = Color.green;
                    break;
                case LevelServices.LevelDifficulty.Medium:
                    buttonBackground.color = Color.yellow;
                    break;
                case LevelServices.LevelDifficulty.Hard:
                    buttonBackground.color = Color.red;
                    break;
                default:
                    buttonBackground.color = Color.green;
                    break;
            }
        }
        public void OnClick()
        {
            SceneController.Instance.UnloadScene(SceneType.MenuScene);
            ServiceManager.LevelService.LoadNextLevel();
        }
    }
}