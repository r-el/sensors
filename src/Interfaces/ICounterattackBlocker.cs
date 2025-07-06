/// <summary>
/// Interface for sensors that can block or affect counterattacks
/// </summary>
using sensors.src.Models.Agents;

namespace sensors.src.Interfaces
{
    public interface ICounterattackBlocker
    {
        /// <summary>
        /// Blocks the counterattack ability of the given counterattacker
        /// </summary>
        void BlockCounterattack(ICounterattack counterattacker);
        
        /// <summary>
        /// Checks if the sensor can still block counterattacks
        /// </summary>
        bool IsBlockingCounterattack();
    }
}
