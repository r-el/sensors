namespace sensors.src.Types.Results
{
    /// <summary>
    /// Represents the result of attempting to match a sensor against an agent's weaknesses
    /// </summary>
    public enum MatchResult
    {
        /// <summary>
        /// Sensor successfully matched against agent's weakness
        /// </summary>
        Match = 1,
        
        /// <summary>
        /// Sensor did not match any of the agent's weaknesses
        /// </summary>
        NoMatch = 0,
        
        /// <summary>
        /// Sensor type matches a weakness, but all instances have already been matched
        /// </summary>
        AlreadyMatched = 2
    }

    /// <summary>
    /// Extension methods for MatchResult enum
    /// </summary>
    public static class MatchResultExtensions
    {
        /// <summary>
        /// Check if the match result represents a successful match
        /// </summary>
        public static bool IsSuccess(this MatchResult result)
            => result == MatchResult.Match;

        /// <summary>
        /// Check if the match result represents a failure (no match or already matched)
        /// </summary>
        public static bool IsFailure(this MatchResult result)
            => result != MatchResult.Match;

        /// <summary>
        /// Get a user-friendly description of the match result
        /// </summary>
        public static string GetDescription(this MatchResult result)
            => result switch
            {
                MatchResult.Match => "Sensor matches agent weakness",
                MatchResult.NoMatch => "Sensor does not match any weakness",
                MatchResult.AlreadyMatched => "All instances of this sensor type have been used",
                _ => "Unknown match result"
            };

        /// <summary>
        /// Get a short display text for UI
        /// </summary>
        public static string GetDisplayText(this MatchResult result)
            => result switch
            {
                MatchResult.Match => "MATCH",
                MatchResult.NoMatch => "NO MATCH",
                MatchResult.AlreadyMatched => "ALREADY MATCHED",
                _ => "UNKNOWN"
            };
    }
}
