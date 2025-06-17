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

        public virtual int Activate()
        {
            Console.WriteLine("Activating all attached sensors...");

            // Activate all attached sensors
            foreach (Sensor s in AttachedSensors)
                s.Activate();

            // Count and return matching sensors
            int matchingCount = CountCorrectSensors();
            int totalSensors = AttachedSensors.Count;

            Console.WriteLine($"Sensors activated: {totalSensors}, Matching weaknesses: {matchingCount}");

            return matchingCount;
        }

        public virtual string AttachSensor(Sensor sensor)
        {
            Console.WriteLine($"\nAttaching {sensor.Type} sensor to {GetType().Name} [{Rank}]...");
            
            if (IsExposed)
                return "Agent is already exposed!";

            AttachedSensors.Add(sensor);

            int correctCount = CountCorrectSensors();
            int requiredCount = Rank.RequiredSensors();

            if (correctCount >= requiredCount)
            {
                IsExposed = true;
                return $"Well done! The agent has been exposed. ({correctCount}/{requiredCount})";
            }

            return $"Sensor attached. Progress: {correctCount}/{requiredCount}";
        }

        protected virtual int CountCorrectSensors()
        {
            List<Sensor> remainingWeaknesses = [.. SecretWeaknesses];
            int matches = 0;

            foreach (Sensor attachedSensor in AttachedSensors)
            {
                Sensor? matchingWeakness = remainingWeaknesses.FirstOrDefault(w => w.Equals(attachedSensor));
                if (matchingWeakness != null)
                {
                    remainingWeaknesses.Remove(matchingWeakness);
                    matches++;
                }
            }

            return matches;
        }

        public override string ToString()
        {
            string agentType = GetType().Name;
            string progress = $"{CountCorrectSensors()}/{Rank.RequiredSensors()} sensors";
            string status = IsExposed ? "- EXPOSED!" : "";
            string attachedSensors = AttachedSensors.Count > 0 
                ? $"\nAttached Sensors:\n{string.Join("\n", AttachedSensors.Select(s => $"  • {s.Type}"))}"
                : "No Attached Sensors";
            
            return $"{agentType} [{Rank}]: {progress} {status} {attachedSensors} ";
        }
    }
}
