namespace sensors.Abstracts
{
    public abstract class BaseSensor(string type)
    {
        public string Type { get; protected set; } = type ?? throw new ArgumentNullException(nameof(type));
        public bool IsActive { get; protected set; } = false;

        public abstract void Activate();

        public override string ToString() 
            => $"Sensor: {Type}, Status: {(IsActive ? "Active" : "Inactive")}";
    }
}
