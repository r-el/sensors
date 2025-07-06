using System;
using sensors.src.Interfaces;
using sensors.src.Models.Agents;
using sensors.src.Types.Enums;
using sensors.src.Types.Results;

namespace sensors.src.Models.Sensors
{
    /// <summary>
    /// Magnetic sensor that can block counterattacks up to 2 times
    /// </summary>
    public class MagneticSensor : Sensor, ICounterattackBlocker
    {
        private int _blockCount = 0;
        private const int _maxBlocks = 2;

        /// <summary>
        /// Public access to the maximum number of blocks allowed
        /// </summary>
        public int MaxBlocks => _maxBlocks;
        
        /// <summary>
        /// Public access to the current block count
        /// </summary>
        public int BlockCount => _blockCount;

        public MagneticSensor()
        {
            Type = SensorType.Magnetic;
        }

        /// <summary>
        /// Blocks the counterattack ability of the given counterattacker
        /// Can only be used a maximum of 2 times
        /// </summary>
        public void BlockCounterattack(ICounterattack counterattacker)
        {
            if (_blockCount < _maxBlocks)
            {
                counterattacker.DisableNextCounterattack();
                _blockCount++;
                Console.WriteLine($"🛡️ Magnetic sensor blocked the counterattack! ({_blockCount}/{_maxBlocks} uses)");
            }
            else
            {
                Console.WriteLine("❌ Magnetic sensor has already blocked maximum number of counterattacks (2).");
            }
        }

        /// <summary>
        /// Check if the sensor can still block counterattacks
        /// </summary>
        public bool IsBlockingCounterattack()
        {
            return _blockCount < _maxBlocks;
        }

        /// <summary>
        /// MagneticSensor uses base implementation - special effects are handled automatically
        /// </summary>
        /// String representation showing remaining blocks
        /// </summary>
        public override string ToString()
        {
            return $"{base.ToString()} | Blocks remaining: {_maxBlocks - _blockCount}";
        }
    }
}
