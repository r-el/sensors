using sensors.Abstracts;
using sensors.Enums;

namespace sensors.Entities
{
    public class IranianAgent : BaseAgent
    {
        public IranianAgent(AgentRank rank = AgentRank.FootSoldier) : base(SensorType.Audio, rank)
        { InitializeWeaknesses(); }

        private void InitializeWeaknesses()
        {
            SensorType[] availableTypes = Enum.GetValues<SensorType>();
            Random rnd = new();

            int targetCount = Rank.RequiredSensors();

            for (int i = 0; i < targetCount; i++)
            {
                SensorType weaknessType = availableTypes[rnd.Next(availableTypes.Length)];
                SecretWeaknesses.Add(new Sensor(weaknessType.ToString()));
            }
        }

        public override int Activate()
        {
            Console.WriteLine($"Iranian agent activated. Rank: {Rank}");
            return base.Activate();
        }

        public override string ToString()
            => $"Iranian Agent [{Rank}] - {(IsExposed ? "Exposed" : "Not Exposed")}";
    }
}
