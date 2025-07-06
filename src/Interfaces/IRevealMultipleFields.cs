using sensors.src.Models.Agents;
using sensors.src.Types.Enums;

namespace sensors.src.Interfaces
{
    // This interface is for sensors that can reveal multiple fields about an agent
    public interface IRevealMultipleFields
    {
        (AgentRank, string) RevealFields(Agent agent);
    }
}
