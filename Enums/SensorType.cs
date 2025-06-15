namespace sensors.Enums
{
    public enum SensorType
    {
        Audio,
        Thermal,
        Pulse,
        Motion,
        Magnetic,
        Signal,
        Light
    }

    public static class SensorTypeExtensions
    {
        public static string GetDescription(this SensorType sensorType)
            => sensorType switch
            {
                SensorType.Audio => "Basic sensor. No special ability.",
                SensorType.Thermal => "Reveals one correct sensor type from the secret list.",
                SensorType.Pulse => "Breaks after 3 activations.",
                SensorType.Motion => "Can activate 3 times total, then breaks.",
                SensorType.Magnetic => "Cancels terrorist counterattack twice if matched correctly.",
                SensorType.Signal => "Reveals one field of information about the terrorist (e.g., rank).",
                SensorType.Light => "Reveals 2 fields of information about the terrorist (e.g., affiliation).",
                _ => throw new ArgumentOutOfRangeException(nameof(sensorType))
            };

        public static bool HasSpecialAbility(this SensorType sensorType) => sensorType != SensorType.Audio;

        public static int GetMaxUsages(this SensorType sensorType)
            => sensorType switch
            {
                SensorType.Pulse => 3,
                SensorType.Motion => 3,
                _ => int.MaxValue // Unlimited usage
            };
    }
}
