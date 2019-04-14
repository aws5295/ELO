using System;
using System.Collections.Generic;
using System.Linq;

namespace Elo
{
    internal class EloCalculator
    {
        private readonly GameData _gameData;
        private readonly Dictionary<string, double> _rankings;
        private readonly double _kFactor;
        private readonly double _startingScore;

        // Constructor
        public EloCalculator(IDataProvider dataProvider, double kFactor, double startingScore)
        {
            _gameData = dataProvider.GetData();
            _kFactor = kFactor;
            _startingScore = startingScore;
        }

        public IReadOnlyDictionary<string, double> GetPlayerRankings()
        {
            var rankings = _gameData.Players.ToDictionary(p => p, _ => _startingScore);

            // Incrementally score each game played
            for (int i = 1; i <= _gameData.NumGames; i++)
            {
                var gameResults = _gameData.GetResultsForGame(i);
                UpdateRankings(rankings, gameResults);
            }

            return rankings;
        }

        private void UpdateRankings(Dictionary<string, double> rankings, List<(string, int)> results)
        {
            // Calculate each player's expceted score relative to the ranking each opponent
            var expectedScore = new Dictionary<string, double>();
            foreach (var (player, result) in results)
            {
                expectedScore.Add(player, CalculateExpectedScore(player, rankings, results));
            }

            // Calculate each player's actual score based on their finishing position
            var actualScore = new Dictionary<string, double>();
            foreach (var (player, result) in results)
            {
                actualScore.Add(player, CalculateActualScore(results.Count, result));
            }

            // Calculate each player's Ranking's Delta: kFactor * (actualScore - expectedScore)
            var deltas = new Dictionary<string, double>();
            foreach (var player in expectedScore.Keys)
            {
                deltas.Add(player, (_kFactor * (actualScore[player] - expectedScore[player])));
            }

            // Update the actual rankings
            foreach (var kvp in deltas)
            {
                rankings[kvp.Key] += kvp.Value;
            }
        }

        private double CalculateActualScore(int numPlayers, int place)
        {
            int numGames = (numPlayers * (numPlayers - 1)) / 2;
            int adjustedPlace = numPlayers - place;

            return adjustedPlace / (double)numGames;
        }

        private double CalculateExpectedScore(string player,
                                              Dictionary<string, double> rankings,
                                              List<(string, int)> results)
        {
            int numPlayers = results.Count;
            int numGames = (numPlayers * (numPlayers - 1)) / 2;
            double rawExpected = 0.0;

            foreach (var (opponent, place) in results)
            {
                if (!player.Equals(opponent))
                {
                    // See how the player was expected to fare against this opponent
                    double exponent = (rankings[opponent] - rankings[player]) / 400.0;
                    double denominator = 1.0 + Math.Pow(10.0, exponent);

                    rawExpected += (1.0 / denominator);
                }
            }

            return rawExpected / numGames;
        }
    }
}
