// SPDX-License-Identifier: MIT
// ASHFALL Trauma Bond System — host wiring.
//
// Binds the TraumaBondSystem authority to the canonical owners:
//   AdjustAffinity  -> SurvivorRelationsSystem.ModifyAffinity (affinity owner)
//   AreOnSameShift  -> DutyRosterSystem (shift owner)
//   GetDay          -> the canonical campaign day clock
// and composes its co-shift efficiency bonus into the duty roster's existing
// work-speed seam. No affinity, roster, or clock copy is created here.

using System;
using System.Collections.Generic;
using Ashfall.Core;
using Ashfall.Core.Survivors;

namespace AtomicWar.GodotApp
{
    public partial class Main
    {
        private TraumaBondHostSession? _traumaBond;
        private bool _traumaBondDirty;

        public TraumaBondHostSession? TraumaBond => _traumaBond;

        public void SetupTraumaBond()
        {
            if (_traumaBond != null)
            {
                WireTraumaBondHooks();
                return;
            }

            _traumaBond = new TraumaBondHostSession();
            var saved = TraumaBondSaveStore.TryLoad();
            if (saved != null) _traumaBond.RestoreState(saved);

            _traumaBond.StateChanged += () => _traumaBondDirty = true;
            WireTraumaBondHooks();
            ComposeTraumaBondWorkSpeed();
        }

        private void WireTraumaBondHooks()
        {
            if (_traumaBond == null) return;
            _traumaBond.WireHooks(
                adjustAffinity: (a, b, delta) => _survivorRelations?.System.ModifyAffinity(a, b, delta),
                areOnSameShift: AreSurvivorsOnSameShift,
                getDay: () => _campaignDay?.Calendar.CurrentDay ?? _simDay);
        }

        /// <summary>
        /// Reads the canonical shift owner. Never copies the roster: an unknown
        /// pair is simply "not on the same shift". The duty roster's general
        /// assignment entry is the shift authority; two survivors share a shift
        /// when they hold the same authored role.
        /// </summary>
        private bool AreSurvivorsOnSameShift(string a, string b)
        {
            if (string.IsNullOrEmpty(a) || string.IsNullOrEmpty(b) || a == b) return false;
            var state = _dutyRoster?.Roster.State;
            if (state?.assignments == null) return false;

            string roleA = string.Empty;
            string roleB = string.Empty;
            foreach (var entry in state.assignments)
            {
                if (entry == null || string.IsNullOrEmpty(entry.role)) continue;
                if (entry.survivorId == a) roleA = entry.role;
                else if (entry.survivorId == b) roleB = entry.role;
            }

            return !string.IsNullOrEmpty(roleA) && roleA == roleB;
        }

        /// <summary>
        /// Composes the co-shift efficiency bonus into the duty roster's existing
        /// work-speed seam. The needs-performance composition is preserved: the
        /// bonus multiplies the existing lookup result instead of replacing it.
        /// </summary>
        private void ComposeTraumaBondWorkSpeed()
        {
            var roster = _dutyRoster?.Roster;
            if (roster == null || _traumaBond == null) return;
            var previous = roster.WorkSpeedMultiplierLookup;
            roster.WorkSpeedMultiplierLookup = survivorId =>
            {
                float baseMultiplier = previous != null ? Math.Max(0.1f, previous(survivorId)) : 1.0f;
                if (string.IsNullOrEmpty(survivorId)) return baseMultiplier;
                float bonus = 0f;
                foreach (var partner in _traumaBond.BondedSurvivorIds())
                {
                    float candidate = _traumaBond.GetCoShiftEfficiencyBonus(survivorId, partner);
                    if (candidate > bonus) bonus = candidate;
                }
                return Math.Max(0.1f, baseMultiplier + bonus);
            };
        }

        /// <summary>
        /// Records a shared hazard for a group of survivors. Affinity is routed to
        /// the canonical relationship owner by the authority itself.
        /// </summary>
        public int RecordSharedHazard(IReadOnlyList<string> participantIds, string hazardId)
        {
            SetupTraumaBond();
            if (_traumaBond == null) return 0;
            int formed = _traumaBond.RecordSharedHazard(participantIds, hazardId);
            if (formed > 0) _traumaBondDirty = true;
            return formed;
        }

        /// <summary>One canonical day of bond decay over the live roster.</summary>
        public void TickTraumaBond(int day)
        {
            SetupTraumaBond();
            if (_traumaBond == null) return;
            _traumaBond.TickDay(GetRosterSurvivorIds(), 24f);
        }

        private List<string> GetRosterSurvivorIds()
        {
            var ids = new List<string>();
            if (_survivors == null) return ids;
            foreach (var survivor in _survivors.Needs.Registered)
            {
                if (survivor == null || string.IsNullOrEmpty(survivor.Id)) continue;
                ids.Add(survivor.Id);
            }
            return ids;
        }

        public void SaveTraumaBond()
        {
            if (_traumaBond == null) return;
            var state = _traumaBond.CaptureState();
            TraumaBondSaveStore.TrySave(state);
            if (CaptureSection("trauma_bond", TraumaBondSaveStore.TryCapturePersisted(state)))
                _traumaBondDirty = false;
        }

        public void FlushTraumaBondIfDirty()
        {
            if (_traumaBondDirty) SaveTraumaBond();
        }

        public void ResetTraumaBond()
        {
            _traumaBond = null;
            _traumaBondDirty = false;
        }
    }
}
