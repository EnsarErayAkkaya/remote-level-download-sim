using EEA.BaseServices;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace EEA.UI
{
    public class LoadLevelButton : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI levelText;

        private void Start()
        {
            levelText.text = $"Level {ServiceManager.LevelService.GetCurrentLevelIndex() + 1}";
        }
        public void OnClick()
        {
            SceneManager.UnloadSceneAsync(1);
            ServiceManager.LevelService.LoadNextLevel();
        }
    }
}