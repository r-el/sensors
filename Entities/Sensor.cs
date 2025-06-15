using sensors.Abstracts;

namespace sensors.Entities
{
    public class Sensor(string name, string type) : BaseSensor(name, type)
    {
        public override void Activate()
        {
            IsActive = true;
            Console.WriteLine($"Activating sensor: {Name} (Type: {Type})");
        }
    }
}
