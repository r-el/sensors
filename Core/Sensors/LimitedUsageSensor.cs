using System;
using sensors.Core.Enums;

namespace sensors.Core.Sensors
{
    /// <summary>
    /// A specialized sensor with limited activations.
    /// Demonstrates Open/Closed Principle - extends behavior without modifying base classes.
    /// </summary>
    public class LimitedUsageSensor : BaseSensor
    {
        private readonly int _maxUsages;
        private int _remainingUsages;

        public LimitedUsageSensor(SensorType type) : base(type)
        {
            _maxUsages = type.GetMaxUsages();
            _remainingUsages = _maxUsages;
        }

        public int RemainingUsages => _remainingUsages;
        public int MaxUsages => _maxUsages;
        public bool IsBroken => _remainingUsages <= 0;

        public override void Activate()
        {
            if (IsBroken)
            {
                Console.WriteLine($"{Type} sensor is broken (no uses remaining).");
                return;
            }

            _remainingUsages--;
            IsActive = true;

            if (IsBroken)
                Console.WriteLine($"{Type} sensor activated and broke! (0/{_maxUsages} uses remaining)");
            else
                Console.WriteLine($"{Type} sensor activated. ({_remainingUsages}/{_maxUsages} uses remaining)");
        }

        public override string ToString()
        {
            string status = IsBroken ? "Broken" : IsActive ? "Active" : "Inactive";
            return $"Sensor: {Type}, Status: {status}, Uses: {_remainingUsages}/{_maxUsages}";
        }
    }
}
