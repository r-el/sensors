using sensors.Core.Agents;
using sensors.Core.Enums;

namespace sensors.Services.Factories
{
    /// <summary>
    /// Factory class for creating different types of agents.
    /// Follows Open/Closed Principle - easy to extend with new agent types.
    /// </summary>
    public static class AgentFactory
    {
        /// <summary>
        /// Creates an agent based on the specified type and rank.
        /// Currently supports Iranian agents, but can be extended for other types.
        /// </summary>
        /// <param name="agentType">The type of agent to create</param>
        /// <param name="rank">The rank of the agent</param>
        /// <returns>A new agent instance</returns>
        public static BaseAgent CreateAgent(AgentType agentType, AgentRank rank)
        {
            return agentType switch
            {
                AgentType.Iranian => new IranianAgent(rank),
                // Future agent types can be added here:
                // AgentType.Russian => new RussianAgent(rank),
                // AgentType.Chinese => new ChineseAgent(rank),
                _ => throw new ArgumentException($"Unsupported agent type: {agentType}")
            };
        }

        /// <summary>
        /// Creates an Iranian agent with the specified rank.
        /// This is a convenience method for the most common case.
        /// </summary>
        /// <param name="rank">The rank of the Iranian agent</param>
        /// <returns>A new Iranian agent instance</returns>
        public static BaseAgent CreateIranianAgent(AgentRank rank)
        {
            return CreateAgent(AgentType.Iranian, rank);
        }
    }

    /// <summary>
    /// Enum for different agent types.
    /// Prepared for future extensibility.
    /// </summary>
    public enum AgentType
    {
        Iranian
        // Future types:
        // Russian,
        // Chinese,
        // Domestic
    }
}
