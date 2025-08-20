using EEA.BaseServices;
using TMPro;
using UnityEngine;

namespace EEA.Gameplay
{
    public class LevelCreator : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI levelIndexText;
        [SerializeField] private TextMeshProUGUI boardText;

        private void Start()
        {
            var levelData = ServiceManager.LevelService.GetCurrentLevelConfig().LevelData;

            levelIndexText.text = $"Level {ServiceManager.LevelService.ActiveLevelIndex + 1}";

            string boardString = levelData.BoardHexToBitwise();

            // setup level
            string levelString = string.Empty;

            for (int i = 0; i < boardString.Length; i++)
            {
                if (boardString[i] == '1')
                {
                    levelString += "x";
                }
                else if (boardString[i] == '0')
                {
                    levelString += "o";
                }

                if ((i + 1) % 8 == 0)
                {
                    levelString += "\n";
                }
                else
                {
                    levelString += " ";
                }
            }

            boardText.text = levelString;
        }
    }
}
