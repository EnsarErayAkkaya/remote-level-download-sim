using EEA;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EEA.UI
{
    public class WinLevelButton : MonoBehaviour
    {
        public void OnClick()
        {
            ServiceManager.LevelService.LevelCompleted(true);

            ServiceManager.LevelService.UnloadLevel();
        }
    }
}