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
    public class OrganizationLeader : CounterattackAgent
    {
        public OrganizationLeader() : base(AgentRank.OrganizationLeader, 8)
        {
        }

        protected override void InitializeWeaknesses()
        {
            // Use most advanced weaknesses for maximum challenge
            SecretWeaknesses = RandomizationService.GenerateBalancedWeaknesses(RequiredSensorCount, true);
        }

        protected override bool CheckCounterattackCondition(int turnNumber)
        {
            // Special reset every 10 turns OR regular counterattack every 3 turns
            return turnNumber > 0 && (turnNumber % 10 == 0 || turnNumber % 3 == 0);
        }

        public override void PerformCounterattack(int turnNumber)
        {
            // Check if this is the special 10-turn reset
            if (turnNumber % 10 == 0)
            {
                PerformSpecialReset();
            }
            else
            {
                PerformRegularCounterattack();
            }
            
            CounterattackPerformed = true;
        }

        private void PerformSpecialReset()
        {
            Console.WriteLine("💀💀💀 ORGANIZATION LEADER ULTIMATE COUNTERATTACK! 💀💀💀");
            Console.WriteLine("🔄 Resetting weakness list and removing ALL sensors!");
            
            // Show which sensors are being removed
            if (AttachedSensors.Count > 0)
            {
                Console.WriteLine("   Removing attached sensors:");
                foreach (var sensor in AttachedSensors)
                {
                    Console.WriteLine($"   🗲 Removed {sensor.Name} ({sensor.Type}) sensor!");
                }
            }
            
            // Clear all attached sensors
            AttachedSensors.Clear();
            
            // Reset weakness list
            ResetMatchingState();
            InitializeWeaknesses();
            
            Console.WriteLine("   🔄 Generated new weakness pattern!");
        }

        private void PerformRegularCounterattack()
        {
            if (AttachedSensors.Count == 0)
            {
                Console.WriteLine("⚔️ Organization Leader attempted counterattack, but no sensors to remove!");
                return;
            }

            var removedSensors = RandomizationService.RemoveRandomItems(AttachedSensors, 1);
            Console.WriteLine($"⚔️ Organization Leader counterattack! Removed {removedSensors[0].Name} ({removedSensors[0].Type}) sensor!");
        }

        public override string ToString()
        {
            return $"Organization Leader [{Rank}] - {(IsExposed ? "Exposed" : "Not Exposed")}";
        }
    }
}
