using EEA.Utilities;
using System;
using System.Text;

namespace EEA.LevelServices
{
    public enum LevelDifficulty
    {
        Easy = 0, Medium = 1, Hard = 2
    }

    [Serializable]
    public class BaseLevelData
    {
        public LevelDifficulty d;
        public string b;

        /// <summary>
        /// Converts the board hex string to a bitwise representation.
        /// </summary>
        /// <returns></returns>
        public string BoardHexToBitwise()
        {
            StringBuilder stringBuilder = ClassPool<StringBuilder>.Spawn() ?? new StringBuilder();

            foreach (char c in b)
            {
                // Convert each hex digit to a 4-bit binary string
                int value = Convert.ToInt32(c.ToString(), 16);
                stringBuilder.Append(Convert.ToString(value, 2).PadLeft(4, '0'));
            }

            return stringBuilder.ToString();
        }
    }
}