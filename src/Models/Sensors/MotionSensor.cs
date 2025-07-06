using System;
using sensors.src.Types.Enums;
using sensors.src.Interfaces;
using sensors.src.Models.Agents;
using sensors.src.Types.Results;

namespace sensors.src.Models.Sensors
{
    // Motion sensor, breaks after 3 activations
    public class MotionSensor : Sensor, IBreakable
    {
        public int UsageCount { get; private set; }
        public int MaxUsages { get; } = 3;
        public bool IsBroken => UsageCount >= MaxUsages;

        public MotionSensor()
        {
            Type = SensorType.Motion;
        }

        // Activates the motion sensor on the given agent
        public override int Activate(Agent agent)
        {
            if (IsBroken)
            {
                Console.WriteLine("Motion sensor is broken and cannot be activated!");
                return 0; // Return 0 matches if broken
            }
            
            Console.WriteLine($"Motion sensor activated - usage {UsageCount + 1}/{MaxUsages}");
            
            // First attach the sensor using base implementation
            int matchCount = base.Activate(agent);
            
            // Handle usage tracking
            HandleUsage();
            
            return matchCount;
        }

        public void HandleUsage()
        {
            UsageCount++;
            if (IsBroken) BreakSensor();
        }

        public void BreakSensor() { /* Add break logic if needed */ }

        public override string ToString()
        {
            return $"Sensor {Name} ({(IsBroken ? "Broken" : $"{3 - UsageCount} uses left")})";
        }
    }
}
