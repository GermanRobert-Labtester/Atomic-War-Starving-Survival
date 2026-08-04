using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Environment
{
    /// <summary>
    /// Spatial model of fallout intensity across the world grid. Systems sample
    /// it to determine the dose rate at a location; storms and zones mutate it.
    /// </summary>
    public class FalloutMap
    {
        private readonly List<FalloutZone> _zones = new List<FalloutZone>();

        /// <summary>Sample the dose rate (rads/hour) at a world position.
        /// Returns the highest overlapping zone intensity, or 0 if none.</summary>
        public float SampleRadsPerHour(Vector2 worldPosition)
        {
            float maxRads = 0f;
            for (int i = 0; i < _zones.Count; i++)
            {
                var zone = _zones[i];
                float dist = Vector2.Distance(worldPosition, zone.Center);
                if (dist <= zone.Radius)
                {
                    float t = 1f - (dist / Mathf.Max(0.01f, zone.Radius));
                    maxRads = Mathf.Max(maxRads, zone.Intensity * t);
                }
            }
            return maxRads;
        }

        /// <summary>Deposit a circular fallout zone of a given intensity.</summary>
        public void AddFalloutZone(Vector2 center, float radius, float intensity)
        {
            if (radius <= 0f || intensity <= 0f) return;
            _zones.Add(new FalloutZone { Center = center, Radius = radius, Intensity = intensity });
        }

        /// <summary>Decay all deposited fallout over elapsed game hours (48h half-life).</summary>
        public void DecayAll(float hours)
        {
            if (hours <= 0f) return;
            for (int i = _zones.Count - 1; i >= 0; i--)
            {
                var z = _zones[i];
                z.Intensity *= Mathf.Pow(0.5f, hours / 48f);
                _zones[i] = z;
                if (z.Intensity < 0.5f)
                    _zones.RemoveAt(i);
            }
        }

        private struct FalloutZone
        {
            public Vector2 Center;
            public float Radius;
            public float Intensity;
        }
    }
}
