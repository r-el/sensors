using sensors.src.Interfaces;
using sensors.src.Models.Agents;
using sensors.src.Types.Enums;

namespace sensors.src.Models.Sensors
{
    // Signal sensor, reveals one field of information about the agent
    public class SignalSensor : Sensor, IRevealField
    {
        public SignalSensor()
        {
            Type = SensorType.Signal;
        }
        // Reveals the agent's rank (raw data)
        public AgentRank RevealField(Agent agent)
        {
            return agent.Rank;
        }
        // Activates the signal sensor on the given agent
        public override bool Activate(Agent agent)
        {
            AgentRank rank = RevealField(agent);
            Console.WriteLine($"Agent rank: {rank}");
            return true;
        }
    }
}

