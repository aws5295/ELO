# Elo

This program applies a modified version of the [Elo rating system](https://en.wikipedia.org/wiki/Elo_rating_system) (commonly used in Chess) to multiplayer games.  The traditional rating system was designed for head-to-head games where one player is facing another.  This version is adapted for multi-player games, for example, a group of friends that routinely gets together for boardgame nights.

## Input
**File:** The input data is a CSV file with the first row containing the player names, and each subsequent row representing a game played between any subset of those players.  A score of 1 indicates that player finished first in that game, 2 means 2nd place, etc.  If N players participated in the game, a score of N+1 or greater is illegal.

An example input is [included](https://github.com/aws5295/Elo/blob/master/Elo/Samples/SampleResults.csv) in the repo:

| Adam | Bobby | Charles | Dorris | Ethan |
| ---- | ----- | ------- | ------ | ----- |
| 1 | 2 | 3 | 4 | 5 |
| 2 | 1 | 3 | 4 | 5 |
| 1 | 2 | 3 | 5 | 4 |
| 1 | 2 | 4 | 3 |`<blank>`|
| 1 | 2 |`<blank>`|`<blank>`|`<blank>`|

In the above input example, Ethan did not play game 4. Charles, Dorris, and Ethan did not play in game 5.

**Starting Score:** This is the the baseline score that all players start at.  Common values are `800`, `1200`, or `1800` for begginner, intermediate, advanced players respectively.

**K-Factor**: The K-Factor represents roughly how many points each player stands to gain or lose against equally matched opponents. A typical starting value may be around 25.

## Output
Below is the example output using the given input:
```
Rankings:
Rank #  1: Adam          1029.38
Rank #  2: Bobby          999.82
Rank #  3: Charles        997.50
Rank #  4: Dorris         990.31
Rank #  5: Ethan          982.99
```

## Possible Future Enhancements
- Allow each player to start a different starting score
- Add Unit Tests for some of the more complicated calculations
- Add a `Team Mode` where multiple teams of multiple players can be scored
  - Average the Elo Rating of each player on a team to determine the team average
  - Run the team Ratings through the original Multiplayer algorithm to see how many ratings points are gained/loss
  - Apply that ratings point gain/loss individually to each team member
