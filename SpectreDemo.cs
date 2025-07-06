using System;
using sensors.src.Services;
using sensors.src.UI;
using sensors.src.Models.Player;
using sensors.src.Services.Factories;

namespace sensors
{
    /// <summary>
    /// Demo program to showcase Spectre.Console UI capabilities
    /// Run this instead of the main program to see the enhanced UI
    /// </summary>
    class SpectreDemo
    {
        public static void RunSpectreDemo()
        {
            Console.WriteLine("=== SPECTRE.CONSOLE UI DEMO ===");
            Console.WriteLine("Press any key to start the enhanced UI demo...");
            Console.ReadKey();

            try
            {
                // Initialize services (same as main program)
                var playerManager = new PlayerManager();
                var gameManager = new GameManager(playerManager);
                var investigationManager = new InvestigationManager();

                // Show enhanced welcome screen
                SpectreUI.ShowWelcomeScreen();

                // Get player name with enhanced UI
                string playerName = SpectreUI.GetPlayerName();
                playerManager.CreatePlayer(playerName);

                // Show enhanced player stats
                SpectreUI.ShowPlayerStats(playerManager);

                // Agent selection with enhanced UI
                var selectedAgentRank = SpectreUI.ShowAgentSelectionMenu(playerManager);
                
                if (selectedAgentRank == sensors.src.Types.Enums.AgentRank.None)
                {
                    Console.WriteLine("Demo ended. Goodbye!");
                    return;
                }

                // Create agent and start investigation demo
                var agent = AgentFactory.CreateAgent(selectedAgentRank);
                int turn = 1;

                Console.WriteLine($"\nDemo: Starting investigation of {agent.GetType().Name}");
                Console.WriteLine("Press any key to continue to sensor selection...");
                Console.ReadKey();

                // Sensor selection with enhanced UI
                var selectedSensor = SpectreUI.ShowSensorSelectionMenu(agent, gameManager, turn);
                
                if (selectedSensor != sensors.src.Types.Enums.SensorType.None)
                {
                    // Simulate investigation result
                    SpectreUI.ShowInvestigationResult(
                        selectedSensor.ToString(), 
                        "Demo investigation completed successfully! This would normally show real sensor data.",
                        false);

                    // Show game result demo
                    SpectreUI.ShowGameResult(true, turn);
                }

                Console.WriteLine("\n=== DEMO COMPLETED ===");
                Console.WriteLine("This demo showcased the enhanced UI capabilities.");
                Console.WriteLine("Your original SimpleUI.cs remains unchanged!");
                Console.WriteLine("\nPress any key to exit...");
                Console.ReadKey();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Demo error: {ex.Message}");
                Console.WriteLine("Press any key to exit...");
                Console.ReadKey();
            }
        }
    }
}
