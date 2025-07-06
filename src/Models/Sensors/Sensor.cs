using sensors.src.Models.Agents;
using sensors.src.Types.Enums;

namespace sensors.src.Models.Sensors
{
    // Base class for all sensors
    public abstract class Sensor
    {
        public SensorType Type { get; protected set; }
        public string Name { get; protected set; }

        protected Sensor()
        {
            // Each derived sensor will set its own name
            Name = GenerateRandomName();
        }

        // Basic activation method, can be extended by derived classes
        // Activates the basic sensor on an agent (no special effect)
        public virtual bool Activate(Agent agent)
        {
            return true; // Basic sensor always succeeds
        }

        /// <summary>
        /// Generates a random name for the sensor
        /// </summary>
        private string GenerateRandomName()
        {
            string[] prefixes = { "Alpha", "Beta", "Gamma", "Delta", "Echo", "Foxtrot", "Guardian", "Hunter", "Intel", "Phantom" };
            string[] suffixes = { "Mark I", "Mark II", "Mark III", "Pro", "Elite", "Advanced", "X1", "X2", "Prime", "Ultra" };
            
            var random = new System.Random();
            string prefix = prefixes[random.Next(prefixes.Length)];
            string suffix = suffixes[random.Next(suffixes.Length)];
            
            return $"{prefix} {suffix}";
        }

        public override string ToString()
        {
            return $"{Name} ({Type})";
        }
    }
}
