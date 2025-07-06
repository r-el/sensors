using System;
using System.Collections.Generic;
using System.Linq;
using sensors.src.Types.Enums;
using sensors.src.Models.Sensors;

namespace sensors.src.Services
{
    /// <summary>
    /// Static service responsible for all randomization logic in the game.
    /// Centralizes random number generation for consistency and simplicity.
    /// </summary>
    public static class RandomizationService
    {
        private static readonly Random _random = new Random();

        /// <summary>
        /// Generates random weakness array for agents
        /// </summary>
        public static List<SensorType> GenerateRandomWeaknesses(int count)
        {
            var weaknesses = new List<SensorType>();
            // Get all valid sensor types (exclude None)
            SensorType[] availableTypes = [.. Enum.GetValues<SensorType>().Where(t => t != SensorType.None)];

            // Add random sensors to weakness list
            for (int i = 0; i < count; i++)
            {
                SensorType randomType = GetRandomSensorType(availableTypes);
                weaknesses.Add(randomType);
            }

            return weaknesses;
        }

        /// <summary>
        /// Random sensor selection from array
        /// </summary>
        public static SensorType GetRandomSensorType(SensorType[] availableTypes)
        {
            if (availableTypes == null || availableTypes.Length == 0)
                throw new ArgumentException("Available types array cannot be null or empty");

            int randomIndex = _random.Next(availableTypes.Length);
            return availableTypes[randomIndex];
        }

        /// <summary>
        /// Gets a random sensor type from all available types
        /// </summary>
        /// <returns>Random sensor type</returns>
        public static SensorType GetRandomSensorType()
        {
            SensorType[] allTypes = Enum.GetValues<SensorType>();
            return GetRandomSensorType(allTypes);
        }

        /// <summary>
        /// Random sensor for counterattack removal
        /// </summary>
        public static int GetRandomSensorIndex(List<Sensor> sensors)
        {
            if (sensors == null || sensors.Count == 0)
                return -1;

            return _random.Next(sensors.Count);
        }

        /// <summary>
        /// Selects a random sensor type from a list
        /// </summary>
        /// <param name="sensorTypes">List of sensor types to choose from</param>
        /// <returns>Index of randomly selected sensor type, or -1 if list is empty</returns>
        public static int GetRandomSensorTypeIndex(List<SensorType> sensorTypes)
        {
            if (sensorTypes == null || sensorTypes.Count == 0)
                return -1;

            return _random.Next(sensorTypes.Count);
        }

        /// <summary>
        /// Generates a random boolean with specified probability
        /// </summary>
        /// <param name="probability">Probability of true (0.0 to 1.0)</param>
        /// <returns>Random boolean</returns>
        public static bool GetRandomBoolean(double probability = 0.5)
        {
            if (probability < 0.0 || probability > 1.0)
                throw new ArgumentOutOfRangeException(nameof(probability), "Probability must be between 0.0 and 1.0");

            return _random.NextDouble() < probability;
        }

        /// <summary>
        /// Generates a random integer within specified range
        /// </summary>
        /// <param name="minValue">Minimum value (inclusive)</param>
        /// <param name="maxValue">Maximum value (exclusive)</param>
        /// <returns>Random integer</returns>
        public static int GetRandomInt(int minValue, int maxValue)
        {
            return _random.Next(minValue, maxValue);
        }

        /// <summary>
        /// Shuffle list using Fisher-Yates
        /// </summary>
        public static void Shuffle<T>(List<T> list)
        {
            if (list == null || list.Count <= 1)
                return;

            // Swap elements randomly
            for (int i = list.Count - 1; i > 0; i--)
            {
                int randomIndex = _random.Next(i + 1);
                (list[i], list[randomIndex]) = (list[randomIndex], list[i]);
            }
        }

        /// <summary>
        /// Smart weakness generation with variety control
        /// HANDLES DUPLICATES: Can create [Thermal, Thermal, Motion] patterns
        /// </summary>
        public static List<SensorType> GenerateBalancedWeaknesses(int count, bool ensureVariety = true)
        {
            var weaknesses = new List<SensorType>();
            SensorType[] availableTypes = Enum.GetValues<SensorType>()
                .Where(t => t != SensorType.None)
                .ToArray();

            // Use random if no variety needed or small count
            if (!ensureVariety || count <= availableTypes.Length)
            {
                return GenerateRandomWeaknesses(count);
            }

            // Add one of each type first for variety
            var shuffledTypes = new List<SensorType>(availableTypes);
            Shuffle(shuffledTypes);

            foreach (SensorType type in shuffledTypes)
            {
                if (weaknesses.Count < count)
                {
                    weaknesses.Add(type);
                }
            }

            // Fill remaining with random (creates duplicates)
            while (weaknesses.Count < count)
            {
                SensorType randomType = GetRandomSensorType();
                weaknesses.Add(randomType);
            }

            // Shuffle final result
            Shuffle(weaknesses);
            return weaknesses;
        }

        /// <summary>
        /// Removes random items from a list and returns the removed items
        /// </summary>
        /// <typeparam name="T">Type of items in the list</typeparam>
        /// <param name="list">The list to remove items from</param>
        /// <param name="count">Number of items to remove</param>
        /// <returns>List of removed items</returns>
        public static List<T> RemoveRandomItems<T>(List<T> list, int count)
        {
            if (list == null || list.Count == 0 || count <= 0)
                return new List<T>();

            count = Math.Min(count, list.Count);
            List<T> removedItems = new List<T>();
            
            for (int i = 0; i < count; i++)
            {
                int index = _random.Next(list.Count);
                T item = list[index];
                removedItems.Add(item);
                list.RemoveAt(index);
            }
            
            return removedItems;
        }
    }
}
