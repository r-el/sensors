
using sensors.Core.Enums;

namespace sensors.Core.Sensors
{
    public class Sensor(SensorType type) : BaseSensor(type)
    {
        public override void Activate() { IsActive = true; }

        public override bool Equals(object? obj)
            => obj is Sensor other && Type.Equals(other.Type);

        public override int GetHashCode() => Type.GetHashCode();
    }
}
