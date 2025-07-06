using System;
using System.Collections.Generic;
using System.Linq;
using sensors.src.Types.Enums;
using sensors.src.Models.Sensors;
using sensors.src.Types.Results;
using sensors.src.Services;
using sensors.src.Interfaces;

namespace sensors.src.Models.Agents
{
    public abstract class Agent
    {
        public AgentRank Rank { get; protected set; }
        public bool IsExposed { get; protected set; }
        public int RequiredSensorCount { get; protected set; }
        public string Affiliation { get; protected set; } = "Iranian";

        protected List<SensorType> SecretWeaknesses { get; set; } = [];
        protected List<Sensor> AttachedSensors { get; set; } = [];
        
        private Dictionary<SensorType, int> _matchedSensorsByType = new Dictionary<SensorType, int>();
        
        protected int _turnCount = 0;
        
        public IReadOnlyList<Sensor> GetAttachedSensors() => AttachedSensors.AsReadOnly();

        protected Agent(AgentRank rank, int requiredSensorCount)
        {
            Rank = rank;
            RequiredSensorCount = requiredSensorCount;
            InitializeWeaknesses();
        }

        protected abstract void InitializeWeaknesses();

        /// <summary>
        /// Resets the internal matching state - used for special abilities
        /// </summary>
        protected void ResetMatchingState()
        {
            _matchedSensorsByType.Clear();
            IsExposed = false;
        }
        
        public virtual MatchResult TryMatchSensor(SensorType sensorType)
        {
            int totalWeaknessCount = SecretWeaknesses.Count(w => w == sensorType);
            if (totalWeaknessCount == 0)
                return MatchResult.NoMatch;
            int currentMatches = _matchedSensorsByType.GetValueOrDefault(sensorType, 0);
            if (currentMatches >= totalWeaknessCount)
                return MatchResult.AlreadyMatched;
            _matchedSensorsByType[sensorType] = currentMatches + 1;
            return MatchResult.Match;
        }

        public virtual (int CurrentProgress, int RequiredProgress) GetSmartProgress()
        {
            // Calculate progress based on currently attached sensors that match weaknesses
            // This ensures progress can go down when sensors are removed by counterattacks
            int matchedCount = 0;
            
            // Count how many currently attached sensors match the agent's weaknesses
            Dictionary<SensorType, int> weaknessCount = new Dictionary<SensorType, int>();
            foreach (SensorType weakness in SecretWeaknesses)
            {
                weaknessCount[weakness] = weaknessCount.GetValueOrDefault(weakness, 0) + 1;
            }
            
            // Count matches among currently attached sensors
            foreach (Sensor attachedSensor in AttachedSensors)
            {
                if (weaknessCount.TryGetValue(attachedSensor.Type, out int count) && count > 0)
                {
                    weaknessCount[attachedSensor.Type] = count - 1;
                    matchedCount++;
                }
            }
            
            return (matchedCount, RequiredSensorCount);
        }

        public virtual SensorType RevealOneWeakness()
        {
            if (SecretWeaknesses != null && SecretWeaknesses.Count > 0)
            {
                int index = RandomizationService.GetRandomSensorTypeIndex(SecretWeaknesses);
                if (index >= 0 && index < SecretWeaknesses.Count)
                    return SecretWeaknesses[index];
            }
            return SensorType.None;
        }

        public virtual int ActivateInactiveSensors()
        {
            if (AttachedSensors.Count == 0)
            {
                Console.WriteLine("No sensors attached.");
                return GetMatches().MatchCount;
            }
            IEnumerable<string> activationResults = AttachedSensors
                .Select(sensor => { sensor.Activate(this); return sensor; })
                .GroupBy(s => s.Type)
                .Select(g => g.Count() > 1 ? $"{g.Key}×{g.Count()}" : g.Key.ToString());
            Console.WriteLine($"Activating remaining sensors on {Rank} agent...");
            Console.WriteLine($"Activated: {string.Join(", ", activationResults)}");
            return GetMatches().MatchCount;
        }

        protected virtual (int MatchCount, List<Sensor> MatchedSensors) GetMatches()
        {
            Dictionary<SensorType, int> weaknessCount = [];
            List<Sensor> matchedSensors = [];
            foreach (SensorType weakness in SecretWeaknesses)
                weaknessCount[weakness] = weaknessCount.GetValueOrDefault(weakness, 0) + 1;
            foreach (Sensor attachedSensor in AttachedSensors)
                if (weaknessCount.TryGetValue(attachedSensor.Type, out int count) && count > 0)
                {
                    weaknessCount[attachedSensor.Type] = count - 1;
                    matchedSensors.Add(attachedSensor);
                }
            return (matchedSensors.Count, matchedSensors);
        }

        public virtual (int CurrentProgress, int RequiredProgress) GetProgress()
        {
            return GetSmartProgress();
        }

        /// <summary>
        /// Attaches a sensor to the agent with turn tracking for improved counterattack management
        /// </summary>
        public virtual AttachmentResult AttachSensor(Sensor sensor, int currentTurn = 0)
        {
            // Use provided turn count instead of internal counter for better game flow control
            if (IsExposed)
                return new AttachmentResult(sensor.Type, false, AttachmentStatus.AlreadyExposed);
                
            AttachedSensors.Add(sensor);
            MatchResult matchResult = TryMatchSensor(sensor.Type);
            bool isMatch = matchResult == MatchResult.Match;
            
            // Note: Counterattack is now handled in GameManager before sensor attachment
            // This prevents double counterattacks
            
            var (currentProgress, requiredProgress) = GetSmartProgress();
            if (currentProgress >= requiredProgress)
            {
                IsExposed = true;
                var result = new AttachmentResult(sensor.Type, isMatch, AttachmentStatus.AgentExposed);
                result.MatchResult = matchResult;
                return result;
            }
            
            var successResult = new AttachmentResult(sensor.Type, isMatch, AttachmentStatus.Success);
            successResult.MatchResult = matchResult;
            return successResult;
        }

        /// <summary>
        /// Legacy method for backward compatibility - increments internal turn counter
        /// </summary>
        public virtual AttachmentResult AttachSensor(Sensor sensor)
        {
            _turnCount++;
            return AttachSensor(sensor, _turnCount);
        }

        public override string ToString()
        {
            string agentType = GetType().Name;
            int correctCount = GetMatches().MatchCount;
            string progress = $"{correctCount}/{RequiredSensorCount}";
            string status = IsExposed ? "- EXPOSED!" : "";
            string attachedSensors = $"[{string.Join(", ", 
                AttachedSensors
                    .GroupBy(s => s.Type)
                    .Select(g => g.Count() > 1 ? $"{g.Key}*{g.Count()}" : $"{g.Key}"))}]";
            return $"{agentType} [{Rank}] - Progress: {progress} {status}\nAttached Sensors: {attachedSensors}";
        }
    }
}
