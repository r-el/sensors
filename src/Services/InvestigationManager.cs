using sensors.src.Models.Agents;
using sensors.src.Models.Sensors;
using sensors.src.Models.Player;
using sensors.src.Types.Results;
using sensors.src.Types.Enums;
using sensors.src.Services.Factories;
using sensors.src.Interfaces;

namespace sensors.src.Services
{
    /// <summary>
    /// Central investigation manager that handles the investigation process.
    /// Contains the Iranian agent and available sensors.
    /// </summary>
    public class InvestigationManager
    {
        private Agent? _iranianAgent;
        private List<Sensor> _availableSensors;

        public InvestigationManager()
        {
            _availableSensors = new List<Sensor>();
        }

        /// <summary>
        /// Initialize investigation with agent and available sensors
        /// </summary>
        public void InitializeInvestigation(Agent iranianAgent)
        {
            _iranianAgent = iranianAgent;
            
            // Initialize available sensors for this investigation
            InitializeAvailableSensors();
        }

        /// <summary>
        /// Create and setup available sensors for the investigation
        /// </summary>
        private void InitializeAvailableSensors()
        {
            _availableSensors.Clear();
            
            // Create one of each sensor type
            var sensorTypes = new[] { 
                SensorType.Audio, SensorType.Thermal, SensorType.Pulse, 
                SensorType.Motion, SensorType.Magnetic, SensorType.Signal, SensorType.Light 
            };
            
            foreach (var type in sensorTypes)
            {
                var sensor = SensorFactory.CreateSensor(type);
                _availableSensors.Add(sensor);
            }
        }

        /// <summary>
        /// Deploy a sensor against the Iranian agent
        /// </summary>
        public AttachmentResult DeploySensor(SensorType sensorType, int currentTurn = 0)
        {
            if (_iranianAgent == null)
                throw new InvalidOperationException("No Iranian agent set for investigation");

            var sensor = GetAvailableSensor(sensorType);
            if (sensor == null)
                return new AttachmentResult(sensorType, false, AttachmentStatus.InvalidSensor);
            
            // Use the updated Activate method - returns number of matches
            int matchCount = sensor.Activate(_iranianAgent);
            
            // Convert to AttachmentResult for backward compatibility
            var (currentProgress, requiredProgress) = _iranianAgent.GetSmartProgress();
            bool isMatch = matchCount > 0;
            AttachmentStatus status = _iranianAgent.IsExposed ? AttachmentStatus.AgentExposed : AttachmentStatus.Success;
            
            return new AttachmentResult(sensorType, isMatch, status);
        }

        /// <summary>
        /// Get available sensor of specified type (if not broken)
        /// </summary>
        private Sensor? GetAvailableSensor(SensorType sensorType)
        {
            return _availableSensors.FirstOrDefault(s => 
                s.Type == sensorType && 
                (!(s is IBreakable breakable) || !breakable.IsBroken));
        }

        /// <summary>
        /// Check if investigation is complete (agent exposed)
        /// </summary>
        public bool IsInvestigationComplete()
        {
            return _iranianAgent?.IsExposed ?? false;
        }

        /// <summary>
        /// Get current investigation progress
        /// </summary>
        public (int Current, int Required) GetProgress()
        {
            if (_iranianAgent == null)
                return (0, 0);
            
            return _iranianAgent.GetSmartProgress();
        }

        /// <summary>
        /// Get the Iranian agent being investigated
        /// </summary>
        public Agent? GetIranianAgent()
        {
            return _iranianAgent;
        }

        /// <summary>
        /// Get list of available sensors with their status
        /// </summary>
        public List<(SensorType Type, bool IsAvailable, string Status)> GetSensorStatus()
        {
            var status = new List<(SensorType, bool, string)>();
            
            foreach (var sensor in _availableSensors)
            {
                bool isAvailable = !(sensor is IBreakable breakable) || !breakable.IsBroken;
                string statusText = sensor.GetStatusInfo();
                status.Add((sensor.Type, isAvailable, statusText));
            }
            
            return status;
        }
    }
}
