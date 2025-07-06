namespace sensors.src.Interfaces
{
    // Interface for sensors that can break after several uses
    public interface IBreakable
    {
        int UsageCount { get; }
        int MaxUsages { get; }
        bool IsBroken { get; }
        void BreakSensor();
        void HandleUsage();
    }
}
