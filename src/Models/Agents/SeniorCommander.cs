using System;
using System.Collections.Generic;
using System.Linq;
using sensors.src.Types.Enums;
using sensors.src.Models.Sensors;
using sensors.src.Services.Factories;
using sensors.src.Interfaces;
using sensors.src.Services;

namespace sensors.src.Models.Agents
{
    public class SeniorCommander : CounterattackAgent
    {
        public SeniorCommander() : base(AgentRank.SeniorCommander, 6)
        {
        }

        protected override void InitializeWeaknesses()
        {
            // Use advanced weaknesses for higher challenge
            SecretWeaknesses = RandomizationService.GenerateBalancedWeaknesses(RequiredSensorCount, true);
        }

        protected override bool CheckCounterattackCondition(int turnNumber)
        {
            // Every 3 turns, not on turn 0
            return turnNumber > 0 && turnNumber % 3 == 0;
        }

        public override void PerformCounterattack(int turnNumber)
        {
            if (AttachedSensors.Count == 0)
            {
                Console.WriteLine("⚔️⚔️ Senior Commander attempted counterattack, but no sensors to remove!");
                return;
            }

            var removedSensors = RandomizationService.RemoveRandomItems(AttachedSensors, 2);
            CounterattackPerformed = true;
            
            Console.WriteLine($"⚔️⚔️ Senior Commander powerful counterattack! Removing {removedSensors.Count} sensors!");
            foreach (var sensor in removedSensors)
            {
                Console.WriteLine($"   🗲 Removed {sensor.Name} ({sensor.Type}) sensor!");
            }
        }

        public override string ToString()
        {
            return $"Senior Commander [{Rank}] - {(IsExposed ? "Exposed" : "Not Exposed")}";
        }
    }
}
