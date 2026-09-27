// SPDX-License-Identifier: MIT
// ============================================================================
// ASHFALL Ration Conflict — host wiring over the sealed
// Ashfall.Core.Survivors.RationConflictSystem.
//
// ResourceRationingSystem (behind EconomyHostSession) stays the sole allocation
// authority; NeedsSystem stays the sole morale authority;
// SurvivorRelationsSystem stays the sole pair-affinity owner. This host only
// feeds the allocations in and routes the consequences out.
// ============================================================================

using System;
using Ashfall.Core;
using Ashfall.Core.Survivors;

namespace AtomicWar.GodotApp
{
    public partial class Main
    {
        private RationConflictHostSession? _rationConflict;
        private bool _rationConflictDirty;

        public RationConflictHostSession? RationConflict => _rationConflict;

        public void SetupRationConflict()
        {
            if (_rationConflict != null) return;

            var rng = _campaignDay != null
                ? _campaignDay.Rng.Fork("ration_conflict", _core?.Clock.Day ?? 0, 0)
                : new SeededRng(31415);
            var saved = RationConflictSaveStore.TryLoad();
            _rationConflict = new RationConflictHostSession(
                new RationConflictSystem(rng),
                () => _economy?.Rationing,
                () => _survivors?.Needs,
                () => _survivorRelationsCore,
                saved);
            _rationConflict.StateChanged += () => _rationConflictDirty = true;
        }

        /// <summary>
        /// Registers every living survivor (so fairness is computed across the whole
        /// shelter) and projects the live rationing owner's per-survivor allocation
        /// multipliers in. No ration tier is recomputed here.
        /// </summary>
        public int SyncRationConflictFromRationing()
        {
            SetupRationConflict();
            if (_rationConflict == null) return 0;

            if (_survivors?.RosterState != null)
            {
                foreach (var s in _survivors.RosterState)
                {
                    if (s != null && s.IsAliveState && !string.IsNullOrEmpty(s.Id))
                        _rationConflict.RegisterSurvivor(s.Id);
                }
            }
            return _rationConflict.SyncAllocationsFromRationing();
        }

        /// <summary>Canonical day-owner body. Returns the number of survivors ticked.</summary>
        public int TickRationConflict()
        {
            if (_rationConflict == null) return 0;
            return _rationConflict.TickDay();
        }

        public string RationConflictStatusLine()
        {
            if (_rationConflict == null) return "no ration conflict state";
            int resentful = 0;
            foreach (string id in _rationConflict.RegisteredSurvivors)
            {
                var s = _rationConflict.Engine.GetState(id);
                if (s != null && s.resentmentLevel > 0f) resentful++;
            }
            return resentful == 0
                ? "rations seen as fair"
                : $"{resentful} survivor(s) resentful over rations";
        }

        public void SaveRationConflict()
        {
            if (_rationConflict == null) return;
            var state = _rationConflict.CaptureState();
            RationConflictSaveStore.TrySave(state);
            if (CaptureSection(RationConflictSaveStore.SectionName, RationConflictSaveStore.TryCapturePersisted(state)))
                _rationConflictDirty = false;
        }

        public void FlushRationConflictIfDirty()
        {
            if (_rationConflictDirty) SaveRationConflict();
        }

        public void ResetRationConflict()
        {
            _rationConflict = null;
            _rationConflictDirty = false;
        }
    }
}
