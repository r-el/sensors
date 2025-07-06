# 🔍 Agent Investigation System

A professional console-based investigation game where you play as an intelligence agent tracking Iranian operatives using advanced sensor technology.

## 🎮 Game Overview

Deploy various sensors to expose enemy agents while avoiding their counterattacks. Each agent rank requires different strategies and sensor combinations.

## 🚀 Quick Start

```bash
dotnet run
```

## 🎯 Features

- **Professional UI**: Clean, modern interface using Spectre.Console
- **Strategic Gameplay**: 7 different sensor types with unique abilities
- **Multiple Agent Types**: From Foot Soldiers to Organization Leaders
- **Counterattack System**: Agents fight back with increasing sophistication
- **Progress Tracking**: Player statistics and performance metrics

## 🛠️ Technology

- **.NET 8.0**: Modern C# development
- **Spectre.Console**: Beautiful terminal UI
- **Professional Architecture**: Clean code with interfaces and design patterns

## 📁 Project Structure

```
src/
├── UI/
│   └── SpectreUI.cs           # Professional user interface
├── Services/
│   ├── GameManager.cs         # Main game logic
│   ├── PlayerManager.cs       # Player management
│   └── Factories/             # Object creation
├── Models/
│   ├── Agents/               # Enemy agent types
│   ├── Sensors/              # Sensor implementations
│   └── Player/               # Player data
├── Types/
│   ├── Enums/               # Type definitions
│   └── Results/             # Result structures
└── Interfaces/              # System contracts
```

## 🎲 Gameplay

1. **Choose Target**: Select enemy agent rank
2. **Deploy Sensors**: Use 7 different sensor types strategically
3. **Avoid Counterattacks**: Higher ranks fight back more aggressively
4. **Expose Agent**: Gather enough intelligence to neutralize target

### 📡 Sensor Types

- **🔊 Audio**: Basic detection sensor
- **🌡️ Thermal**: Reveals correct sensor types
- **⚡ Pulse**: Limited uses (3 maximum)
- **🏃 Motion**: Breakable after 3 uses
- **🛡️ Magnetic**: Blocks counterattacks (2 uses)
- **📡 Signal**: Reveals agent information
- **💡 Light**: Advanced intelligence gathering

### 🎖️ Enemy Ranks

- **👤 Foot Soldier**: 3 sensors, no counterattacks
- **👥 Squad Leader**: 4 sensors, counterattacks every 3 turns
- **🎖️ Senior Commander**: 6 sensors, powerful counterattacks
- **⭐ Organization Leader**: 8 sensors, devastating abilities

Built with professional game development practices and clean architecture.
