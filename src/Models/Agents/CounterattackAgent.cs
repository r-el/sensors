using sensors.src.Types.Enums;
using sensors.src.Interfaces;

namespace sensors.src.Models.Agents
{
    /// <summary>
    /// Abstract base class for agents that can perform counterattacks
    /// </summary>
    public abstract class CounterattackAgent(AgentRank rank, int sensorSlots, List<SensorType>? predefinedWeaknesses = null) 
        : Agent(rank, sensorSlots, predefinedWeaknesses), ICounterattack
    {
        private bool _nextCounterattackDisabled = false;
        public bool CounterattackPerformed { get; protected set; }

        public bool ShouldPerformCounterattack(int turnNumber)
        {
            if (_nextCounterattackDisabled)
            {
                _nextCounterattackDisabled = false;
                return false;
            }

            return CheckCounterattackCondition(turnNumber);
        }

        /// <summary>
        /// Each agent type implements its own counterattack condition logic
        /// </summary>
        protected abstract bool CheckCounterattackCondition(int turnNumber);

        /// <summary>
        /// Each agent type implements its own counterattack behavior
        /// </summary>
        public abstract void PerformCounterattack(int turnNumber);

        public void DisableNextCounterattack()
        {
            _nextCounterattackDisabled = true;
            Console.WriteLine($"{GetType().Name}'s next counterattack has been disabled by magnetic sensor!");
        }

        public void ResetCounterattackState()
        {
            CounterattackPerformed = false;
        }
    }
}
