using System;
using System.Linq;
using sensors.Core.Agents;
using sensors.Core.Enums;

namespace sensors.UI
{
    /// <summary>
    /// Handles all user interface operations for the investigation game.
    /// Follows Single Responsibility Principle - only manages UI/UX.
    /// </summary>
    public class UserInterface
    {
        public void ShowWelcomeMessage()
        {
            ClearScreen();
            Console.WriteLine("========== Investigation Game ==========\n");
            Console.WriteLine("Welcome to Investigation Game!");
            Console.WriteLine("Your mission is to find the agent's weaknesses.");
            Console.WriteLine();
        }

        public void ShowTargetInfo(BaseAgent agent)
        {
            ClearScreen();
            Console.WriteLine($"Target: {agent}");
            Console.WriteLine($"You need to find {agent.Rank.RequiredSensors()} correct sensors to expose the agent.");
            Console.WriteLine();
        }

        public void ShowVictoryMessage(BaseAgent agent)
        {
            ClearScreen();
            Console.WriteLine("Congratulations! You've exposed the agent!");
            Console.WriteLine("Mission accomplished!");
            Console.WriteLine($"Final Status: {agent}");
        }

        public AgentRank SelectAgentRank()
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

        public void ShowAvailableSensors(SensorType[] availableTypes)
        {
            Console.WriteLine("Available sensor types:");
            availableTypes.Select((type, idx) => $"  {idx + 1}. {type}")
                         .ToList()
                         .ForEach(Console.WriteLine);
            
            Console.WriteLine();
            ShowCommands();
            Console.WriteLine();
        }

        public void ShowCommands()
        {
            Console.WriteLine("Commands:");
            Console.WriteLine("  A - Activate sensors");
            Console.WriteLine("  S - Show current status");
            Console.WriteLine("  X - Exit game");
        }

        public void ShowGameStatus(BaseAgent agent)
        {
            ClearScreen();
            Console.WriteLine("========== Current Status ==========");
            Console.WriteLine(agent.ToString());
            Console.WriteLine("====================================");
        }

        public void ShowActivationResults(int matches, int required)
        {
            Console.WriteLine("\n--- Activating Remaining Sensors ---");
            if (matches >= required)
                Console.WriteLine("You have enough matching sensors to expose the agent!");
            else
                Console.WriteLine($"Keep searching! You need {required - matches} more correct sensors.");
        }

        public string GetPlayerInput(int maxSensorChoice)
        {
            Console.Write($"Choose a sensor type (1-{maxSensorChoice}), A to activate, S for status, or X to exit: ");
            return Console.ReadLine()?.Trim() ?? "";
        }

        public void ShowMessage(string message)
        {
            Console.WriteLine(message);
        }

        public void ShowGoodbyeMessage()
        {
            Console.WriteLine("Goodbye.. ");
        }

        public void ShowInvalidChoiceMessage()
        {
            Console.WriteLine("Invalid choice. Please try again.");
        }

        public void ClearScreen() => Console.Clear();

        public void WaitForKeyPress()
        {
            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
        }
    }
}
