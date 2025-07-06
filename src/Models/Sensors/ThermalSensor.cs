using System;
using sensors.src.Types.Enums;
using sensors.src.Models.Agents;
using sensors.src.Interfaces;

namespace sensors.src.Models.Sensors
{
    // Thermal sensor, reveals one correct weakness from the agent's secret list
    public class ThermalSensor : Sensor, IReveal
    {
        public ThermalSensor()
        {
            Type = SensorType.Thermal;
        }

        // Activates the thermal sensor on the given agent and reveals a weakness
        public override bool Activate(Agent agent)
        {
            SensorType? revealedWeakness = RevealInformation(agent);
            Console.WriteLine($"Thermal sensor activated, revealed: {revealedWeakness}");
            return true; // ThermalSensor always succeeds
        }

        public SensorType? RevealInformation(Agent agent)
        {
            SensorType weakness = agent.RevealOneWeakness();
            return weakness != SensorType.None ? weakness : (SensorType?)null;
        }
    }
}
