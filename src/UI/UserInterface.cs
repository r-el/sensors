using System;
using System.Linq;
using sensors.src.Models.Agents;
using sensors.src.Types;
using sensors.src.Types.Enums;
using sensors.src.Types.Results;
using sensors.src.Interfaces;
using sensors.src.Services;

namespace sensors.src.UI
{
    /// <summary>
    /// Handles all user interface operations.
    /// This is a static utility class.
    /// </summary>
    public static class UserInterface
    {
        public static void ShowWelcomeMessage()
        {
            ClearScreen();
            Console.WriteLine("========== Iranian Agent Investigation Game ==========\n");
            Console.WriteLine("Welcome to the Iranian Agent Investigation Game!");
            Console.WriteLine("Your mission is to find the agent's weaknesses.");
            Console.WriteLine("Use various sensors to expose the Iranian agents!\n");
        }

        public static void ShowTargetInfo(Agent agent)
        {
            ClearScreen();
            Console.WriteLine("========== Target Information ==========");
            Console.WriteLine($"Target: {agent.GetType().Name} - Rank: {agent.Rank}");
            Console.WriteLine($"Required sensors to expose: {agent.RequiredSensorCount}");
            
            if (agent.Rank.HasCounterAttack())
            {
                Console.WriteLine($"⚠️  WARNING: This agent can perform counterattacks!");
            }
            Console.WriteLine("=========================================\n");
        }

        public static void ShowVictoryMessage(Agent agent)
        {
            ClearScreen();
            Console.WriteLine("🎉 ========== MISSION ACCOMPLISHED! ========== 🎉");
            Console.WriteLine("Congratulations! You successfully completed the investigation!");
            Console.WriteLine($"Final Status: {agent}");
            Console.WriteLine("The Iranian agent has been successfully exposed!");
            Console.WriteLine("================================================\n");
        }

        public static AgentRank SelectAgentRank(PlayerManager playerManager)
        {
            Console.WriteLine("========== Select Target Agent ==========");
            Console.WriteLine("Choose the rank of the Iranian agent to investigate:");
            
            AgentRank[] availableRanks = playerManager.GetAvailableRanks();

            for (int i = 0; i < availableRanks.Length; i++)
            {
                AgentRank rank = availableRanks[i];
                string counterInfo = rank.HasCounterAttack() ? " [⚔️ Has counterattack]" : "";
                Console.WriteLine($"\"{i + 1}. {rank} (Requires {rank.RequiredSensors()} sensors){counterInfo}\"");
            }
            
            // Show locked ranks
            var allRanks = new[] { AgentRank.FootSoldier, AgentRank.SquadLeader, AgentRank.SeniorCommander, AgentRank.OrganizationLeader };
            var lockedRanks = allRanks.Where(rank => !availableRanks.Contains(rank)).ToArray();
            
            if (lockedRanks.Any())
            {
                Console.WriteLine("\n--- Locked Ranks ---");
                foreach (var lockedRank in lockedRanks)
                {
                    string lockMessage = playerManager.GetRankLockMessage(lockedRank);
                    Console.WriteLine($"   {lockedRank}: {lockMessage}");
                }
            }
            
            Console.WriteLine("==========================================\n");

            while (true)
            {
                Console.Write($"Choose agent rank (1-{availableRanks.Length}): ");
                string input = Console.ReadLine()?.Trim() ?? "";

                if (int.TryParse(input, out int choice) && choice >= 1 && choice <= availableRanks.Length)
                    return availableRanks[choice - 1];

                Console.WriteLine("❌ Invalid choice. Please try again.");
            }
        }

        /// <summary>
        /// Displays the main investigation screen.
        /// </summary>
        public static void ShowInvestigationScreen(Agent agent, GameManager gameManager)
        {
            // Don't clear screen here - it will be cleared after user input
            Console.WriteLine($"===== INVESTIGATION: IRANIAN AGENT ({agent.Rank.ToString().ToUpper()}) =====");
            
            var (current, required) = agent.GetProgress();
            Console.WriteLine($"Progress: {current}/{required} sensors matched");

            var attachedSensors = agent.GetAttachedSensors();
            string attachedSensorsText = attachedSensors.Any()
                ? string.Join(", ", attachedSensors
                    .GroupBy(s => s.Type)
                    .Select(g => g.Count() > 1 ? $"{g.Key}*{g.Count()}" : g.Key.ToString()))
                : "";
            Console.WriteLine($"Attached: [{attachedSensorsText}]\n");

            Console.WriteLine("Available Sensors:");
            SensorType[] availableTypes = Enum.GetValues<SensorType>()
                .Where(type => type != SensorType.None)
                .ToArray();

            for (int i = 0; i < availableTypes.Length; i += 2)
            {
                string left = GetSensorDisplay(i, availableTypes, gameManager);
                string right = (i + 1 < availableTypes.Length) ? GetSensorDisplay(i + 1, availableTypes, gameManager) : "";
                Console.WriteLine($"{left.PadRight(35)}{right}");
            }
            
            Console.WriteLine("\nEnter sensor number to attach and activate");
            Console.WriteLine("Commands: S-Status | H-Help | Q-Quit");
            Console.Write("> ");
        }

        /// <summary>
        /// Enhanced sensor display that shows sensor status and usage information
        /// </summary>
        private static string GetSensorDisplay(int index, SensorType[] types, GameManager gameManager)
        {
            if (index >= types.Length) return "";
            
            SensorType type = types[index];
            string emoji = type.GetEmoji();
            string availability = gameManager.GetSensorAvailabilityInfo(type);
            
            return $"{index + 1}. {emoji} {type}{availability}";
        }

        /// <summary>
        /// Shows enhanced activation result with better formatting
        /// </summary>
        public static void ShowActivationResult(AttachmentResult result, Agent agent)
        {
            Console.WriteLine();
            if (result.Status == AttachmentStatus.AgentExposed)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("🎉 AGENT EXPOSED! Mission accomplished!");
                Console.ResetColor();
            }
            else if (result.IsMatch)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"✅ {result.SensorType} sensor matched!");
                Console.ResetColor();
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"❌ {result.SensorType} sensor did not match.");
                Console.ResetColor();
            }
            
            if (!string.IsNullOrEmpty(result.SpecialEffectMessage))
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine($"💫 {result.SpecialEffectMessage}");
                Console.ResetColor();
            }
            
            Console.WriteLine();
        }

        /// <summary>
        /// Shows a simplified help screen with sensor descriptions.
        /// </summary>
        public static void ShowSensorHelp()
        {
            ClearScreen();
            Console.WriteLine("========== SENSOR GUIDE ==========");
            SensorType[] types = Enum.GetValues<SensorType>()
                .Where(type => type != SensorType.None)
                .ToArray();
            
            foreach (SensorType type in types)
            {
                Console.WriteLine($"{type.ToString().PadRight(10)}: {type.GetDescription()}");
            }
            Console.WriteLine("\n==================================");
            WaitForKeyPress();
        }

        /// <summary>
        /// Shows a detailed status of the investigation.
        /// </summary>
        public static void ShowDetailedStatus(Agent agent)
        {
            ClearScreen();
            Console.WriteLine("========== INVESTIGATION STATUS ==========");
            Console.WriteLine($"Agent: {agent.Rank} ({agent.Affiliation})");
            Console.WriteLine($"Required sensors: {agent.RequiredSensorCount}");
            
            var (current, required) = agent.GetProgress();
            Console.WriteLine($"Current progress: {current}/{required}\n");

            Console.WriteLine("Attached sensors:");
            var attachedSensors = agent.GetAttachedSensors();
            if (attachedSensors.Any())
            {
                foreach (var sensor in attachedSensors)
                {
                    string status = "Active";
                    if (sensor is IBreakable breakable)
                    {
                        status = breakable.IsBroken ? "BROKEN" : $"{breakable.UsageCount} uses completed";
                    }
                    Console.WriteLine($"- {sensor.Name} ({sensor.Type}) - {status}");
                }
            }
            else
            {
                Console.WriteLine("- None");
            }
            
            Console.WriteLine("\n========================================");
            WaitForKeyPress();
        }

        public static void ShowMessage(string message)
        {
            Console.WriteLine($"💬 {message}");
        }

        public static void ShowError(string error)
        {
            Console.WriteLine($"❌ Error: {error}");
            WaitForKeyPress();
        }

        /// <summary>
        /// Shows warning messages with special formatting
        /// </summary>
        public static void ShowWarning(string message)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(message);
            Console.ResetColor();
        }

        /// <summary>
        /// Shows current turn information
        /// </summary>
        public static void ShowTurnInfo(int turn)
        {
            Console.WriteLine($"\n--- Turn {turn} ---");
        }

        /// <summary>
        /// Shows the current progress in a clear format
        /// </summary>
        public static void ShowProgress((int current, int total) progress)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine($"📊 Current progress: {progress.current}/{progress.total} sensors matched");
            Console.ResetColor();
        }

        /// <summary>
        /// Shows updated progress specifically after counterattack
        /// </summary>
        public static void ShowUpdatedProgressAfterCounterattack((int current, int total) progress)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"⚡ Updated progress after counterattack: {progress.current}/{progress.total} sensors matched");
            Console.ResetColor();
        }

        /// <summary>
        /// Asks the player if they want to continue to investigate the next agent rank
        /// </summary>
        public static bool AskContinueToNextAgent(PlayerManager playerManager)
        {
            Console.WriteLine("\n=== Continue Investigation? ===");
            
            // Check if there are higher ranks available to unlock
            var availableRanks = playerManager.GetAvailableRanks();
            var allRanks = new[] { AgentRank.FootSoldier, AgentRank.SquadLeader, AgentRank.SeniorCommander, AgentRank.OrganizationLeader };
            
            // Find the next locked rank that could be unlocked
            AgentRank? nextUnlockableRank = null;
            foreach (var rank in allRanks)
            {
                if (!availableRanks.Contains(rank))
                {
                    nextUnlockableRank = rank;
                    break;
                }
            }
            
            if (nextUnlockableRank.HasValue)
            {
                Console.WriteLine($"🎯 Next challenge available: {nextUnlockableRank} agent!");
                Console.WriteLine("You can now investigate a higher-ranking agent.");
            }
            else
            {
                Console.WriteLine("🏆 Congratulations! You have access to all agent ranks!");
                Console.WriteLine("You can continue investigating any available agents.");
            }
            
            Console.WriteLine("\nChoose your next action:");
            Console.WriteLine("1. Continue to next investigation");
            Console.WriteLine("2. Exit game");
            Console.Write("\nEnter your choice (1-2): ");
            
            while (true)
            {
                string input = Console.ReadLine()?.Trim() ?? "";
                
                switch (input)
                {
                    case "1":
                        Console.WriteLine("Starting new investigation...\n");
                        WaitForKeyPress();
                        return true;
                    case "2":
                        Console.WriteLine("Thank you for playing! Goodbye!");
                        return false;
                    default:
                        Console.Write("Invalid choice. Please enter 1 or 2: ");
                        break;
                }
            }
        }

        public static void WaitForKeyPress()
        {
            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
        }

        public static void ClearScreen()
        {
            try
            {
                Console.Clear();
            }
            catch (System.IO.IOException)
            {
                // Can fail if not in a real terminal
            }
        }
    }
}