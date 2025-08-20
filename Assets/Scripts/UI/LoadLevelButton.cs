using EEA.BaseServices;
using UnityEngine;

namespace EEA.UI
{
    public class LoadLevelButton : MonoBehaviour
    {
        public void OnClick()
        {
            ServiceManager.LevelService.LoadNextLevel();
        }
    }
}