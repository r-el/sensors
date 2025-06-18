using System;
using sensors.Core.Agents;
using sensors.Core.Enums;
using sensors.Core.Sensors;
using sensors.Services.Factories;
using sensors.Services.Results;
using sensors.UI;

namespace sensors.Game
{
    /// <summary>
    /// Controls the game flow and coordinates between UI and game state.
    /// Follows Single Responsibility Principle - only manages game logic flow.
    /// </summary>
    public class GameController
    {
        private readonly GameState _gameState;
        private readonly UserInterface _ui;

        public GameController()
        {
            _gameState = new GameState();
            _ui = new UserInterface();
        }

        /// <summary>
        /// Main game loop - coordinates the entire investigation process.
        /// </summary>
        public void StartInvestigation()
        {
            _ui.ShowWelcomeMessage();
            
            SetupAgent();
            
            ShowGameInfo();
            
            RunGameLoop();
        }

        private void SetupAgent()
        {
            AgentRank selectedRank = _ui.SelectAgentRank();
            BaseAgent agent = AgentFactory.CreateIranianAgent(selectedRank);
            _gameState.SetAgent(agent);
        }

        private void ShowGameInfo()
        {
            if (_gameState.Agent == null) return;
            
            _ui.ShowTargetInfo(_gameState.Agent);
            _ui.ShowAvailableSensors(_gameState.AvailableTypes);
        }

        private void RunGameLoop()
        {
            while (_gameState.IsGameRunning && !_gameState.IsGameComplete)
            {
                ProcessPlayerChoice();

                if (_gameState.IsGameComplete)
                {
                    _ui.ShowVictoryMessage(_gameState.Agent!);
                    break;
                }

                RefreshGameDisplay();
            }
        }

        private void ProcessPlayerChoice()
        {
            if (_gameState.Agent == null) return;

            string input = _ui.GetPlayerInput(_gameState.AvailableTypes.Length);

            switch (input.ToUpper())
            {
                case "X":
                    HandleExitCommand();
                    break;
                case "S":
                    HandleStatusCommand();
                    break;
                case "A":
                    HandleActivateCommand();
                    break;
                default:
                    HandleSensorChoice(input);
                    break;
            }
        }

        private void HandleExitCommand()
        {
            _gameState.ExitGame();
            _ui.ShowGoodbyeMessage();
        }

        private void HandleStatusCommand()
        {
            if (_gameState.Agent == null) return;
            
            _ui.ShowGameStatus(_gameState.Agent);
            _ui.WaitForKeyPress();
        }

        private void HandleActivateCommand()
        {
            if (_gameState.Agent == null) return;
            
            int matches = _gameState.Agent.ActivateInactiveSensors();
            int required = _gameState.Agent.Rank.RequiredSensors();
            _ui.ShowActivationResults(matches, required);
            _ui.WaitForKeyPress();
        }

        private void HandleSensorChoice(string input)
        {
            if (_gameState.Agent == null) return;

            if (TryParseSensorChoice(input, out int choice))
            {
                AttachAndActivateSensor(choice);
            }
            else
            {
                _ui.ShowInvalidChoiceMessage();
                _ui.WaitForKeyPress();
            }
        }

        private bool TryParseSensorChoice(string input, out int choice)
        {
            return int.TryParse(input, out choice) && 
                   choice >= 1 && 
                   choice <= _gameState.AvailableTypes.Length;
        }

        private void AttachAndActivateSensor(int choice)
        {
            if (_gameState.Agent == null) return;

            BaseSensor newSensor = SensorFactory.CreateSensor(_gameState.AvailableTypes[choice - 1]);
            AttachmentResult result = _gameState.Agent.AttachSensor(newSensor);
            
            // Activate the sensor immediately after attachment
            newSensor.Activate();
            
            (int currentProgress, int requiredProgress) = _gameState.GetProgress();
            _ui.ShowMessage(result.GetMessage(currentProgress, requiredProgress));
            _ui.WaitForKeyPress();
        }

        private void RefreshGameDisplay()
        {
            if (_gameState.Agent == null) return;
            
            _ui.ShowTargetInfo(_gameState.Agent);
            _ui.ShowAvailableSensors(_gameState.AvailableTypes);
        }
    }
}
