using sensors.src.Types.Enums;

namespace sensors.src.Models.Sensors
{
    // Basic audio sensor, no special abilities
    public class AudioSensor : Sensor
    {
        public AudioSensor()
        {
            Type = SensorType.Audio;
        }
    }
}
