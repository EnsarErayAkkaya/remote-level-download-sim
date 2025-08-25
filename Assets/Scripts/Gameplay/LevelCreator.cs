using EEA;
using EEA.Utilities;
using EEA.Web;
using System.Text;
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
            StringBuilder stringBuilder = ClassPool<StringBuilder>.Spawn() ?? new StringBuilder();
            stringBuilder.Clear();

            for (int i = 0; i < boardString.Length; i++)
            {
                if (boardString[i] == '1')
                {
                    stringBuilder.Append('x');
                }
                else if (boardString[i] == '0')
                {
                    stringBuilder.Append('o');
                }

                if ((i + 1) % 8 == 0)
                {
                    stringBuilder.AppendLine();
                }
                else
                {
                    stringBuilder.Append(' ');
                }
            }

            boardText.text = stringBuilder.ToString();
        }
    }
}
