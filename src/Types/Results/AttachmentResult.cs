using System;
using sensors.src.Types.Enums;

namespace sensors.src.Types.Results
{
    public class AttachmentResult
    {
        public SensorType SensorType { get; set; }
        public bool IsMatch { get; set; }
        public string SpecialEffectMessage { get; set; } = "";
        public AttachmentStatus Status { get; set; }
        public MatchResult? MatchResult { get; set; }
        
        public AttachmentResult(SensorType sensorType, bool isMatch, AttachmentStatus status, string specialMessage = "")
        {
            SensorType = sensorType;
            IsMatch = isMatch;
            Status = status;
            SpecialEffectMessage = specialMessage;
        }

        /// <summary>
        /// Get user-friendly message for match result
        /// </summary>
        public string GetMatchMessage()
        {
            return MatchResult switch
            {
                Results.MatchResult.Match => "MATCH",
                Results.MatchResult.AlreadyMatched => "Already matched all instances of this sensor type",
                Results.MatchResult.NoMatch => "NO MATCH",
                null => IsMatch ? "MATCH" : "NO MATCH",
                _ => "Unknown result"
            };
        }
    }

    public enum AttachmentStatus
    {
        Success,
        AgentExposed,
        AlreadyExposed,
        InvalidSensor,
        SensorBroken
    }

    public static class AttachmentResultExtensions
    {
        public static string GetMessage(this AttachmentStatus result, int currentProgress = 0, int requiredProgress = 0)
            => result switch
            {
                AttachmentStatus.Success => $"Sensor attached successfully. Progress: {currentProgress}/{requiredProgress}",
                AttachmentStatus.AgentExposed => $"Well done! The agent has been exposed. ({currentProgress}/{requiredProgress})",
                AttachmentStatus.AlreadyExposed => "Agent is already exposed!",
                AttachmentStatus.InvalidSensor => "Invalid sensor provided.",
                AttachmentStatus.SensorBroken => "The sensor is broken and cannot be used.",
                _ => throw new ArgumentOutOfRangeException(nameof(result))
            };

        public static bool IsSuccess(this AttachmentStatus result)
            => result == AttachmentStatus.Success || result == AttachmentStatus.AgentExposed;

        public static bool IsFailure(this AttachmentStatus result)
            => !IsSuccess(result);
    }
}
