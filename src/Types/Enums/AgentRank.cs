using System;

namespace sensors.src.Types.Enums
{
    public enum AgentRank
    {
        None = 0,           // For players who haven't defeated any agents yet
        FootSoldier,
        SquadLeader,
        SeniorCommander,
        OrganizationLeader
    }

    public static class AgentRankExtensions
    {
        public static int RequiredSensors(this AgentRank rank)
            => rank switch
            {
                AgentRank.None => 0,
                AgentRank.FootSoldier => 2,
                AgentRank.SquadLeader => 4,
                AgentRank.SeniorCommander => 6,
                AgentRank.OrganizationLeader => 8,
                _ => throw new ArgumentOutOfRangeException(nameof(rank))
            };

        public static int AttackRate(this AgentRank rank)
            => rank switch
            {
                AgentRank.None => 0,
                AgentRank.FootSoldier => 0, // No counterattack
                AgentRank.SquadLeader => 3,
                AgentRank.SeniorCommander => 3,
                AgentRank.OrganizationLeader => 3,
                _ => throw new ArgumentOutOfRangeException(nameof(rank))
            };

        public static bool HasCounterAttack(this AgentRank rank)
            => rank != AgentRank.FootSoldier && rank != AgentRank.None;

        public static int AttackStrength(this AgentRank rank)
            => rank switch
            {
                AgentRank.None => 0,
                AgentRank.FootSoldier => 0,
                AgentRank.SquadLeader => 1,        // Removes 1 sensor
                AgentRank.SeniorCommander => 2,    // Removes 2 sensors  
                AgentRank.OrganizationLeader => 1, // Removes 1 sensor normally
                _ => throw new ArgumentOutOfRangeException(nameof(rank))
            };

        public static bool HasSpecialAbility(this AgentRank rank)
            => rank == AgentRank.OrganizationLeader; // Reset all every 10 turns

        /// <summary>
        /// Gets the special ability rate (turns) for agents with special abilities
        /// </summary>
        public static int SpecialAbilityRate(this AgentRank rank)
            => rank switch
            {
                AgentRank.OrganizationLeader => 10, // Every 10 turns
                _ => 0 // No special ability
            };

        /// <summary>
        /// Gets a description of the agent's counterattack behavior
        /// </summary>
        public static string GetCounterattackDescription(this AgentRank rank)
            => rank switch
            {
                AgentRank.None => "Not applicable",
                AgentRank.FootSoldier => "No counterattack",
                AgentRank.SquadLeader => "Every 3 turns: removes 1 sensor",
                AgentRank.SeniorCommander => "Every 3 turns: removes 2 sensors",
                AgentRank.OrganizationLeader => "Every 3 turns: removes 1 sensor | Every 10 turns: resets all weaknesses",
                _ => throw new ArgumentOutOfRangeException(nameof(rank))
            };
    }
}
