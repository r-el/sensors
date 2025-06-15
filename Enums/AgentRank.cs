namespace sensors.Enums
{
    public enum AgentRank
    {
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
                AgentRank.FootSoldier => 2,
                AgentRank.SquadLeader => 4,
                AgentRank.SeniorCommander => 6,
                AgentRank.OrganizationLeader => 8,
                _ => throw new ArgumentOutOfRangeException(nameof(rank))
            };

        public static int AttackRate(this AgentRank rank)
            => rank switch
            {
                AgentRank.FootSoldier => 0, // No counterattack
                AgentRank.SquadLeader => 3,
                AgentRank.SeniorCommander => 3,
                AgentRank.OrganizationLeader => 3,
                _ => throw new ArgumentOutOfRangeException(nameof(rank))
            };

        public static bool HasCounterAttack(this AgentRank rank)
            => rank != AgentRank.FootSoldier;
    }
}
