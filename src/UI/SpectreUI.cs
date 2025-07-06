using System;
using Spectre.Console;
using sensors.src.Services;
using sensors.src.Types.Enums;
using sensors.src.Models.Agents;
using sensors.src.Models.Sensors;
using sensors.src.Interfaces;
using sensors.src.Types.Results;
using System.Linq;

namespace sensors.src.UI
{
    /// <summary>
    /// Professional minimal UI using Spectre.Console
    /// </summary>
    public static class SpectreUI
    {
        public static void ShowWelcomeScreen()
        {
            AnsiConsole.Clear();
            AnsiConsole.Write(new Panel(new FigletText("INVESTIGATION").Centered().Color(Color.Blue))
                .Header(" 🔍 Agent Investigation System ").Border(BoxBorder.Double).BorderColor(Color.Green));
            AnsiConsole.MarkupLine("\nPress [blue]any key[/] to continue...");
            Console.ReadKey();
        }

        public static string GetPlayerName()
        {
            AnsiConsole.Clear();
            AnsiConsole.Write(new Panel("Enter agent credentials for system access")
                .Header("🔒 Agent Registration").Border(BoxBorder.Rounded).BorderColor(Color.Yellow));
            var name = AnsiConsole.Ask<string>("Agent name:");
            return string.IsNullOrWhiteSpace(name) ? "Agent" : name.Trim();
        }

        public static void ShowPlayerStats(PlayerManager playerManager)
        {
            AnsiConsole.Clear();
            var player = playerManager.GetCurrentPlayer();
            if (player == null) { AnsiConsole.MarkupLine("[red]Error: Agent not found[/]"); return; }

            var table = new Table().AddColumn("Metric").AddColumn("Value");
            table.AddRow("Agent", $"[blue]{player.Name}[/]")
                 .AddRow("Missions", $"{player.GamesPlayed}")
                 .AddRow("Successful", $"[green]{player.GamesWon}[/]")
                 .AddRow("Success Rate", $"[cyan]{player.WinRate:P0}[/]");
            table.Border(TableBorder.Minimal).Title("📊 Performance");

            AnsiConsole.Write(table);
            AnsiConsole.MarkupLine("\nPress any key to continue...");
            Console.ReadKey();
        }

        public static AgentRank ShowAgentSelectionMenu(PlayerManager playerManager)
        {
            AnsiConsole.Clear();
            var choices = new[] { "👤 Foot Soldier", "👥 Squad Leader", "🎖️ Senior Commander", "⭐ Organization Leader", "❌ Exit" };
            var agents = new[] { AgentRank.FootSoldier, AgentRank.SquadLeader, AgentRank.SeniorCommander, AgentRank.OrganizationLeader, AgentRank.None };
            
            var selection = AnsiConsole.Prompt(new SelectionPrompt<string>()
                .Title("🎯 Select Target").AddChoices(choices));
            
            return agents[Array.IndexOf(choices, selection)];
        }

        public static SensorType ShowSensorSelectionMenu(Agent agent, object gameManager, int turn)
        {
            AnsiConsole.Clear();
            
            // Enhanced mission status with counterattack warnings
            string statusText = $"Turn {turn} | Target: {agent.GetType().Name} | Status: {(agent.IsExposed ? "[green]EXPOSED[/]" : "[yellow]Active[/]")}";
            
            // Add counterattack risk warning
            if (agent is ICounterattack && turn > 0)
            {
                bool isCounterattackTurn = false;
                if (agent.Rank == AgentRank.SquadLeader && turn % 3 == 0) isCounterattackTurn = true;
                if (agent.Rank == AgentRank.SeniorCommander && turn % 3 == 0) isCounterattackTurn = true;
                if (agent.Rank == AgentRank.OrganizationLeader && (turn % 3 == 0 || turn % 10 == 0)) isCounterattackTurn = true;
                
                if (isCounterattackTurn)
                    statusText += " | [red]⚠️ COUNTERATTACK RISK[/]";
            }
            
            AnsiConsole.Write(new Panel(statusText)
                .Header("🎮 Mission Status").Border(BoxBorder.Rounded));

            // Show sensor status table for enhanced gameplay
            var table = new Table().Expand().Border(TableBorder.Minimal)
                .AddColumn("Sensor").AddColumn("Type").AddColumn("Status");
            
            var sensorTypes = new[] { SensorType.Audio, SensorType.Thermal, SensorType.Pulse, SensorType.Motion, 
                                     SensorType.Magnetic, SensorType.Signal, SensorType.Light };
                                     
            foreach (var sensorType in sensorTypes)
            {
                string emoji = SensorTypeExtensions.GetEmoji(sensorType);  // Use SensorType extension
                string typeInfo = sensorType.HasSpecialAbility() ? "[cyan]Special[/]" : "[white]Standard[/]";
                string status = GetSensorStatusDisplay(sensorType);
                table.AddRow($"{emoji} {sensorType}", typeInfo, status);
            }
            
            AnsiConsole.Write(table);

            // Sensor selection menu
            var choices = sensorTypes.Select(s => $"{SensorTypeExtensions.GetEmoji(s)} {s}").ToArray()
                         .Concat(new[] { "❓ Help", "🚪 Exit" }).ToArray();

            var selection = AnsiConsole.Prompt(new SelectionPrompt<string>()
                .Title("🔍 Select Sensor").AddChoices(choices));
            
            if (selection.IndexOf("Help") >= 0) { ShowHelpMenu(); return ShowSensorSelectionMenu(agent, gameManager, turn); }
            if (selection.IndexOf("Exit") >= 0) return SensorType.None;
            
            // Find and return selected sensor type
            return sensorTypes[Array.FindIndex(choices, c => c == selection)];
        }

        /// <summary>
        /// Get display status for each sensor type
        /// </summary>
        private static string GetSensorStatusDisplay(SensorType sensorType)
        {
            // Show usage limits for breakable sensors
            if (sensorType == SensorType.Pulse || sensorType == SensorType.Motion)
            {
                int maxUses = sensorType.GetMaxUsages();
                return $"[yellow]{maxUses} uses[/]";
            }
            
            // Show special abilities
            if (sensorType.HasSpecialAbility())
            {
                return "[green]Ready[/]";
            }
            
            // Standard sensors
            return "[green]Available[/]";
        }

        public static void ShowHelpMenu()
        {
            AnsiConsole.Clear();
            
            // Enhanced help with organized sensor information
            var table = new Table().AddColumn("Sensor").AddColumn("Type").AddColumn("Description");
            var sensors = new[] { SensorType.Audio, SensorType.Thermal, SensorType.Pulse, SensorType.Motion, 
                                 SensorType.Magnetic, SensorType.Signal, SensorType.Light };
            
            foreach (var sensor in sensors)
            {
                string sensorInfo = $"{SensorTypeExtensions.GetEmoji(sensor)} {sensor}";
                string typeInfo = sensor.HasSpecialAbility() ? "[cyan]Special[/]" : "[white]Standard[/]";
                string description = SensorTypeExtensions.GetDescription(sensor);
                
                // Add usage limit info for breakable sensors
                if (sensor == SensorType.Pulse || sensor == SensorType.Motion)
                {
                    description += $" [yellow](Max {sensor.GetMaxUsages()} uses)[/]";
                }
                
                table.AddRow(sensorInfo, typeInfo, description);
            }
            
            table.Border(TableBorder.Minimal).Title("📖 Sensor Guide");
            AnsiConsole.Write(table);
            
            // Add tactical tips
            var tips = new Panel(
                "[bold yellow]Tactical Tips:[/]\n" +
                "• Use [cyan]Thermal Sensor[/] early to reveal correct sensor types\n" +
                "• [cyan]Magnetic Sensor[/] can block 2 counterattacks - save for dangerous agents\n" +
                "• [cyan]Signal[/] and [cyan]Light Sensors[/] provide intelligence without matching\n" +
                "• [yellow]Pulse[/] and [yellow]Motion[/] sensors have limited uses - use wisely\n" +
                "• Higher rank agents counterattack more frequently and powerfully")
                .Header("💡 Strategy Guide").Border(BoxBorder.Rounded).BorderColor(Color.Yellow);
            
            AnsiConsole.Write(tips);
            AnsiConsole.MarkupLine("\nPress any key to return...");
            Console.ReadKey();
        }

        public static void ShowInvestigationResult(string sensorName, string result, bool wasCounterattacked)
        {
            AnsiConsole.Clear();
            var status = wasCounterattacked ? "[red]COMPROMISED[/]" : "[green]SUCCESS[/]";
            var icon = wasCounterattacked ? "⚠️" : "✅";
            
            AnsiConsole.Write(new Panel($"{icon} {sensorName}\n\n{result}" + 
                (wasCounterattacked ? "\n\n[red]⚡ COUNTERATTACK DETECTED[/]" : ""))
                .Header($"🔍 {status}").Border(BoxBorder.Rounded)
                .BorderColor(wasCounterattacked ? Color.Red : Color.Green));
            
            AnsiConsole.MarkupLine("\nPress any key...");
            Console.ReadKey();
        }

        public static bool ShowContinueGameMenu()
        {
            var selection = AnsiConsole.Prompt(new SelectionPrompt<string>()
                .Title("Continue?").AddChoices("Continue Mission", "Exit"));
            return selection == "Continue Mission";
        }

        public static void ShowGameResult(bool won, int turns)
        {
            AnsiConsole.Clear();
            if (won)
            {
                AnsiConsole.Write(new Panel(new FigletText("MISSION\nCOMPLETE").Centered().Color(Color.Green))
                    .Header("🎉 Success").Border(BoxBorder.Double).BorderColor(Color.Green));
                AnsiConsole.MarkupLine($"\n[green]Target neutralized in {turns} turns[/]");
            }
            else
            {
                AnsiConsole.Write(new Panel(new FigletText("MISSION\nABORTED").Centered().Color(Color.Red))
                    .Header("💀 Failed").Border(BoxBorder.Double).BorderColor(Color.Red));
                AnsiConsole.MarkupLine($"\n[red]Operation duration: {turns} turns[/]");
            }
            AnsiConsole.MarkupLine("\nPress any key...");
            Console.ReadKey();
        }

        public static void ShowActivationResult(AttachmentResult result, Agent agent)
        {
            AnsiConsole.Clear();
            var status = result.IsMatch ? "[green]SUCCESS[/]" : "[yellow]NO MATCH[/]";
            var icon = result.IsMatch ? "✅" : "⚠️";
            
            string content = $"{icon} {result.SensorType}\n\n";
            content += $"Match: {result.GetMatchMessage()}";
            
            // Only show status if it's not the default success (avoid redundancy)
            if (result.Status != AttachmentStatus.Success)
            {
                content += $"\nStatus: {result.Status}";
            }
            
            // Display special effects if any
            if (!string.IsNullOrEmpty(result.SpecialEffectMessage))
            {
                content += $"\n\n[cyan]Special Effect:[/] {result.SpecialEffectMessage}";
            }
            
            // Show progress for successful matches
            if (result.IsMatch && agent != null)
            {
                var (current, required) = agent.GetSmartProgress();
                content += $"\n\nProgress: [yellow]{current}/{required}[/]";
                
                // Celebrate target exposure
                if (agent.IsExposed)
                {
                    content += "\n\n[green]🎯 TARGET EXPOSED![/]";
                }
            }

            AnsiConsole.Write(new Panel(content)
                .Header($"🔍 {status}").Border(BoxBorder.Rounded)
                .BorderColor(result.IsMatch ? Color.Green : Color.Yellow));
            
            AnsiConsole.MarkupLine("\nPress any key...");
            Console.ReadKey();
        }

        public static void ShowTargetBriefing(Agent agent)
        {
            AnsiConsole.Clear();
            
            // Enhanced briefing with specific target information
            var briefing = $"Target Classification: [yellow]{agent.GetType().Name}[/]\n" +
                          $"Threat Level: [red]{agent.Rank}[/]\n";
            
            // Get required sensor count from agent progress
            var (current, required) = agent.GetSmartProgress();
            briefing += $"Required Sensors: [green]{required}[/]\n\n";
            
            // Add counterattack warning if applicable
            if (agent is ICounterattack)
            {
                briefing += $"[red]⚠️ COUNTERATTACK CAPABLE[/]\n";
                
                // Add specific counterattack pattern based on agent type
                if (agent.Rank == AgentRank.SquadLeader)
                    briefing += "Pattern: Every 3 turns - removes 1 sensor\n\n";
                else if (agent.Rank == AgentRank.SeniorCommander)
                    briefing += "Pattern: Every 3 turns - removes 2 sensors\n\n";
                else if (agent.Rank == AgentRank.OrganizationLeader)
                    briefing += "Pattern: Every 3 turns - removes 1 sensor, Every 10 turns - resets all\n\n";
            }
            
            // Mission objectives
            briefing += "Mission Objectives:\n" +
                       "1. Attach appropriate sensors to expose agent\n" +
                       "2. Avoid counterattacks where possible\n" +
                       "3. Use special sensor abilities strategically";

            AnsiConsole.Write(new Panel(briefing)
                .Header("📋 Mission Briefing").Border(BoxBorder.Double).BorderColor(Color.Blue));
            
            // Special equipment information table
            var specialSensors = new Table().Border(TableBorder.Rounded).BorderColor(Color.Yellow);
            specialSensors.AddColumn("Special Equipment").AddColumn("Function");
            specialSensors.AddRow($"{SensorTypeExtensions.GetEmoji(SensorType.Thermal)} Thermal", "Reveals one correct sensor type");
            specialSensors.AddRow($"{SensorTypeExtensions.GetEmoji(SensorType.Magnetic)} Magnetic", "Can block counterattacks (2 uses)");
            specialSensors.AddRow($"{SensorTypeExtensions.GetEmoji(SensorType.Signal)} Signal", "Reveals agent rank information");
            specialSensors.AddRow($"{SensorTypeExtensions.GetEmoji(SensorType.Light)} Light", "Reveals 2 fields of agent data");
            
            AnsiConsole.Write(specialSensors);
            
            AnsiConsole.MarkupLine("\nPress any key to begin investigation...");
            Console.ReadKey();
        }

        public static void ShowCounterattackWarning(string warning)
        {
            AnsiConsole.MarkupLine($"\n[red]⚠️  WARNING: {warning}[/]");
            System.Threading.Thread.Sleep(2000); // Show warning for 2 seconds
        }

        public static void ShowGameResult(bool won, Agent agent, PlayerManager playerManager)
        {
            AnsiConsole.Clear();
            if (won)
            {
                AnsiConsole.Write(new Panel(new FigletText("MISSION\nCOMPLETE").Centered().Color(Color.Green))
                    .Header("🎉 Success").Border(BoxBorder.Double).BorderColor(Color.Green));
                AnsiConsole.MarkupLine($"\n[green]Target {agent.GetType().Name} successfully exposed![/]");
                AnsiConsole.MarkupLine($"[cyan]Final Status: NEUTRALIZED[/]");
            }
            else
            {
                AnsiConsole.Write(new Panel(new FigletText("MISSION\nABORTED").Centered().Color(Color.Red))
                    .Header("💀 Failed").Border(BoxBorder.Double).BorderColor(Color.Red));
                AnsiConsole.MarkupLine($"\n[red]Target {agent.GetType().Name} remains active[/]");
            }
            
            // Show updated stats
            var player = playerManager.GetCurrentPlayer();
            if (player != null)
            {
                AnsiConsole.MarkupLine($"\nAgent Performance: {player.GamesWon}/{player.GamesPlayed} ({player.WinRate:P0})");
            }
            
            AnsiConsole.MarkupLine("\nPress any key...");
            Console.ReadKey();
        }

        /// <summary>
        /// Display counterattack results with detailed information
        /// </summary>
        public static void ShowCounterattackResult(Agent agent, string message, int removedCount = 0)
        {
            AnsiConsole.Clear();
            
            var content = $"[red]⚡ AGENT COUNTERATTACK[/]\n\n{agent.GetType().Name} performed a counterattack!\n\n";
            
            // Show specific damage based on agent type
            if (removedCount > 0)
            {
                content += $"[red]Damage:[/] {removedCount} sensor(s) removed\n";
            }
            
            // Add custom message if provided
            if (!string.IsNullOrEmpty(message))
            {
                content += $"\n{message}";
            }
            
            // Add tactical advice
            content += "\n\n[yellow]Tactical Note:[/] Consider using Magnetic Sensor to block future attacks.";
            
            AnsiConsole.Write(new Panel(content)
                .Header("⚠️ COUNTERATTACK").Border(BoxBorder.Heavy)
                .BorderColor(Color.Red));
            
            AnsiConsole.MarkupLine("\nPress any key to continue...");
            Console.ReadKey();
        }
    }
}
