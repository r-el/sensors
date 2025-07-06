using System;
using System.Linq;
using sensors.src.Models.Agents;
using sensors.src.Types.Enums;
using sensors.src.Models.Sensors;
using sensors.src.Services.Factories;
using sensors.src.Types.Results;
using sensors.src.UI;
using sensors.src.Interfaces;

namespace sensors.src.Services
{
    /// <summary>
    /// Controls the game flow and coordinates between UI and game state.
    /// </summary>
    public class GameManager
    {
        private Agent? _currentAgent;
        private bool _gameRunning;
        private Sensor[] _sensors = null!;
        private int _currentTurn = 0; // Track current turn for better counterattack management
        private readonly PlayerManager _playerManager;

        public GameManager(PlayerManager playerManager)
        {
            _playerManager = playerManager ?? throw new ArgumentNullException(nameof(playerManager));
            _gameRunning = false;
            InitializeSensors();
        }

        private void InitializeSensors()
        {
            _sensors =
            [
                new AudioSensor(),      // SensorType.Audio = 1
                new ThermalSensor(),    // SensorType.Thermal = 2
                new PulseSensor(),      // SensorType.Pulse = 3
                new MotionSensor(),     // SensorType.Motion = 4
                new MagneticSensor(),   // SensorType.Magnetic = 5
                new SignalSensor(),     // SensorType.Signal = 6
                new LightSensor()       // SensorType.Light = 7
            ];
        }

        /// <summary>
        /// Main game loop - coordinates the entire investigation process.
        /// Returns true if player wants to continue to next agent, false if they want to quit.
        /// </summary>
        public bool StartInvestigation()
        {
            UserInterface.ShowWelcomeMessage();
            
            // Reset sensors for new investigation
            ResetSensors();
            
            SetupAgent();
            
            if (_currentAgent != null)
            {
                UserInterface.ShowTargetInfo(_currentAgent);
                UserInterface.WaitForKeyPress();
                return RunGameLoop();
            }
            
            return false;
        }

        private void SetupAgent()
        {
            AgentRank selectedRank = UserInterface.SelectAgentRank(_playerManager);
            _currentAgent = AgentFactory.CreateAgent(selectedRank);
            UserInterface.ShowMessage($"Iranian {selectedRank} agent captured and ready for investigation!");
            
            // Record game start
            _playerManager.RecordGameStart();
            
            UserInterface.WaitForKeyPress();
        }

        private bool RunGameLoop()
        {
            if (_currentAgent == null) return false;

            _gameRunning = true;
            _currentTurn = 0; // Reset turn counter for new investigation
            
            while (_gameRunning && !_currentAgent.IsExposed)
            {
                // Clear screen before showing new turn
                UserInterface.ClearScreen();
                
                // Show turn information and warnings (showing current turn + 1 for next action)
                UserInterface.ShowTurnInfo(_currentTurn + 1); // Show next turn number
                ShowCounterAttackWarnings();
                
                UserInterface.ShowInvestigationScreen(_currentAgent, this);
                ProcessPlayerInput(); // Turn increment is handled inside AttachAndActivateSensorByChoice
            }

            if (_currentAgent.IsExposed)
            {
                UserInterface.ShowVictoryMessage(_currentAgent);
                
                // Record victory in player manager
                _playerManager.RecordVictory(_currentAgent.Rank);
                
                // Show updated player stats
                _playerManager.ShowPlayerStats();
                
                // Ask if player wants to continue to next agent
                return UserInterface.AskContinueToNextAgent(_playerManager);
            }
            
            UserInterface.ShowMessage("Investigation ended.");
            UserInterface.WaitForKeyPress();
            return false;
        }

        /// <summary>
        /// Shows warnings about potential counterattacks
        /// </summary>
        private void ShowCounterAttackWarnings()
        {
            if (_currentAgent is ICounterattack counterattacker)
            {
                int nextTurn = _currentTurn + 1; // Check for next turn warnings
                
                // Check for regular counterattack
                if (_currentAgent.Rank.AttackRate() > 0 && nextTurn % _currentAgent.Rank.AttackRate() == 0)
                {
                    UserInterface.ShowWarning($"⚠️ Warning: {_currentAgent.Rank} agent may counterattack this turn!");
                }
                
                // Check for special ability (OrganizationLeader)
                if (_currentAgent.Rank == AgentRank.OrganizationLeader && nextTurn % 10 == 0)
                {
                    UserInterface.ShowWarning($"🚨🚨 CRITICAL: Agent may use devastating special ability! 🚨🚨");
                }
            }
        }

        private bool ProcessPlayerInput()
        {
            if (_currentAgent == null) return false;

            string input = Console.ReadLine()?.Trim().ToUpper() ?? "";
            
            switch (input)
            {
                case "S":
                    UserInterface.ShowDetailedStatus(_currentAgent);
                    return false; // No turn advancement for status
                case "H":
                    UserInterface.ShowSensorHelp();
                    return false; // No turn advancement for help
                case "Q":
                    _gameRunning = false;
                    UserInterface.ShowMessage("Quitting investigation.");
                    return false; // No turn advancement for quit
                default:
                    if (int.TryParse(input, out int sensorChoice))
                    {
                        return AttachAndActivateSensorByChoice(sensorChoice);
                    }
                    else
                    {
                        UserInterface.ShowError("Invalid command.");
                        return false; // No turn advancement for invalid input
                    }
            }
        }

        private bool AttachAndActivateSensorByChoice(int choice)
        {
            if (_currentAgent == null) return false;

            // Get all valid sensor types
            SensorType[] availableTypes = Enum.GetValues<SensorType>()
                .Where(type => type != SensorType.None)
                .ToArray();
            
            if (choice >= 1 && choice <= availableTypes.Length)
            {
                SensorType selectedType = availableTypes[choice - 1];
                
                // Convert enum to array index
                int index = (int)selectedType - 1;
                
                // Check bounds
                if (index < 0 || index >= _sensors.Length)
                {
                    UserInterface.ShowError("Invalid sensor type!");
                    return false;
                }
                
                // Get sensor from array
                Sensor sensor = _sensors[index];
                
                // Check if sensor is broken
                if (sensor is IBreakable breakable && breakable.IsBroken)
                {
                    UserInterface.ShowError($"The {selectedType} sensor is broken and can't be used!");
                    return false;
                }
                
                // Increment turn before processing
                _currentTurn++;
                
                // Handle counterattack BEFORE sensor attachment
                HandleCounterattack();
                
                // Attach and activate sensor with current turn information
                AttachmentResult result = sensor.ActivateOn(_currentAgent, _currentTurn);
                
                // Show sensor activation results
                UserInterface.ShowActivationResult(result, _currentAgent);
                
                // Single wait for user to read all results
                UserInterface.WaitForKeyPress();
                
                return true; // Success
            }
            else
            {
                UserInterface.ShowError($"Invalid sensor choice. Please select 1-{availableTypes.Length}");
                return false; // Failed
            }
        }

        /// <summary>
        /// Returns sensor availability information with emoji and clear usage format
        /// </summary>
        public string GetSensorAvailabilityInfo(SensorType type)
        {
            int index = (int)type - 1;
            if (index < 0 || index >= _sensors.Length)
                return "";
                
            Sensor sensor = _sensors[index];
            
            // Check for magnetic sensor block count
            if (sensor is MagneticSensor magneticSensor)
            {
                int remaining = magneticSensor.MaxBlocks - magneticSensor.BlockCount;
                return $" ({remaining} blocks left)";
            }
            
            // If it's a breakable sensor, check its status
            if (sensor is IBreakable breakable)
            {
                if (breakable.IsBroken)
                    return " [💥 BROKEN]";
                else
                {
                    int remaining = breakable.MaxUsages - breakable.UsageCount;
                    return $" ({remaining} uses left)";
                }
            }
            
            return ""; // Regular sensor - always available
        }

        /// <summary>
        /// Handles counterattack and updates the display with current progress
        /// </summary>
        private void HandleCounterattack()
        {
            if (_currentAgent is ICounterattack counterAgent && 
                counterAgent.ShouldPerformCounterattack(_currentTurn))
            {
                // Perform counterattack
                counterAgent.PerformCounterattack(_currentTurn);
                
                // Show immediate feedback about counterattack
                UserInterface.ShowWarning("🔴 Agent performed a counterattack!");
                
                // Reset counterattack state
                counterAgent.ResetCounterattackState();
                
                // Note: Progress will be shown in next turn's investigation screen
            }
        }

        /// <summary>
        /// Resets all sensors to their initial state for a new investigation
        /// </summary>
        private void ResetSensors()
        {
            InitializeSensors();
        }
    }
}
