# Sensors Investigation Game

A C# investigation game where players use various sensor types to uncover secret information about Iranian agents.


## Sensor Types

The game supports 7 different sensor types:

1. **Audio Sensor** - Basic sensor with no special abilities
2. **Thermal Sensor** - Reveals one correct sensor type from the agent's secret list
3. **Pulse Sensor** - Breaks after 3 activations (implements IBreakable)
4. **Motion Sensor** - Can activate 3 times total, then breaks
5. **Magnetic Sensor** - Cancels agent counterattack twice when matched correctly
6. **Signal Sensor** - Reveals one field of information about the agent
7. **Light Sensor** - Reveals 2 fields of information about the agent

## Key Features

- **Clean Architecture**: Following SOLID principles with clear separation of concerns
- **Extension Methods**: Sensor logic centralized in `SensorExtensions.cs`
- **Interface-Based Design**: `IBreakable`, `IReveal`, `ICounterattack`, `ICounterattackBlocker`
- **Class Hierarchy**: `CounterattackAgent` for agents with counterattack capabilities
- **Factory Pattern**: `SensorFactory` for creating sensor instances
- **No Code Duplication**: Common functionality extracted to base classes and extensions

## Game Flow

1. Player selects an agent rank to investigate (FootSoldier or SquadLeader)
2. Player selects sensors to attach (different ranks require different sensor counts)
3. Player attempts to match sensors with the agent's secret list
4. Special sensor effects are triggered based on matches
5. Game continues until mission success or failure

## Building and Running

```bash
dotnet build
dotnet run
```

## Dependencies

- .NET 8.0
- No external packages required
