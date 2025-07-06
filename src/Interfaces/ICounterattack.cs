namespace sensors.src.Interfaces
{
    /// <summary>
    /// Defines the contract for agents that can perform counterattacks.
    /// </summary>
    public interface ICounterattack
    {
        /// <summary>
        /// Determines if a counterattack should be performed based on turn number
        /// </summary>
        bool ShouldPerformCounterattack(int turnNumber);

        /// <summary>
        /// Performs the counterattack action based on turn number
        /// </summary>
        void PerformCounterattack(int turnNumber);

        /// <summary>
        /// Indicates if a counterattack was performed this turn
        /// </summary>
        bool CounterattackPerformed { get; }

        /// <summary>
        /// Resets the counterattack state for next turn
        /// </summary>
        void ResetCounterattackState();

        /// <summary>
        /// Disables the next counterattack (used by special sensors like Magnetic)
        /// </summary>
        void DisableNextCounterattack();
    }
}
