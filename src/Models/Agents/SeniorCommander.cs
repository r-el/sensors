using sensors.src.Types.Enums;
using sensors.src.Services;

namespace sensors.src.Models.Agents
{
    /// <summary>
    /// Senior Commander agent - highest rank target with advanced counterattack capabilities.
    /// Requires 6 sensors to expose and performs devastating counterattacks every 3 turns.
    /// </summary>
    public class SeniorCommander(List<SensorType>? predefinedWeaknesses = null) 
        : CounterattackAgent(AgentRank.SeniorCommander, 6, predefinedWeaknesses ?? RandomizationService.GenerateBalancedWeaknesses(6, true))
    {

        /// <summary>
        /// Checks if counterattack should trigger on this turn.
        /// Senior Commander attacks every 3 turns (turns 3, 6, 9, etc).
        /// </summary>
        /// <param name="turnNumber">Current game turn number</param>
        /// <returns>True if counterattack should occur</returns>
        protected override bool CheckCounterattackCondition(int turnNumber)
        {
            // Counterattack every 3 turns, but not on turn 0
            return turnNumber > 0 && turnNumber % 3 == 0;
        }

        /// <summary>
        /// Executes a powerful counterattack that removes up to 2 sensors.
        /// This represents the Senior Commander's advanced defensive capabilities.
        /// </summary>
        /// <param name="turnNumber">Current turn number (for logging purposes)</param>
        public override void PerformCounterattack(int turnNumber)
        {
            // Check if there are any sensors to remove
            if (AttachedSensors.Count == 0)
            {
                Console.WriteLine("⚔️⚔️ Senior Commander attempted counterattack, but no sensors to remove!");
                return;
            }

            // Remove up to 2 sensors randomly - powerful counterattack
            var removedSensors = RandomizationService.RemoveRandomItems(AttachedSensors, 2);
            CounterattackPerformed = true;
            
            // Display counterattack results
            Console.WriteLine($"⚔️⚔️ Senior Commander powerful counterattack! Removing {removedSensors.Count} sensors!");
            foreach (var sensor in removedSensors)
            {
                Console.WriteLine($"   🗲 Removed {sensor.Name} ({sensor.Type}) sensor!");
            }
        }

        /// <summary>
        /// Returns a string representation of the Senior Commander's current status.
        /// Includes rank and exposure state for debugging and display purposes.
        /// </summary>
        /// <returns>Formatted status string</returns>
        public override string ToString()
        {
            return $"Senior Commander [{Rank}] - {(IsExposed ? "Exposed" : "Not Exposed")}";
        }
    }
}
