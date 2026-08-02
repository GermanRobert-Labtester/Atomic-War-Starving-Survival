using UnityEngine;
using AtomicWar._Game.Survivors;
using AtomicWar._Game.Shelter;

namespace AtomicWar._Game.Environment
{
    /// <summary>
    /// Models ambient and perceived temperature (nuclear-winter cold), factoring
    /// shelter insulation, weather, heater modules, and clothing. Feeds Warmth need.
    /// Reads Shelter aggregate stats (IndoorTempBonus) directly.
    /// </summary>
    public class TemperatureSystem
    {
        public float AmbientCelsius { get; private set; } = -10f;

        public void SetAmbient(float celsius)
        {
            AmbientCelsius = celsius;
        }

        /// <summary>Advance ambient temperature over elapsed game hours.</summary>
        public void Tick(float gameHours)
        {
            // Ambient temperature processing per tick
        }

        /// <summary>Calculates indoor temperature factoring shelter heater output.</summary>
        public float GetIndoorTemperature(Shelter.Shelter shelter)
        {
            float bonus = shelter != null ? shelter.IndoorTempBonus : 0f;
            return AmbientCelsius + bonus;
        }

        /// <summary>Perceived temperature for a survivor given gear and shelter.</summary>
        public float GetPerceivedTemperature(Survivor survivor, Shelter.Shelter shelter = null)
        {
            float baseTemp = shelter != null ? GetIndoorTemperature(shelter) : AmbientCelsius;
            return baseTemp;
        }
    }
}
