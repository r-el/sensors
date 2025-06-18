using sensors.Core.Sensors;
using sensors.Core.Enums;

namespace sensors.Services.Factories
{
    /// <summary>
    /// Factory class for creating different types of sensors.
    /// Follows Factory Pattern and Open/Closed Principle.
    /// </summary>
    public static class SensorFactory
    {
        /// <summary>
        /// Creates the appropriate sensor type based on the sensor's characteristics.
        /// Limited usage sensors (Pulse, Motion) get special behavior.
        /// </summary>
        /// <param name="sensorType">The type of sensor to create</param>
        /// <returns>A new sensor instance with appropriate behavior</returns>
        public static BaseSensor CreateSensor(SensorType sensorType)
        {
            return sensorType switch
            {
                // Sensors with limited usage get special implementation
                SensorType.Pulse or SensorType.Motion => new LimitedUsageSensor(sensorType),
                
                // Standard sensors get basic implementation
                _ => new Sensor(sensorType)
            };
        }

        /// <summary>
        /// Creates a basic unlimited-usage sensor.
        /// </summary>
        /// <param name="sensorType">The type of sensor to create</param>
        /// <returns>A new basic sensor instance</returns>
        public static Sensor CreateBasicSensor(SensorType sensorType)
        {
            return new Sensor(sensorType);
        }

        /// <summary>
        /// Creates a limited-usage sensor with the specified usage count.
        /// </summary>
        /// <param name="sensorType">The type of sensor to create</param>
        /// <returns>A new limited usage sensor instance</returns>
        public static LimitedUsageSensor CreateLimitedSensor(SensorType sensorType)
        {
            if (sensorType.GetMaxUsages() == int.MaxValue)
                throw new ArgumentException($"Sensor type {sensorType} does not have usage limitations.");
                
            return new LimitedUsageSensor(sensorType);
        }
    }
}
