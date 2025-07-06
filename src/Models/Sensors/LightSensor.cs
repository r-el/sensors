using sensors.src.Types.Enums;
using sensors.src.Interfaces;
using sensors.src.Models.Agents;

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
    }
}
