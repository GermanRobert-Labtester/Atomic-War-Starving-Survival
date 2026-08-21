using System;
using System.Collections.Generic;

namespace Ashfall.Core.Muster
{
    /// <summary>Serialized state of the Long Walk (Section V.4) — Osric Fane's
    /// all-region circuit trader (water, footwear, or a stale situation report).</summary>
    public class LongWalkState
    {
        public string systemId = LongWalkSystem.SystemId;
        public bool isActive;
        public string currentRegion = LongWalkSystem.StartRegion;
        public int daysUntilDeparture = 1;
        public int crossingsCompleted;
        public int escortCount;          // fresh-intelligence legs guarded (Approach A)
        public int resupplyCount;        // water/footwear trades (Approach B)
        public Dictionary<string, float> lastKnownFactionTrust = new Dictionary<string, float>();

        public const int CircuitLegs = 6;
        public const int RegionCycleDays = 60;
    }

    /// <summary>
    /// Engine-agnostic state machine for faction_long_walk (Section V.4). Mirrors
    /// the traveling-caravan route-advance shape: DailyTick() advances daysUntilDeparture
    /// and cycles the standing circuit. Approach A (escort) and B (resupply) are tracked
    /// so the second crossing's fork has a mechanical home. Requests return a deliberately
    /// stale snapshot — they say so unprompted.
    /// </summary>
    public class LongWalkSystem
    {
        public const string SystemId = "long_walk_system";
        public const string StartRegion = "the_grid";
        public static readonly string[] Circuit =
            { "the_grid", "the_verge", "the_spine", "the_toll", "the_drown", "the_coast" };

        private static readonly Dictionary<string, float> s_staleDefaults =
            new Dictionary<string, float>(StringComparer.Ordinal)
            {
                { "faction_cold_count", 25f }, { "faction_the_provisioned", 25f },
                { "faction_long_walk", 30f }, { "faction_scavenger_guild", 25f },
                { "faction_iron_raiders", 10f }, { "faction_hydro_barons", 15f },
                { "faction_undertow", 30f }, { "faction_the_tally", 30f }
            };

        private readonly LongWalkState _state;

        public event Action<LongWalkState> OnStateChanged;
        public event Action OnRegionChanged;

        public LongWalkSystem(LongWalkState state = null!)
        {
            _state = state ?? new LongWalkState();
            if (_state.systemId != SystemId) _state.systemId = SystemId;
            if (_state.lastKnownFactionTrust == null)
                _state.lastKnownFactionTrust = new Dictionary<string, float>(StringComparer.Ordinal);
            if (string.IsNullOrEmpty(_state.currentRegion)) _state.currentRegion = StartRegion;
        }

        public LongWalkState State => _state;
        public string CurrentRegion => _state.currentRegion;
        public int DaysUntilDeparture => _state.daysUntilDeparture;
        public int CrossingsCompleted => _state.crossingsCompleted;

        /// <summary>Route-advance. On departure the group moves a leg of the
        /// standing circuit and departs again ~RegionCycleDays later.</summary>
        public void DailyTick(int day)
        {
            _state.daysUntilDeparture--;
            if (_state.daysUntilDeparture > 0) { RaiseChanged(); return; }

            _state.crossingsCompleted++;
            int idx = Array.IndexOf(Circuit, _state.currentRegion);
            idx = (idx + 1) % Circuit.Length;
            _state.currentRegion = Circuit[idx];
            // Fresh intelligence on the leg just walked, scaled by escorts offered.
            if (_state.escortCount > 0)
                _state.lastKnownFactionTrust["faction_long_walk"] = 30f + _state.escortCount * 5f;
            _state.daysUntilDeparture = LongWalkState.RegionCycleDays;
            OnRegionChanged?.Invoke();
            RaiseChanged();
        }

        /// <summary>Approach A — guard a leg. No payment; fresher intel next pass.</summary>
        public void RecordEscort() { _state.escortCount++; RaiseChanged(); }

        /// <summary>Approach B — resupply for goods and a stale report. Sustainable,
        /// permanently at arm's length.</summary>
        public void RecordResupply() { _state.resupplyCount++; RaiseChanged(); }

        /// <summary>Returns a stale-but-reasonable sector-wide snapshot (last known).</summary>
        public Dictionary<string, float> RequestSituationReport()
        {
            var snap = new Dictionary<string, float>(StringComparer.Ordinal);
            foreach (var kv in s_staleDefaults)
            {
                float v = 50f;
                if (_state.lastKnownFactionTrust.TryGetValue(kv.Key, out float known))
                    v = known;
                else
                    v = kv.Value;
                snap[kv.Key] = v;
            }
            return snap;
        }

        public void Activate() { _state.isActive = true; RaiseChanged(); }

        // ── Save / Load ────────────────────────────────────────────────

        public LongWalkState CaptureState()
        {
            var copy = new LongWalkState
            {
                systemId = _state.systemId,
                isActive = _state.isActive,
                currentRegion = _state.currentRegion,
                daysUntilDeparture = _state.daysUntilDeparture,
                crossingsCompleted = _state.crossingsCompleted,
                escortCount = _state.escortCount,
                resupplyCount = _state.resupplyCount
            };
            var keys = new List<string>(_state.lastKnownFactionTrust.Keys);
            keys.Sort(StringComparer.Ordinal);
            foreach (var k in keys)
                copy.lastKnownFactionTrust[k] = _state.lastKnownFactionTrust[k];
            return copy;
        }

        public void RestoreState(LongWalkState saved)
        {
            if (saved == null) return;
            _state.systemId = SystemId;
            _state.isActive = saved.isActive;
            _state.currentRegion = string.IsNullOrEmpty(saved.currentRegion)
                ? StartRegion : saved.currentRegion;
            _state.daysUntilDeparture = saved.daysUntilDeparture;
            _state.crossingsCompleted = Math.Max(0, saved.crossingsCompleted);
            _state.escortCount = Math.Max(0, saved.escortCount);
            _state.resupplyCount = Math.Max(0, saved.resupplyCount);
            _state.lastKnownFactionTrust.Clear();
            if (saved.lastKnownFactionTrust != null)
                foreach (var kv in saved.lastKnownFactionTrust)
                    _state.lastKnownFactionTrust[kv.Key] = kv.Value;
            RaiseChanged();
        }

        private void RaiseChanged() => OnStateChanged?.Invoke(_state);
    }
}
