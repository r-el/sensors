using System;
using sensors.src.Models.Agents;
using sensors.src.Types.Enums;

namespace sensors.src.Types
{
    /// <summary>
    /// Represents the current state of the investigation game.
    /// Follows Single Responsibility Principle - only manages game state.
    /// </summary>
    public class GameState
    {
        public Agent? Agent { get; private set; }
        public bool IsGameRunning { get; set; } = true;
        public SensorType[] AvailableTypes { get; }

        public GameState()
        {
            AvailableTypes = Enum.GetValues<SensorType>();
        }

        public void SetAgent(Agent agent)
        {
            Agent = agent ?? throw new ArgumentNullException(nameof(agent));
        }

        public bool IsGameComplete => Agent?.IsExposed == true;

        public (int current, int required) GetProgress()
        {
            if (Agent == null)
                return (0, 0);
                
            return Agent.GetProgress();
        }

        public void ExitGame()
        {
            IsGameRunning = false;
        }
    }
}
