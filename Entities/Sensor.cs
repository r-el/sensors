using sensors.Abstracts;

namespace sensors.Entities
{
    public class Sensor(string type) : BaseSensor(type)
    {
        public override void Activate()
        {
            IsActive = true;
            Console.WriteLine($"Activating sensor: {Type}");
        }

        public override bool Equals(object? obj)
            => obj is Sensor other && Type.Equals(other.Type);

        public override int GetHashCode()
            => Type.GetHashCode();
    }
}
