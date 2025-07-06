using sensors.src.Types.Enums;
using sensors.src.Models.Sensors;
using sensors.src.Types.Results;
using sensors.src.Services;

namespace sensors.src.Models.Agents
{
    public abstract class Agent(AgentRank rank, int sensorSlots, List<SensorType>? predefinedWeaknesses = null)
    {
        public AgentRank Rank { get; protected set; } = rank;
        public bool IsExposed { get; protected set; }
        public string Affiliation { get; protected set; } = "Iranian";

        protected List<SensorType> SecretWeaknesses { get; set; } = ValidateAndSetWeaknesses(predefinedWeaknesses, sensorSlots);
        
        // SensorSlots is derived from the actual weaknesses count
        public int SensorSlots => SecretWeaknesses.Count;
        
        private static List<SensorType> ValidateAndSetWeaknesses(List<SensorType>? predefinedWeaknesses, int requiredCount)
        {
            if (predefinedWeaknesses == null)
                return RandomizationService.GenerateRandomWeaknesses(requiredCount);
                
            if (predefinedWeaknesses.Count != requiredCount)
                throw new ArgumentException($"Predefined weaknesses count ({predefinedWeaknesses.Count}) must match required sensor count ({requiredCount})");
                
            return [.. predefinedWeaknesses];
        }
        protected List<Sensor> AttachedSensors { get; set; } = [];
        
        public IReadOnlyList<Sensor> GetAttachedSensors() => AttachedSensors.AsReadOnly();

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
            
            return (matchedCount, SensorSlots);
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

        /// <summary>
        /// Attaches a sensor to the agent with turn tracking for improved counterattack management
        /// </summary>
        public virtual AttachmentResult AttachSensor(Sensor sensor, int currentTurn = 0)
        {
            // Use provided turn count instead of internal counter for better game flow control
            if (IsExposed)
                return new AttachmentResult(sensor.Type, false, AttachmentStatus.AlreadyExposed);
                
            AttachedSensors.Add(sensor);
            
            // Check if sensor matches the agent's weaknesses
            bool isMatch = SecretWeaknesses.Contains(sensor.Type);
            MatchResult matchResult = isMatch ? MatchResult.Match : MatchResult.NoMatch;
            
            // Note: Counterattack is now handled in GameManager before sensor attachment
            // This prevents double counterattacks
            
            var (currentProgress, requiredProgress) = GetSmartProgress();
            if (currentProgress >= requiredProgress)
            {
                IsExposed = true;
                AttachmentResult result = new(sensor.Type, isMatch, AttachmentStatus.AgentExposed)
                {
                    MatchResult = matchResult
                };
                return result;
            }
            
            AttachmentResult successResult = new AttachmentResult(sensor.Type, isMatch, AttachmentStatus.Success);
            successResult.MatchResult = matchResult;
            return successResult;
        }

        /// <summary>
        /// Legacy method for backward compatibility - uses default turn of 0
        /// </summary>
        public virtual AttachmentResult AttachSensor(Sensor sensor)
            => AttachSensor(sensor, 0);

        public override string ToString()
        {
            string agentType = GetType().Name;
            var (correctCount, requiredCount) = GetSmartProgress();
            string progress = $"{correctCount}/{requiredCount}";
            string status = IsExposed ? "- EXPOSED!" : "";
            string attachedSensors = $"[{string.Join(", ", 
                AttachedSensors
                    .GroupBy(s => s.Type)
                    .Select(g => g.Count() > 1 ? $"{g.Key}*{g.Count()}" : $"{g.Key}"))}]";
            return $"{agentType} [{Rank}] - Progress: {progress} {status}\nAttached Sensors: {attachedSensors}";
        }
    }
}
