using sensors.src.Interfaces;
using sensors.src.Models.Agents;
using sensors.src.Types.Enums;

namespace sensors.src.Models.Sensors
{
    // Light sensor, reveals two fields of information about the agent
    public class LightSensor : Sensor, IRevealMultipleFields
    {
        public LightSensor()
        {
            Type = SensorType.Light;
        }
        // Reveals two raw fields (rank and affiliation) about the agent
        public (AgentRank, string) RevealFields(Agent agent)
        {
            return (agent.Rank, agent.Affiliation);
        }
        // Activates the light sensor on the given agent
        public override bool Activate(Agent agent)
        {
            (AgentRank rank, string affiliation) = RevealFields(agent);
            Console.WriteLine($"Agent rank: {rank}");
            Console.WriteLine($"Affiliation: {affiliation}");
            return true;
        }
    }
}
