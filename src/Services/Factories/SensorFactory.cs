using sensors.src.Models.Sensors;
using sensors.src.Types.Enums;

namespace sensors.src.Services.Factories
{
    // Factory for creating sensors by type
    public static class SensorFactory
    {
        public static Sensor CreateSensor(SensorType type)
        {
            switch (type)
            {
                case SensorType.Audio:
                    return new AudioSensor();
                case SensorType.Thermal:
                    return new ThermalSensor();
                case SensorType.Pulse:
                    return new PulseSensor();
                case SensorType.Motion:
                    return new MotionSensor();
                case SensorType.Magnetic:
                    return new MagneticSensor();
                case SensorType.Signal:
                    return new SignalSensor();
                case SensorType.Light:
                    return new LightSensor();
                default:
                    return new AudioSensor(); // Default to AudioSensor
            }
        }
    }
}
