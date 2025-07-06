using System;

namespace sensors.src.Types.Enums
{
    // Enum for all sensor types
    public enum SensorType
    {
        None = 0,
        Audio = 1,
        Thermal = 2,
        Pulse = 3,
        Motion = 4,
        Magnetic = 5,
        Signal = 6,
        Light = 7
    }

    /// <summary>
    /// Extension methods for SensorType enum
    /// </summary>
    public static class SensorTypeExtensions
    {
        /// <summary>
        /// Gets the emoji representation for each sensor type
        /// </summary>
        public static string GetEmoji(this SensorType sensorType)
        {
            return sensorType switch
            {
                SensorType.Audio => "🔊",
                SensorType.Thermal => "🌡️",
                SensorType.Pulse => "⚡",
                SensorType.Motion => "🏃",
                SensorType.Magnetic => "🛡️",
                SensorType.Signal => "📡",
                SensorType.Light => "💡",
                _ => ""
            };
        }

        public static string GetDescription(this SensorType sensorType)
            => sensorType switch
            {
                SensorType.None => "No sensor selected",
                SensorType.Audio => "Basic sensor. No special ability.",
                SensorType.Thermal => "Reveals one correct sensor type from the secret list.",
                SensorType.Pulse => "Breaks after 3 activations.",
                SensorType.Motion => "Can activate 3 times total, then breaks.",
                SensorType.Magnetic => "Cancels terrorist counterattack twice if matched correctly.",
                SensorType.Signal => "Reveals one field of information about the terrorist (e.g., rank).",
                SensorType.Light => "Reveals 2 fields of information about the terrorist (e.g., affiliation).",
                _ => throw new ArgumentOutOfRangeException(nameof(sensorType))
            };

        public static bool HasSpecialAbility(this SensorType sensorType) => sensorType != SensorType.Audio && sensorType != SensorType.None;

        public static int GetMaxUsages(this SensorType sensorType)
            => sensorType switch
            {
                SensorType.None => 0,
                SensorType.Pulse => 3,
                SensorType.Motion => 3,
                _ => int.MaxValue // Unlimited usage
            };
    }
}
