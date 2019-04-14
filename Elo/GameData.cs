using System;
using System.Collections.Generic;
using System.Linq;

namespace Elo
{
    public class GameData : Dictionary<string, List<int>>
    {
        // Use any key, because when initializing the data,
        // We assure that each key's list has the same number of elements.
        public int NumGames => this[Keys.First()].Count;

        public IEnumerable<string> Players => Keys;

        public List<(string, int)> GetResultsForGame(int gameNumber)
        {
            if (gameNumber <= 0 || gameNumber > NumGames)
            {
                throw new ArgumentException($"Number of games must be between [1 and {gameNumber}]");
            }

            var result = new List<(string, int)>();

            foreach (var kvp in this)
            {
                // Only return the game if the player participated
                if (kvp.Value[gameNumber - 1] != -1)
                {
                    result.Add((kvp.Key, kvp.Value[gameNumber - 1]));
                }
            }

            return result;
        }
    }
}
