using System;

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
            // Convert the hex string to a bitwise representation
            int bitwise = Convert.ToInt32(board, 16);
            return Convert.ToString(bitwise, 2);
        }
    }
}