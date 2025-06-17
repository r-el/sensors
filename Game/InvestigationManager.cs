using sensors.Abstracts;
using sensors.Entities;
using sensors.Enums;

namespace sensors.Game
{
    public class InvestigationManager()
    {
        private readonly SensorType[] _availableTypes = Enum.GetValues<SensorType>();
        private BaseAgent _agent = null!;
        private bool _gameRunning = true;

        public void StartInvestigation()
        {
            ClearScreen();
            Console.WriteLine("========== Investigation Game ==========\n");
            Console.WriteLine("Welcome to Investigation Game!");
            Console.WriteLine("Your mission is to find the agent's weaknesses.");
            Console.WriteLine();

            SetupAgent();

            ClearScreen();
            Console.WriteLine($"Target: {_agent}");
            Console.WriteLine($"You need to find {_agent.Rank.RequiredSensors()} correct sensors to expose the agent.");
            Console.WriteLine();

            DisplayAvailableSensors();

            while (_gameRunning && !_agent.IsExposed)
            {
                ProcessPlayerChoice();

                if (_agent.IsExposed)
                {
                    ClearScreen();
                    Console.WriteLine("Congratulations! You've exposed the agent!");
                    Console.WriteLine("Mission accomplished!");
                    Console.WriteLine($"Final Status: {_agent}");
                    break;
                }

                ClearScreen();
                Console.WriteLine($"Target: {_agent}");
                Console.WriteLine($"You need to find {_agent.Rank.RequiredSensors()} correct sensors to expose the agent.");
                Console.WriteLine();
                DisplayAvailableSensors();
            }
        }

        private void SetupAgent()
        {
            AgentRank selectedRank = SelectAgentRank();
            _agent = new IranianAgent(selectedRank);
        }

        private AgentRank SelectAgentRank()
        {
            AgentRank[] ranks = Enum.GetValues<AgentRank>();

            Console.WriteLine("Select agent rank:");
            foreach ((AgentRank rank, int idx) in ranks.Select((rank, idx) => (rank, idx)))
                Console.WriteLine($"{idx + 1}. {rank} (Requires {rank.RequiredSensors()} sensors)");
            Console.WriteLine();

            while (true)
            {
                Console.Write($"Choose rank (1-{ranks.Length}): ");
                string input = Console.ReadLine()?.Trim() ?? "";

                if (int.TryParse(input, out int choice) && choice >= 1 && choice <= ranks.Length)
                    return ranks[choice - 1];

                Console.WriteLine("Invalid choice. Please try again.");
            }
        }

        private void DisplayAvailableSensors()
        {
            Console.WriteLine("Available sensor types:");
            _availableTypes.Select((type, idx) => $"  {idx + 1}. {type}")
                          .ToList()
                          .ForEach(Console.WriteLine);

            Console.WriteLine();
            Console.WriteLine("Commands:");
            Console.WriteLine("  A - Activate sensors");
            Console.WriteLine("  S - Show current status");
            Console.WriteLine("  X - Exit game");
            Console.WriteLine();
        }

        private void DisplayGameStatus()
        {
            ClearScreen();
            Console.WriteLine("========== Current Status ==========");
            Console.WriteLine(_agent.ToString());
            Console.WriteLine("====================================");
        }

        private void ProcessPlayerChoice()
        {
            Console.Write($"Choose a sensor type (1-{_availableTypes.Length}), A to activate, S for status, or X to exit: ");
            string input = Console.ReadLine()?.Trim() ?? "";

            if (input.ToUpper() == "X")
            {
                _gameRunning = false;
                Console.WriteLine("Goodbye.. ");
                return;
            }

            if (input.ToUpper() == "S")
            {
                DisplayGameStatus();
                WaitForKeyPress();
                return;
            }

            if (input.ToUpper() == "A")
            {
                ActivateSensors();
                WaitForKeyPress();
                return;
            }

            if (int.TryParse(input, out int choice) && choice >= 1 && choice <= _availableTypes.Length)
            {
                Sensor newSensor = new(_availableTypes[choice - 1]);
                AttachmentResult result = _agent.AttachSensor(newSensor);

                // Activate the sensor immediately after attachment
                newSensor.Activate();

                (int currentProgress, int requiredProgress) = _agent.GetProgress();
                Console.WriteLine(result.GetMessage(currentProgress, requiredProgress));
                WaitForKeyPress();
            }
            else
            {
                Console.WriteLine("Invalid choice. Please try again.");
                WaitForKeyPress();
            }
        }

        private void ActivateSensors()
        {
            Console.WriteLine("\n--- Activating Remaining Sensors ---");
            int matches = _agent.ActivateInactiveSensors();
            int required = _agent.Rank.RequiredSensors();

            if (matches >= required)
                Console.WriteLine("You have enough matching sensors to expose the agent!");
            else
                Console.WriteLine($"Keep searching! You need {required - matches} more correct sensors.");
        }

        private static void ClearScreen() { Console.Clear(); }

        private static void WaitForKeyPress()
        {
            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
        }
    }
}
