using System;
using sensors.src.Types.Enums;

namespace sensors.src.Models.Player
{
    /// <summary>
    /// Represents a player in the investigation game.
    /// Stores player progress and achievements.
    /// </summary>
    public class Player
    {
        public string Name { get; set; }
        public int GamesPlayed { get; set; }
        public int GamesWon { get; set; }
        public AgentRank? HighestRankDefeated { get; set; }
        public DateTime LastPlayed { get; set; }
        
        public Player(string name)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            GamesPlayed = 0;
            GamesWon = 0;
            HighestRankDefeated = null;
            LastPlayed = DateTime.Now;
        }

        public double WinRate => GamesPlayed > 0 ? (double)GamesWon / GamesPlayed * 100 : 0;

        public void RecordVictory(AgentRank defeatedRank)
        {
            GamesWon++;
            if (HighestRankDefeated == null || defeatedRank > HighestRankDefeated)
            {
                HighestRankDefeated = defeatedRank;
            }
            LastPlayed = DateTime.Now;
        }

        public void RecordGame()
        {
            GamesPlayed++;
            LastPlayed = DateTime.Now;
        }

        public override string ToString()
        {
            string highestRank = HighestRankDefeated?.ToString() ?? "None";
            return $"Player: {Name} | Games: {GamesPlayed} | Wins: {GamesWon} | Win Rate: {WinRate:F1}% | Highest Rank Defeated: {highestRank}";
        }
    }
}
