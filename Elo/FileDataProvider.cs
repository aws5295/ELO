using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Elo
{
    public interface IDataProvider
    {
        GameData GetData();
    }

    internal class FileDataProvider : IDataProvider
    {
        private readonly string _filePath;

        internal FileDataProvider(string filePath)
        {
            if (!(File.Exists(filePath)))
            {
                throw new DirectoryNotFoundException($"Invalid File Path: {filePath}");
            }

            _filePath = filePath;
        }

        /// Returns a Dictionary of Player Name-Player Position key-value pairs
        public GameData GetData()
        {
            string[] rawData = File.ReadAllLines(_filePath, Encoding.UTF8);
            if (rawData.Length < 2)
            {
                throw new InvalidDataException($"Data must have at least 1 row of Names and 1 row of results");
            }

            var result = new GameData();

            // Add Names
            var names = rawData[0].Split(',', StringSplitOptions.RemoveEmptyEntries);
            foreach (string name in names)
            {
                if (result.ContainsKey(name))
                {
                    throw new InvalidDataException($"Duplicate names not allowed in data: '{name}'!");
                }
                result[name] = new List<int>();
            }

            // Add Data for each name, -1 connotes that they did not participate
            for (int i = 1; i < rawData.Length; i++)
            {
                var gameResults = rawData[i].Split(',');
                if (gameResults.Length != names.Length)
                {
                    throw new InvalidDataException($"Missing Game results for game #{i}!");
                }

                for (int j = 0; j < names.Length; j++)
                {
                    int playerResult = string.IsNullOrWhiteSpace(gameResults[j])
                                            ? -1
                                            : int.Parse(gameResults[j]);

                    result[names[j]].Add(playerResult);
                }
            }

            return result;
        }
    }
}