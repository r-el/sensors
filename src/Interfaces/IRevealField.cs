using sensors.src.Models.Agents;
using sensors.src.Types.Enums;

// This interface is for sensors that can reveal a single field about an agent
namespace sensors.src.Interfaces
{
    public interface IRevealField
    {
        AgentRank RevealField(Agent agent);
    }
}
