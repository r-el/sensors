using System;
using sensors.src.Types.Enums;
using sensors.src.Interfaces;
using sensors.src.Models.Agents;

namespace sensors.src.Models.Sensors
{
    // Pulse sensor, breaks after 3 activations
    public class PulseSensor : Sensor, IBreakable
    {
        // Track usage and breakage
        public int UsageCount { get; private set; } = 0;
        public int MaxUsages { get; } = 3;
        public bool IsBroken { get; private set; } = false;

        public PulseSensor()
        {
            Type = SensorType.Pulse;
        }

        // Activates the pulse sensor on the given agent
        public override int Activate(Agent agent)
        {
            if (IsBroken)
            {
                Console.WriteLine("Pulse sensor is broken and cannot be activated!");
                return 0; // Return 0 matches if broken
            }
            
            Console.WriteLine($"Pulse sensor activated - usage {UsageCount + 1}/{MaxUsages}");
            
            // First attach the sensor using base implementation
            int matchCount = base.Activate(agent);
            
            // Handle usage tracking
            HandleUsage();
            
            return matchCount;
        }

        public void BreakSensor()
        {
            // Mark as broken only if not already broken
            if (!IsBroken)
            {
                IsBroken = true;
                Console.WriteLine("⚠️ Pulse sensor has broken after maximum usage!");
            }
        }

        public void HandleUsage()
        {
            // Increment count and check for breakage
            UsageCount++;
            
            if (UsageCount >= MaxUsages)
            {
                BreakSensor();
            }
        }

        public override string ToString()
        {
            string status = IsBroken ? "BROKEN" : "Active";
            return $"Sensor: {Type}, Status: {status}, Usage: {UsageCount}/{MaxUsages}";
        }
    }
}
