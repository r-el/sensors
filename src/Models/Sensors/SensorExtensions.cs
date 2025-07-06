using System;
using sensors.src.Models.Agents;
using sensors.src.Models.Sensors;
using sensors.src.Types.Enums;
using sensors.src.Interfaces;
using sensors.src.Types.Results;

namespace sensors.src.Models.Sensors
{
    // Extension methods for sensor activation and management
    public static class SensorExtensions
    {
        /// <summary>
        /// Extension method to activate a sensor on an agent with turn tracking
        /// </summary>
        public static AttachmentResult ActivateOn(this Sensor sensor, Agent agent, int currentTurn = 0)
        {
            // Check if sensor is broken
            if (sensor is IBreakable breakable && breakable.IsBroken)
            {
                Console.WriteLine($"❌ Sensor {sensor.Type} is broken and cannot be used");
                return new AttachmentResult(sensor.Type, false, AttachmentStatus.InvalidSensor);
            }
            
            // Attach sensor to agent with turn information
            AttachmentResult attachResult = agent.AttachSensor(sensor, currentTurn);
            
            // Activate sensor (HandleUsage is called inside each sensor's Activate method)
            sensor.Activate(agent);
            
            // Apply special effects
            string specialMessage = ApplySpecialEffect(sensor, agent);
            
            // Update result
            attachResult.SpecialEffectMessage = specialMessage;
            
            return attachResult;
        }

        /// <summary>
        /// Legacy method for backward compatibility
        /// </summary>
        public static AttachmentResult ActivateOn(this Sensor sensor, Agent agent)
        {
            return ActivateOn(sensor, agent, 0);
        }
        
        /// <summary>
        /// Applies special effects based on sensor type
        /// </summary>
        private static string ApplySpecialEffect(Sensor sensor, Agent agent)
        {
            // Handle reveal sensors
            if (sensor is IReveal revealSensor)
            {
                SensorType? revealed = revealSensor.RevealInformation(agent);
                return revealed?.ToString() ?? "No weakness detected";
            }
            
            // Sensor-specific effects
            switch (sensor.Type)
            {
                case SensorType.Magnetic:
                    // Prevents counterattacks
                    if (agent is ICounterattack counterAgent)
                    {
                        counterAgent.DisableNextCounterattack();
                        return "Magnetic field disrupts agent's counterattack capabilities";
                    }
                    return "";
                    
                case SensorType.Signal:
                    // Reveals agent rank
                    return $"Signal intercept reveals: Agent rank is {agent.Rank}";
                    
                case SensorType.Light:
                    // Reveals two pieces of info
                    return $"Light analysis reveals: Agent rank is {agent.Rank}, Affiliation: {agent.Affiliation}";
                    
                default:
                    return "";
            }
        }
        
        /// <summary>
        /// Returns sensor status information
        /// STATUS TRACKING: Provides clear feedback about sensor condition
        /// </summary>
        public static string GetStatusInfo(this Sensor sensor)
        {
            // BREAKABLE SENSOR STATUS: Show usage and condition for breakable sensors
            if (sensor is IBreakable breakable)
            {
                if (breakable.IsBroken)
                    return "Broken - Cannot be used";
                else
                    // USAGE TRACKING: Show remaining uses for player planning
                    return $"Working - {3 - breakable.UsageCount} uses left";
            }
            
            // STANDARD SENSORS: Always available unless broken
            return "Working";
        }
    }
}