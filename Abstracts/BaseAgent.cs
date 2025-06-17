using sensors.Entities;
using sensors.Enums;

namespace sensors.Abstracts
{
    public abstract class BaseAgent(AgentRank rank)
    {
        public AgentRank Rank { get; set; } = rank;
        public bool IsExposed { get; protected set; }

        protected List<Sensor> SecretWeaknesses { get; set; } = [];
        protected List<Sensor> AttachedSensors { get; set; } = [];

        public virtual int ActivateInactiveSensors()
        {
            List<Sensor> inactiveSensors = AttachedSensors.Where(s => !s.IsActive).ToList();
            
            if (inactiveSensors.Count == 0)
            {
                Console.WriteLine("All attached sensors are already active.");
                return GetMatches().MatchCount;
            }

            // IEnumerable for Lazy evaluation
            IEnumerable<string> activationResults = inactiveSensors
                .Select(sensor => { sensor.Activate(); return sensor; })
                .GroupBy(s => s.Type)
                .Select(g => g.Count() > 1 ? $"{g.Key}×{g.Count()}" : g.Key.ToString());
            
            Console.WriteLine($"Activated: {string.Join(", ", activationResults)}");
            
            return GetMatches().MatchCount;
        }

        protected virtual (int MatchCount, List<Sensor> MatchedSensors) GetMatches()
        {
            Dictionary<SensorType, int> weaknessCount = [];
            List<Sensor> matchedSensors = [];

            foreach (Sensor weakness in SecretWeaknesses)
                weaknessCount[weakness.Type] = weaknessCount.GetValueOrDefault(weakness.Type, 0) + 1;

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
            int currentProgress = GetMatches().MatchCount;
            int requiredProgress = Rank.RequiredSensors();
            return (currentProgress, requiredProgress);
        }

        public virtual AttachmentResult AttachSensor(Sensor sensor)
        {
            Console.WriteLine($"\nAttaching {sensor.Type} sensor to {GetType().Name} [{Rank}]...");

            if (IsExposed)
                return AttachmentResult.AlreadyExposed;

            AttachedSensors.Add(sensor);

            int correctCount = GetMatches().MatchCount;
            int requiredCount = Rank.RequiredSensors();

            if (correctCount >= requiredCount)
            {
                IsExposed = true;
                return AttachmentResult.AgentExposed;
            }

            return AttachmentResult.Success;
        }

        public override string ToString()
        {
            string agentType = GetType().Name;
            int correctCount = GetMatches().MatchCount;
            string progress = $"{correctCount}/{Rank.RequiredSensors()}";
            string status = IsExposed ? "- EXPOSED!" : "";
            string attachedSensors = $"[{string.Join(", ", 
                AttachedSensors
                    .GroupBy(s => s.Type)
                    .Select(g => g.Count() > 1 ? $"{g.Key}*{g.Count()}" : $"{g.Key}"))}]";

            return $"{agentType} [{Rank}] - Progress: {progress} {status}\nAttached Sensors: {attachedSensors}";
        }
    }
}
