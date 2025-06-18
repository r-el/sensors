using System;

namespace sensors.Services.Results
{
    public enum AttachmentResult
    {
        Success,
        AgentExposed,
        AlreadyExposed,
        InvalidSensor
    }

    public static class AttachmentResultExtensions
    {
        public static string GetMessage(this AttachmentResult result, int currentProgress = 0, int requiredProgress = 0)
            => result switch
            {
                AttachmentResult.Success => $"Sensor attached successfully. Progress: {currentProgress}/{requiredProgress}",
                AttachmentResult.AgentExposed => $"Well done! The agent has been exposed. ({currentProgress}/{requiredProgress})",
                AttachmentResult.AlreadyExposed => "Agent is already exposed!",
                AttachmentResult.InvalidSensor => "Invalid sensor provided.",
                _ => throw new ArgumentOutOfRangeException(nameof(result))
            };

        public static bool IsSuccess(this AttachmentResult result)
            => result == AttachmentResult.Success || result == AttachmentResult.AgentExposed;

        public static bool IsFailure(this AttachmentResult result)
            => !result.IsSuccess();
    }
}
