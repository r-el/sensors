using System;
using sensors.src.Types.Enums;
using sensors.src.Models.Agents;

namespace sensors.src.Models.Sensors
{
    // Basic audio sensor, no special abilities
    public class AudioSensor : Sensor
    {
        public AudioSensor()
        {
            Type = SensorType.Audio;
        }

        // No special effect for AudioSensor
        public override bool Activate(Agent agent)
        {
            return true; // AudioSensor always succeeds
        }
    }
}
