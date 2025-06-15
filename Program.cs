using sensors.Entities;
using sensors.Enums;

Console.WriteLine("=== Agent Detection Game ===");
Console.WriteLine("Your mission: Find the agent's weaknesses!");
Console.WriteLine();

IranianAgent agent = new(AgentRank.FootSoldier);

Console.WriteLine($"Target: {agent}");
Console.WriteLine($"You need to find {agent.Rank.RequiredSensors()} correct sensors to expose the agent.");
Console.WriteLine();

SensorType[] availableTypes = [.. Enum.GetValues<SensorType>()];
Console.WriteLine("Available sensor types:");

foreach ((SensorType type, int idx) in availableTypes.Select((type, i) => (type, i + 1)))
    Console.WriteLine($"{idx}. {type}");
Console.WriteLine();

while (!agent.IsExposed)
{
    Console.Write($"Choose a sensor type (1-{availableTypes.Length}): ");
    string input = Console.ReadLine() ?? "";

    Console.WriteLine(
        int.TryParse(input, out int choice) && choice >= 1 && choice <= availableTypes.Length ?
        ProcessSensorChoice(agent, availableTypes[choice - 1]) :
        "Invalid choice. Please try again."
    );

    if (agent.IsExposed)
    {
        Console.WriteLine("\nCongratulations! You've exposed the agent!");
        Console.WriteLine("Final activation:");
        Console.WriteLine($"Total matching sensors: {agent.Activate()}");
        break;
    }

    Console.WriteLine();
}

Console.WriteLine("\nGame Over! Press any key to exit...");
Console.ReadKey();

// Local function with file-scoped namespace and enhanced features
static string ProcessSensorChoice(IranianAgent agent, SensorType selectedType)
{
    Sensor newSensor = new(selectedType.ToString());

    Console.WriteLine($"\nAttaching {selectedType} sensor...");
    return agent.AttachSensor(newSensor);
}
