using System;
using sensors.src.Models.Agents;
using sensors.src.Types.Enums;

namespace sensors.src.Services.Factories
{
    /// <summary>
    /// Factory class for creating different types of agents.
    /// Follows Open/Closed Principle - easy to extend with new agent types.
    /// </summary>
    public static class AgentFactory
    {
        /// <summary>
        /// Creates an agent based on the specified rank.
        /// Currently supports only FootSoldier, but can be extended for other ranks.
        /// </summary>
        /// <param name="rank">The rank of the agent</param>
        /// <returns>A new agent instance</returns>
        public static Agent CreateAgent(AgentRank rank)
        {
            return rank switch
            {
                AgentRank.FootSoldier => new FootSoldier(),
                AgentRank.SquadLeader => new SquadLeader(),
                AgentRank.SeniorCommander => new SeniorCommander(),
                AgentRank.OrganizationLeader => new OrganizationLeader(),
                _ => throw new ArgumentException($"Unsupported agent rank: {rank}")
            };
        }

        /// <summary>
        /// Creates a FootSoldier agent.
        /// This is a convenience method for the most common case.
        /// </summary>
        /// <returns>A new FootSoldier agent instance</returns>
        public static Agent CreateFootSoldier()
        {
            return CreateAgent(AgentRank.FootSoldier);
        }
    }
}
