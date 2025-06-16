using sensors.Enums;

namespace sensors.Abstracts
{
    public abstract class BaseSensor(SensorType type)
    {
        public SensorType Type { get; protected set; } = type;
        public bool IsActive { get; protected set; } = false;

        public abstract void Activate();

        public override string ToString() 
            => $"Sensor: {Type}, Status: {(IsActive ? "Active" : "Inactive")}";
    }
}
