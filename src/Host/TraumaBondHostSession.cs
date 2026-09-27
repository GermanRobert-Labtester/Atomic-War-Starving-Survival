// SPDX-License-Identifier: MIT
// ============================================================================
// Host Session : TraumaBondHostSession
// Purpose      : Trauma bond authority — survivors who endure extreme hazards
//                together form deep bonds. The host only routes the authority's
//                three hooks into the canonical owners (affinity, shift, clock)
//                and persists its own bounded state. No affinity, roster, or
//                clock copy is created here.
// ============================================================================

using System;
using System.Collections.Generic;
using Ashfall.Core.Survivors;

namespace AtomicWar.GodotApp
{
    /// <summary>Immutable projection of the trauma-bond surface for day reports and UI.</summary>
    public sealed class TraumaBondProjection
    {
        public int BondedSurvivorCount;
        public int TotalBonds;
        public int StrongBondCount;
        public float LastCoShiftBonus;
        public string LastEvent = string.Empty;
    }

    public sealed class TraumaBondHostSession : HostSessionBase
    {
        private readonly TraumaBondSystem _system = new();
        private string _lastEvent = string.Empty;
        private float _lastCoShiftBonus;

        public TraumaBondSystem System => _system;
        public string LastEvent => _lastEvent;

        public TraumaBondHostSession() { }

        /// <summary>
        /// Routes the authority's hooks into the canonical owners. The host keeps
        /// no affinity, shift, or day state of its own.
        /// </summary>
        public void WireHooks(
            Action<string, string, float> adjustAffinity,
            Func<string, string, bool> areOnSameShift,
            Func<float> getDay)
        {
            _system.AdjustAffinity = adjustAffinity;
            _system.AreOnSameShift = areOnSameShift;
            _system.GetDay = getDay;
        }

        /// <summary>Records one shared hazard endured by two or more survivors.</summary>
        public int RecordSharedHazard(IReadOnlyList<string> participantIds, string hazardId)
        {
            if (participantIds == null || participantIds.Count < 2) return 0;
            int before = GetTotalBondCount();
            _system.OnSharedHazardEndured(new List<string>(participantIds), hazardId);
            int after = GetTotalBondCount();
            _lastEvent = $"Shared hazard '{hazardId}' endured by {participantIds.Count} survivor(s); bonds {before} -> {after}.";
            RaiseStateChanged();
            return after - before;
        }

        /// <summary>
        /// Co-shift efficiency bonus for a pair. The value is gated by the
        /// authority's own bond-strength floor and by the canonical shift owner.
        /// </summary>
        public float GetCoShiftEfficiencyBonus(string survivorA, string survivorB)
        {
            if (string.IsNullOrEmpty(survivorA) || string.IsNullOrEmpty(survivorB)) return 0f;
            if (survivorA == survivorB) return 0f;
            if (_system.AreOnSameShift != null && !_system.AreOnSameShift(survivorA, survivorB)) return 0f;

            float bonus = _system.GetCoShiftEfficiencyBonus(survivorA, survivorB);
            _lastCoShiftBonus = bonus;
            return bonus;
        }

        /// <summary>Daily bond decay over the canonical day length.</summary>
        public void TickDay(IEnumerable<string> survivorIds, float gameHours = 24f)
        {
            if (survivorIds == null) return;
            foreach (string id in survivorIds)
                _system.Tick(id, gameHours);
            RaiseStateChanged();
        }

        public int GetTotalBondCount()
        {
            int total = 0;
            foreach (var id in BondedSurvivorIds()) total += _system.GetBondCount(id);
            return total;
        }

        public IEnumerable<string> BondedSurvivorIds()
        {
            var ids = new List<string>();
            foreach (var survivor in _system.CaptureState().Survivors)
            {
                if (survivor == null || string.IsNullOrEmpty(survivor.SurvivorId)) continue;
                if (_system.GetBondCount(survivor.SurvivorId) > 0) ids.Add(survivor.SurvivorId);
            }
            return ids;
        }

        public int GetStrongBondCount()
        {
            int strong = 0;
            var state = _system.CaptureState();
            foreach (var survivor in state.Survivors)
            {
                if (survivor?.Bonds == null) continue;
                foreach (var bond in survivor.Bonds)
                    if (bond != null && bond.BondStrength >= TraumaBondSystem.MinBondStrengthForBonus) strong++;
            }
            return strong;
        }

        public TraumaBondProjection GetProjection() => new TraumaBondProjection
        {
            BondedSurvivorCount = new List<string>(BondedSurvivorIds()).Count,
            TotalBonds = GetTotalBondCount(),
            StrongBondCount = GetStrongBondCount(),
            LastCoShiftBonus = _lastCoShiftBonus,
            LastEvent = _lastEvent
        };

        // ── Save / Load ──────────────────────────────────────────────────

        public TraumaBondSaveState CaptureState() => _system.CaptureState();

        public void RestoreState(TraumaBondSaveState? state)
        {
            _system.RestoreState(state);
            _lastEvent = "Trauma bond state restored.";
            RaiseStateChanged();
        }

        public void Reset()
        {
            _system.RestoreState(null);
            _lastEvent = string.Empty;
            _lastCoShiftBonus = 0f;
            RaiseStateChanged();
        }
    }
}
