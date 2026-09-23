// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;

namespace Ashfall.Core.Economy
{
    [Serializable]
    public sealed class SeasonalMigrationEntry
    {
        public string Phase { get; set; } = string.Empty;
        public string RegionId { get; set; } = string.Empty;
        public int PopulationDelta { get; set; }
    }

    [Serializable]
    public sealed class FactionMigrationSchedule
    {
        public string FactionId { get; set; } = string.Empty;
        public List<SeasonalMigrationEntry> Schedule { get; set; } = new();
    }

    [Serializable]
    public sealed class SeasonalMigrationCatalog
    {
        public int schema_version { get; set; } = 1;
        public int DwellDays { get; set; } = 10;
        public List<FactionMigrationSchedule> Factions { get; set; } = new();
    }

    [Serializable]
    public sealed class SeasonalMigrationSaveState
    {
        public int schema_version { get; set; } = 1;
        public Dictionary<string, int> RegionWeights { get; set; } = new();
        public string LastAppliedPhase { get; set; } = string.Empty;
        public int LastTransitionDay { get; set; }
        public HashSet<string> AppliedTransitionKeys { get; set; } = new();
    }

    /// <summary>
    /// XP-08 Plan 199 / C3-199: Seasonal Human Migration Schedule Engine.
    /// Tracks regional human population weight deltas driven by season phases.
    /// Engine-free, deterministic daily tick with dwell hysteresis (10 days).
    /// Explicitly distinct from wildlife ecosystems; never mutates wildlife state.
    /// </summary>
    public sealed class SeasonalHumanMigrationEngine
    {
        public const int DefaultBaseWeight = 100;
        public const int DefaultDwellDays = 10;

        private readonly Dictionary<string, int> _regionWeights = new(StringComparer.Ordinal);
        private readonly HashSet<string> _appliedTransitionKeys = new(StringComparer.Ordinal);
        private readonly List<FactionMigrationSchedule> _schedules = new();
        private readonly int _dwellDays;

        public int DwellDays => _dwellDays;
        public string LastAppliedPhase { get; private set; } = string.Empty;
        public int LastTransitionDay { get; private set; } = 0;
        public IReadOnlyDictionary<string, int> RegionWeights => _regionWeights;

        public SeasonalHumanMigrationEngine(
            SeasonalMigrationCatalog? catalog = null,
            IEnumerable<string>? knownRegions = null)
        {
            _dwellDays = catalog != null && catalog.DwellDays > 0 ? catalog.DwellDays : DefaultDwellDays;
            if (catalog?.Factions != null)
            {
                _schedules.AddRange(catalog.Factions);
            }

            if (knownRegions != null)
            {
                foreach (var region in knownRegions)
                {
                    if (!string.IsNullOrWhiteSpace(region) && !_regionWeights.ContainsKey(region))
                    {
                        _regionWeights[region] = DefaultBaseWeight;
                    }
                }
            }
        }

        public int GetRegionPopulationWeight(string regionId)
        {
            if (string.IsNullOrWhiteSpace(regionId)) return DefaultBaseWeight;
            return _regionWeights.TryGetValue(regionId, out int weight) ? weight : DefaultBaseWeight;
        }

        /// <summary>
        /// Daily tick called after the season phase is evaluated.
        /// Applies scheduled deltas exactly once when a phase changes and dwell requirement is satisfied.
        /// </summary>
        public bool TickDay(int currentDay, string currentSeasonPhase)
        {
            if (string.IsNullOrWhiteSpace(currentSeasonPhase))
                return false;

            if (string.Equals(currentSeasonPhase, LastAppliedPhase, StringComparison.OrdinalIgnoreCase))
                return false;

            // Enforce dwell days hysteresis
            if (LastTransitionDay > 0 && (currentDay - LastTransitionDay) < _dwellDays)
                return false;

            string transitionKey = $"{currentSeasonPhase}_day{currentDay}";
            if (_appliedTransitionKeys.Contains(transitionKey))
                return false;

            bool anyApplied = false;
            foreach (var faction in _schedules)
            {
                foreach (var entry in faction.Schedule)
                {
                    if (string.Equals(entry.Phase, currentSeasonPhase, StringComparison.OrdinalIgnoreCase))
                    {
                        int current = GetRegionPopulationWeight(entry.RegionId);
                        int next = Math.Max(10, current + entry.PopulationDelta);
                        _regionWeights[entry.RegionId] = next;
                        anyApplied = true;
                    }
                }
            }

            LastAppliedPhase = currentSeasonPhase;
            LastTransitionDay = currentDay;
            _appliedTransitionKeys.Add(transitionKey);
            return anyApplied;
        }

        public SeasonalMigrationSaveState CaptureState()
        {
            return new SeasonalMigrationSaveState
            {
                schema_version = 1,
                RegionWeights = new Dictionary<string, int>(_regionWeights),
                LastAppliedPhase = LastAppliedPhase,
                LastTransitionDay = LastTransitionDay,
                AppliedTransitionKeys = new HashSet<string>(_appliedTransitionKeys)
            };
        }

        public void RestoreState(SeasonalMigrationSaveState? state)
        {
            if (state == null) return;
            if (state.RegionWeights != null)
            {
                _regionWeights.Clear();
                foreach (var kvp in state.RegionWeights)
                    _regionWeights[kvp.Key] = kvp.Value;
            }
            LastAppliedPhase = state.LastAppliedPhase ?? string.Empty;
            LastTransitionDay = state.LastTransitionDay;
            if (state.AppliedTransitionKeys != null)
            {
                _appliedTransitionKeys.Clear();
                foreach (var k in state.AppliedTransitionKeys)
                    _appliedTransitionKeys.Add(k);
            }
        }
    }
}
