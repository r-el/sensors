using System;
using sensors.Core.Enums;
using sensors.Services.Factories;

namespace sensors.Core.Agents
{
    public class IranianAgent : BaseAgent
    {
        public IranianAgent(AgentRank rank = AgentRank.FootSoldier) : base(rank)
        { InitializeWeaknesses(); }

        private void InitializeWeaknesses()
        {
            SensorType[] availableTypes = Enum.GetValues<SensorType>();
            Random rnd = new();

            int targetCount = Rank.RequiredSensors();

            for (int i = 0; i < targetCount; i++)
            {
                SensorType weaknessType = availableTypes[rnd.Next(availableTypes.Length)];
                SecretWeaknesses.Add(SensorFactory.CreateSensor(weaknessType));
            }
        }

        public override int ActivateInactiveSensors()
        {
            Console.WriteLine($"Activating remaining sensors on {Rank} agent...");
            return base.ActivateInactiveSensors();
        }
    }
}
