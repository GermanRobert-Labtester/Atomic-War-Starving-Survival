using UnityEngine;

namespace AtomicWar._Game.Environment
{
    /// <summary>
    /// Spatial model of fallout intensity across the world grid. Systems sample
    /// it to determine the dose rate at a location; storms and zones mutate it.
    /// </summary>
    public class FalloutMap
    {
        /// <summary>Sample the dose rate (rads/hour) at a world position.</summary>
        public float SampleRadsPerHour(Vector2 worldPosition) => throw new System.NotImplementedException();

        /// <summary>Deposit a circular fallout zone of a given intensity.</summary>
        public void AddFalloutZone(Vector2 center, float radius, float intensity) => throw new System.NotImplementedException();

        /// <summary>Decay all deposited fallout over elapsed game hours.</summary>
        public void DecayAll(float hours) => throw new System.NotImplementedException();
    }
}
