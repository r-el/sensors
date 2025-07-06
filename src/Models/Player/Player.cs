using System;
using sensors.src.Types.Enums;

namespace sensors.src.Models.Player
{
    /// <summary>
    /// Represents a player in the investigation game.
    /// Stores player progress and achievements.
    /// </summary>
    public class Player(string name, int gamesPlayed = 0, int gamesWon = 0, AgentRank? highestRankDefeated = AgentRank.FootSoldier, DateTime? lastPlayed = null)
    {
        public string Name { get; set; } = name ?? throw new ArgumentNullException(nameof(name));
        public int GamesPlayed { get; set; } = gamesPlayed;
        public int GamesWon { get; set; } = gamesWon;
        public AgentRank? HighestRankDefeated { get; set; } = highestRankDefeated;
        public DateTime LastPlayed { get; set; } = lastPlayed ?? DateTime.Now;

        public double WinRate => GamesPlayed > 0 ? (double)GamesWon / GamesPlayed * 100 : 0;

        public void RecordVictory(AgentRank defeatedRank)
        {
            GamesWon++;
            if (HighestRankDefeated == null || defeatedRank > HighestRankDefeated)
                HighestRankDefeated = defeatedRank;
            LastPlayed = DateTime.Now;
        }

        public void RecordGame()
        {
            GamesPlayed++;
            LastPlayed = DateTime.Now;
        }

        public override string ToString()
            => $"Player: {Name} | Games: {GamesPlayed} | Wins: {GamesWon} | Win Rate: {WinRate:F1}% | Highest Rank Defeated: {HighestRankDefeated?.ToString() ?? "None"}"; 
    }
}
