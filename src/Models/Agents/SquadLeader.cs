using sensors.src.Types.Enums;
using sensors.src.Services;

namespace sensors.src.Models.Agents
{
    public class SquadLeader(List<SensorType>? predefinedWeaknesses = null) 
        : CounterattackAgent(AgentRank.SquadLeader, 4, predefinedWeaknesses ?? RandomizationService.GenerateBalancedWeaknesses(4, true))
    {

        protected override bool CheckCounterattackCondition(int turnNumber)
        {
            // Counterattack happens BEFORE sensor attachment on turns 3, 6, 9, 12...
            return turnNumber > 0 && turnNumber % 3 == 0;
        }

        public override void PerformCounterattack(int turnNumber)
        {
            if (AttachedSensors.Count == 0)
            {
                Console.WriteLine("⚔️ Squad Leader attempted counterattack, but no sensors to remove!");
                return;
            }

            var removedSensors = RandomizationService.RemoveRandomItems(AttachedSensors, 1);
            CounterattackPerformed = true;
            
            Console.WriteLine($"⚔️ Squad Leader counterattack! Removed {removedSensors[0].Name} ({removedSensors[0].Type}) sensor!");
        }

        public override string ToString()
        {
            return $"Squad Leader [{Rank}] - {(IsExposed ? "Exposed" : "Not Exposed")}";
        }
    }
}
