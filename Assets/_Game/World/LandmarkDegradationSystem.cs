using System;
using System.Collections.Generic;

namespace AtomicWar._Game.World
{
    /// <summary>
    /// Landmark Degradation System (#58) — returning to previously
    /// scavenged locations reveals progressive collapse: flooded
    /// basements, caved-in roofs, and altered search routes.
    ///
    /// Plain C#, save-safe.
    /// </summary>
    public class LandmarkDegradationSystem
    {
        public const int DegradationCheckDays = 7;
        public const float CollapseChancePerVisit = 0.10f;
        public const float FloodChanceInRain = 0.25f;
        public const int MinDaysBeforeDegradation = 5;

        [Serializable]
        public class LandmarkState
        {
            public string LocationId;
            public int VisitCount;
            public int DaysSinceLastVisit;
            public bool IsCollapsed;
            public bool IsFlooded;
            public bool RoutesBlocked;
            public float LootQualityMultiplier; // degrades over time
        }

        public event Action<string, string> OnLandmarkDegraded;
        // locationId, degradationType (collapsed/flooded/blocked)
        public event Action<string> OnRoutesChanged;

        private readonly Dictionary<string, LandmarkState> _landmarks =
            new Dictionary<string, LandmarkState>();

        public void RegisterLandmark(string locationId)
        {
            if (!_landmarks.ContainsKey(locationId))
            {
                _landmarks[locationId] = new LandmarkState
                {
                    LocationId = locationId,
                    LootQualityMultiplier = 1f
                };
            }
        }

        public void MarkVisited(string locationId)
        {
            if (_landmarks.TryGetValue(locationId, out var state))
            {
                state.VisitCount++;
                state.DaysSinceLastVisit = 0;

                // Diminishing returns on loot
                state.LootQualityMultiplier = Math.Max(0.3f,
                    1f - state.VisitCount * 0.15f);
            }
        }

        public LandmarkState GetState(string locationId)
        {
            return _landmarks.TryGetValue(locationId, out var state) ? state : null;
        }

        public void Tick(int currentDay, bool isRaining, System.Random rng)
        {
            foreach (var kv in _landmarks)
            {
                var state = kv.Value;
                state.DaysSinceLastVisit++;

                if (state.DaysSinceLastVisit >= MinDaysBeforeDegradation &&
                    state.DaysSinceLastVisit % DegradationCheckDays == 0)
                {
                    float roll = (float)(rng?.NextDouble() ?? 0.5);

                    if (!state.IsCollapsed &&
                        roll < CollapseChancePerVisit * state.VisitCount)
                    {
                        state.IsCollapsed = true;
                        state.RoutesBlocked = true;
                        OnLandmarkDegraded?.Invoke(state.LocationId, "collapsed");
                        OnRoutesChanged?.Invoke(state.LocationId);
                    }

                    if (!state.IsFlooded && isRaining &&
                        roll < FloodChanceInRain)
                    {
                        state.IsFlooded = true;
                        OnLandmarkDegraded?.Invoke(state.LocationId, "flooded");
                    }
                }
            }
        }
    }
}
