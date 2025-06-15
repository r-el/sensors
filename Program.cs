using sensors.Entities;

Sensor[] sensors = [new("Temperature-Sensor-01", "Temperature"), new("Motion-Sensor-02", "Motion")];

Console.WriteLine("Initial status:");
for (int i = 0; i < sensors.Length; i++)
    Console.WriteLine($"Sensor #{i + 1}: {sensors[i]}");

Console.WriteLine("\n--- Activating sensors ---");
sensors[0].Activate();
sensors[1].Activate();

Console.WriteLine("\nStatus after activation:");
for (int i = 0; i < sensors.Length; i++)
    Console.WriteLine($"Sensor #{i + 1}: {sensors[i]}");
