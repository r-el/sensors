using sensors.src.Models.Agents;
using sensors.src.Types.Enums;
using sensors.src.Types.Results;
using sensors.src.Interfaces;

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

        /// <summary>
        /// Activates the sensor on an agent: attaches the sensor and returns the number of matching sensors
        /// </summary>
        /// <param name="agent">The agent to attach this sensor to</param>
        /// <returns>Number of sensors that match the agent's weaknesses after attachment</returns>
        public virtual int Activate(Agent agent)
        {
            // Check if sensor is broken
            if (this is IBreakable breakable && breakable.IsBroken)
            {
                Console.WriteLine($"❌ Sensor {Type} is broken and cannot be used");
                return 0; // Return 0 matches if broken
            }
            
            // Attach this sensor to the agent
            AttachmentResult result = agent.AttachSensor(this);
            
            // Apply special effects based on sensor type
            ApplySpecialEffects(agent);
            
            // Get current progress (number of matching sensors)
            var (currentMatches, requiredMatches) = agent.GetSmartProgress();
            
            return currentMatches;
        }

        /// <summary>
        /// Applies special effects based on sensor type and interfaces
        /// </summary>
        protected virtual void ApplySpecialEffects(Agent agent)
        {
            // Handle reveal sensors
            if (this is IReveal revealSensor)
            {
                SensorType? revealed = revealSensor.RevealInformation(agent);
                if (revealed.HasValue)
                {
                    Console.WriteLine($"🔍 {Type} sensor reveals weakness: {revealed.Value}");
                }
            }
            
            // Sensor-specific effects
            switch (Type)
            {
                case SensorType.Magnetic:
                    // Prevents counterattacks
                    if (agent is ICounterattack counterAgent)
                    {
                        counterAgent.DisableNextCounterattack();
                        Console.WriteLine("🧲 Magnetic field disrupts agent's counterattack capabilities");
                    }
                    break;
                    
                case SensorType.Signal:
                    // Reveals agent rank
                    Console.WriteLine($"📡 Signal intercept reveals: Agent rank is {agent.Rank}");
                    break;
                    
                case SensorType.Light:
                    // Reveals two pieces of info
                    Console.WriteLine($"💡 Light analysis reveals: Agent rank is {agent.Rank}, Affiliation: {agent.Affiliation}");
                    break;
            }
        }

        /// <summary>
        /// Returns sensor status information
        /// STATUS TRACKING: Provides clear feedback about sensor condition
        /// </summary>
        public virtual string GetStatusInfo()
        {
            // BREAKABLE SENSOR STATUS: Show usage and condition for breakable sensors
            if (this is IBreakable breakable)
            {
                if (breakable.IsBroken)
                    return "Broken - Cannot be used";
                else
                    // USAGE TRACKING: Show remaining uses for player planning
                    return $"Working - {breakable.MaxUsages - breakable.UsageCount} uses left";
            }
            
            // STANDARD SENSORS: Always available unless broken
            return "Working";
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
            return $"Sensor: {Type} ({Name})";
        }
    }
}