using System;
using System.Collections.Generic;

namespace AtomicWar._Game.World
{
    /// <summary>
    /// Location Evolution System (#55) — expedition map locations change
    /// over time based on player actions and faction movements. Cleared
    /// ruins may become raider outposts, faction outposts, or barren
    /// ghost sectors.
    ///
    /// Plain C#, save-safe.
    /// </summary>
    public class LocationEvolutionSystem
    {
        public const int EvolutionCheckIntervalDays = 10;
        public const float RaiderCaptureChance = 0.20f;
        public const float FactionSettlementChance = 0.15f;
        public const float AbandonmentChance = 0.25f;
        public const float ReinforceChance = 0.10f;

        [Serializable]
        public class LocationState
        {
            public string LocationId;
            public string CurrentOwner; // "none", "raiders", "garrison", "militia", "cult"
            public int DaysSinceLastVisit;
            public int DaysSinceCleared;
            public bool IsCleared;
            public bool IsBarren;
            public int EvolutionDay;
        }

        public event Action<string, string, string> OnLocationEvolved;
        // locationId, oldOwner, newOwner
        public event Action<string> OnLocationAbandoned;
        public event Action<string, string> OnLocationCaptured;
        // locationId, captorFaction

        private readonly Dictionary<string, LocationState> _locations =
            new Dictionary<string, LocationState>();
        private int _lastEvolutionDay = -1;

        public void RegisterLocation(string locationId, string initialOwner = "none")
        {
            if (!_locations.ContainsKey(locationId))
            {
                _locations[locationId] = new LocationState
                {
                    LocationId = locationId,
                    CurrentOwner = initialOwner,
                    DaysSinceLastVisit = 0,
                    DaysSinceCleared = 0,
                    IsCleared = false
                };
            }
        }

        public void MarkLocationVisited(string locationId)
        {
            if (_locations.TryGetValue(locationId, out var state))
                state.DaysSinceLastVisit = 0;
        }

        public void MarkLocationCleared(string locationId)
        {
            if (_locations.TryGetValue(locationId, out var state))
            {
                state.IsCleared = true;
                state.DaysSinceCleared = 0;
                state.CurrentOwner = "none";
            }
        }

        public void Tick(int currentDay, System.Random rng)
        {
            if (currentDay - _lastEvolutionDay < EvolutionCheckIntervalDays)
                return;
            _lastEvolutionDay = currentDay;

            foreach (var kv in _locations)
            {
                var state = kv.Value;
                state.DaysSinceLastVisit++;
                state.DaysSinceCleared++;

                if (state.IsCleared && state.DaysSinceCleared > 20)
                {
                    float roll = (float)(rng?.NextDouble() ?? 0.5);
                    string oldOwner = state.CurrentOwner;

                    if (roll < RaiderCaptureChance)
                    {
                        state.CurrentOwner = "raiders";
                        state.IsCleared = false;
                        OnLocationCaptured?.Invoke(state.LocationId, "raiders");
                    }
                    else if (roll < RaiderCaptureChance + FactionSettlementChance)
                    {
                        string[] factions = { "garrison", "militia", "cult" };
                        state.CurrentOwner = factions[rng?.Next(factions.Length) ?? 0];
                        state.IsCleared = false;
                    }
                    else if (roll < RaiderCaptureChance + FactionSettlementChance +
                        AbandonmentChance)
                    {
                        state.IsBarren = true;
                        OnLocationAbandoned?.Invoke(state.LocationId);
                    }

                    if (oldOwner != state.CurrentOwner)
                        OnLocationEvolved?.Invoke(state.LocationId, oldOwner,
                            state.CurrentOwner);
                }
            }
        }

        public LocationState GetLocationState(string locationId)
        {
            return _locations.TryGetValue(locationId, out var state) ? state : null;
        }
    }
}
