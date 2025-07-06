using sensors.src.Models.Agents;
using sensors.src.Models.Sensors;
using sensors.src.Types.Results;

namespace sensors.src.Services
{
    /// <summary>
    /// Manages the investigation process between agents and sensors
    /// </summary>
    public class InvestigationManager
    {
        private Agent? _targetAgent;

        public void SetTarget(Agent agent)
        {
            _targetAgent = agent;
        }

        public AttachmentResult AttachSensorToTarget(Sensor sensor)
        {
            if (_targetAgent == null)
                throw new InvalidOperationException("No target agent set for investigation");

            return _targetAgent.AttachSensor(sensor);
        }

        public int ActivateAllSensors()
        {
            if (_targetAgent == null)
                throw new InvalidOperationException("No target agent set for investigation");

            return _targetAgent.ActivateInactiveSensors();
        }

        public (int Current, int Required) GetInvestigationProgress()
        {
            if (_targetAgent == null)
                throw new InvalidOperationException("No target agent set for investigation");

            return _targetAgent.GetProgress();
        }

        public bool IsTargetExposed()
        {
            return _targetAgent?.IsExposed ?? false;
        }

        public string GetTargetStatus()
        {
            return _targetAgent?.ToString() ?? "No target agent set";
        }
    }
}
