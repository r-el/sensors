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

        public SensorType? RevealInformation(Agent agent)
        {
            SensorType weakness = agent.RevealOneWeakness();
            return weakness != SensorType.None ? weakness : (SensorType?)null;
        }
    }
}
