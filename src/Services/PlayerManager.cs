using sensors.src.Models.Player;
using sensors.src.Types.Enums;

namespace sensors.src.Services
{
    /// <summary>
    /// Manages player data and achievements.
    /// </summary>
    public class PlayerManager
    {
        private Player? _currentPlayer;
        private readonly List<Player> _players;

        public PlayerManager()
        {
            _players = new List<Player>();
        }

        public void CreatePlayer(string name)
        {
            _currentPlayer = new Player(name);
            _players.Add(_currentPlayer);
        }

        public Player? GetCurrentPlayer() => _currentPlayer;

        public void RecordGameStart()
        {
            _currentPlayer?.RecordGame();
        }

        public void RecordVictory(AgentRank defeatedRank)
        {
            _currentPlayer?.RecordVictory(defeatedRank);
        }

        public List<Player> GetTopPlayers(int count = 10)
        {
            return _players
                .OrderByDescending(p => p.WinRate)
                .ThenByDescending(p => p.GamesWon)
                .Take(count)
                .ToList();
        }

        public void ShowPlayerStats()
        {
            if (_currentPlayer == null)
            {
                Console.WriteLine("No player logged in.");
                return;
            }

            Console.WriteLine("\n=== Player Statistics ===");
            Console.WriteLine(_currentPlayer.ToString());
        }

        /// <summary>
        /// Checks if the current player can access a specific agent rank
        /// </summary>
        public bool CanAccessRank(AgentRank targetRank)
        {
            if (_currentPlayer == null) return false;

            // Always allow FootSoldier (starting rank)
            if (targetRank == AgentRank.FootSoldier) return true;

            // For other ranks, player must have defeated the previous rank
            return _currentPlayer.HighestRankDefeated != null && 
                   _currentPlayer.HighestRankDefeated >= GetPreviousRank(targetRank);
        }

        /// <summary>
        /// Gets all agent ranks available to the current player
        /// </summary>
        public AgentRank[] GetAvailableRanks()
        {
            if (_currentPlayer == null)
                return new[] { AgentRank.FootSoldier }; // Default to just FootSoldier

            var availableRanks = new List<AgentRank>();
            var allRanks = new[] { AgentRank.FootSoldier, AgentRank.SquadLeader, AgentRank.SeniorCommander, AgentRank.OrganizationLeader };

            foreach (var rank in allRanks)
            {
                if (CanAccessRank(rank))
                    availableRanks.Add(rank);
            }

            return availableRanks.ToArray();
        }

        /// <summary>
        /// Gets the previous rank in the progression hierarchy
        /// </summary>
        private static AgentRank GetPreviousRank(AgentRank rank)
        {
            return rank switch
            {
                AgentRank.SquadLeader => AgentRank.FootSoldier,
                AgentRank.SeniorCommander => AgentRank.SquadLeader,
                AgentRank.OrganizationLeader => AgentRank.SeniorCommander,
                _ => AgentRank.FootSoldier
            };
        }

        /// <summary>
        /// Gets a message explaining why a rank is locked
        /// </summary>
        public string GetRankLockMessage(AgentRank targetRank)
        {
            if (_currentPlayer == null)
                return "No player logged in.";

            if (CanAccessRank(targetRank))
                return string.Empty;

            var requiredRank = GetPreviousRank(targetRank);
            return $"🔒 Locked: Defeat {requiredRank} agent first! (Current highest: {_currentPlayer.HighestRankDefeated})";
        }
    }
}
