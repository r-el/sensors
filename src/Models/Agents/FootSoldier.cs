using sensors.src.Types.Enums;
using sensors.src.Services;

namespace sensors.src.Models.Agents
{
    public class FootSoldier(List<SensorType>? predefinedWeaknesses = null) 
        : Agent(AgentRank.FootSoldier, 2, predefinedWeaknesses)
    {
    }
}
