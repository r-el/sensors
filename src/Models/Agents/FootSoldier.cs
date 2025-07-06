using System;
using sensors.src.Types.Enums;
using sensors.src.Models.Sensors;
using sensors.src.Services.Factories;
using sensors.src.Services;

namespace sensors.src.Models.Agents
{
    public class FootSoldier : Agent
    {
        public FootSoldier() : base(AgentRank.FootSoldier, 2)
        {
        }

        protected override void InitializeWeaknesses()
        {
            SecretWeaknesses = RandomizationService.GenerateRandomWeaknesses(RequiredSensorCount);
        }

        public override int ActivateInactiveSensors()
        {
            Console.WriteLine($"Activating remaining sensors on {Rank} agent...");
            return base.ActivateInactiveSensors();
        }
    }
}
