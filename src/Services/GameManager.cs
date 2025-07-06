using System;
using sensors.src.Models.Agents;
using sensors.src.Models.Player;
using sensors.src.Types.Enums;
using sensors.src.Services.Factories;
using sensors.src.Types.Results;
using sensors.src.UI;
using sensors.src.Interfaces;

namespace sensors.src.Services
{
    /// <summary>
    /// Main game controller that orchestrates the investigation process
    /// </summary>
    public class GameManager
    {
        private readonly PlayerManager _playerManager;
        private readonly InvestigationManager _investigationManager;
        private int _currentTurn;

        public GameManager(PlayerManager playerManager)
        {
            _playerManager = playerManager ?? throw new ArgumentNullException(nameof(playerManager));
            _investigationManager = new InvestigationManager();
            _currentTurn = 0;
        }

        /// <summary>
        /// Get current turn number
        /// </summary>
        public int GetCurrentTurn() => _currentTurn;

        /// <summary>
        /// Start a new investigation session
        /// </summary>
        public bool StartInvestigation()
        {
            // Get current player
            var player = _playerManager.GetCurrentPlayer();
            if (player == null)
            {
                SpectreUI.ShowGameResult(false, null!, _playerManager);
                return false;
            }

            // Let player select target agent
            AgentRank selectedRank = SpectreUI.ShowAgentSelectionMenu(_playerManager);
            if (selectedRank == AgentRank.None)
                return false;

            // Create target agent
            Agent targetAgent = AgentFactory.CreateAgent(selectedRank);
            
            // Initialize investigation (without player - GameManager handles player)
            _investigationManager.InitializeInvestigation(targetAgent);
            
            // Show mission briefing
            SpectreUI.ShowTargetBriefing(targetAgent);
            
            // Run investigation loop
            bool investigationResult = RunInvestigationLoop();
            
            // Update player statistics (GameManager responsibility)
            UpdatePlayerStatistics(player, targetAgent, investigationResult);
            
            // Show final results
            SpectreUI.ShowGameResult(investigationResult, targetAgent, _playerManager);
            
            return true;
        }

        /// <summary>
        /// Main investigation loop
        /// </summary>
        private bool RunInvestigationLoop()
        {
            // Reset turn counter for new investigation
            _currentTurn = 0;
            
            while (!_investigationManager.IsInvestigationComplete())
            {
                var agent = _investigationManager.GetIranianAgent();
                if (agent == null) break;

                // Show sensor selection menu with current turn
                SensorType selectedSensor = SpectreUI.ShowSensorSelectionMenu(
                    agent, this, _currentTurn);
                
                if (selectedSensor == SensorType.None)
                    return false; // Player chose to exit
                
                // Increment turn counter
                _currentTurn++;
                
                // Deploy sensor with current turn number
                AttachmentResult result = _investigationManager.DeploySensor(selectedSensor, _currentTurn);
                
                // Handle counterattacks based on current turn
                HandleCounterattack(agent);
                
                // Show results
                SpectreUI.ShowActivationResult(result, agent);
            }
            
            return _investigationManager.IsInvestigationComplete();
        }

        /// <summary>
        /// Handle agent counterattacks based on current turn
        /// </summary>
        private void HandleCounterattack(Agent agent)
        {
            if (agent is ICounterattack counterAgent)
            {
                // Use GameManager's turn counter for counterattack logic
                if (counterAgent.ShouldPerformCounterattack(_currentTurn))
                {
                    counterAgent.PerformCounterattack(_currentTurn);
                    
                    // Show counterattack result
                    SpectreUI.ShowCounterattackResult(agent, "Agent performed counterattack!", 1);
                }
                
                // Reset counterattack state for next turn
                counterAgent.ResetCounterattackState();
            }
        }

        /// <summary>
        /// Update player statistics after investigation completion
        /// </summary>
        private void UpdatePlayerStatistics(sensors.src.Models.Player.Player player, Agent agent, bool wasSuccessful)
        {
            // Always record that a game was played
            player.RecordGame();
            
            // If successful, record victory with agent rank
            if (wasSuccessful)
            {
                player.RecordVictory(agent.Rank);
            }
        }
    }
}
