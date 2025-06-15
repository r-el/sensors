namespace sensors.Abstracts
{
    public abstract class BaseSensor(string name, string type)
    {
        public string Name { get; protected set; } = name ?? throw new ArgumentNullException(nameof(name));
        public string Type { get; protected set; } = type ?? throw new ArgumentNullException(nameof(type));
        public bool IsActive { get; protected set; } = false;

        public abstract void Activate();

        public override string ToString() 
            => $"Sensor: {Name}, Type: {Type}, Status: {(IsActive ? "Active" : "Inactive")}";
    }
}
