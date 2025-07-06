using sensors.src.Models.Agents;
using sensors.src.Types.Enums;

namespace sensors.src.Interfaces
{
    /// <summary>
    /// Defines the contract for sensors that can reveal information about agents
    /// </summary>
    public interface IReveal
    {
        /// <summary>
        /// Reveals information about the agent when the sensor is activated
        /// </summary>
        /// <param name="agent">The agent to reveal information about</param>
        /// <returns>The weakness revealed about the agent, or null if none</returns>
        SensorType? RevealInformation(Agent agent);
    }
}
