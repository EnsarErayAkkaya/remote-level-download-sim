using EEA.BaseServices;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
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
                case BaseServices.LevelServices.LevelDifficulty.Easy:
                    buttonBackground.color = Color.green;
                    break;
                case BaseServices.LevelServices.LevelDifficulty.Medium:
                    buttonBackground.color = Color.yellow;
                    break;
                case BaseServices.LevelServices.LevelDifficulty.Hard:
                    buttonBackground.color = Color.red;
                    break;
                default:
                    buttonBackground.color = Color.green;
                    break;
            }
        }
        public void OnClick()
        {
            SceneManager.UnloadSceneAsync(1);
            ServiceManager.LevelService.LoadNextLevel();
        }
    }
}