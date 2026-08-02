using AtomicWar._Game.Survivors;

namespace AtomicWar._Game.Environment
{
    /// <summary>
    /// Models ambient and perceived temperature (nuclear-winter cold), factoring
    /// shelter insulation, weather, and clothing. Feeds the Warmth need.
    /// </summary>
    public class TemperatureSystem
    {
        public float AmbientCelsius { get; private set; }

        /// <summary>Advance ambient temperature over elapsed game hours.</summary>
        public void Tick(float gameHours) => throw new System.NotImplementedException();

        /// <summary>Perceived temperature for a survivor given gear and shelter.</summary>
        public float GetPerceivedTemperature(Survivor survivor) => throw new System.NotImplementedException();
    }
}
