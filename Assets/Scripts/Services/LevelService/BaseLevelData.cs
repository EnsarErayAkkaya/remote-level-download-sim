using System;
using System.Text;

namespace EEA.BaseServices.LevelServices
{
    public enum LevelDifficulty
    {
        Easy = 0, Medium = 1, Hard = 2
    }
    public class BaseLevelData
    {
        public LevelDifficulty difficulty;
        public string board;

        /// <summary>
        /// Converts the board hex string to a bitwise representation.
        /// </summary>
        /// <returns></returns>
        public string BoardHexToBitwise()
        {
            StringBuilder binary = new StringBuilder();

            foreach (char c in board)
            {
                // Convert each hex digit to a 4-bit binary string
                int value = Convert.ToInt32(c.ToString(), 16);
                binary.Append(Convert.ToString(value, 2).PadLeft(4, '0'));
            }

            return binary.ToString();
        }
    }
}