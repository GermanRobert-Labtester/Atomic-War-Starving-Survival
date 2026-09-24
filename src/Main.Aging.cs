// SPDX-License-Identifier: MIT
// ============================================================================
// ASHFALL Plan 176 — Aging & Elderly Survivor System Host Wiring.
// Core AgingSystem & SurvivorAgingProgressionEngine are the authorities for
// chronological age progression, life stages, retirement, elder mentorship,
// and age milestones.
// ============================================================================

using System;
using System.Collections.Generic;
using Godot;
using Ashfall.Core;
using Ashfall.Core.Survivors;

namespace AtomicWar.GodotApp
{
    public partial class Main
    {
        private AgingHostSession? _aging;
        private bool _agingDirty;

        public AgingHostSession? Aging => _aging;

        public void SetupAging()
        {
            if (_aging != null) return;

            _aging = AgingHostSession.Create(_dataDir);

            var saved = AgingSaveStore.TryLoad();
            if (saved != null)
            {
                _aging.RestoreState(saved);
            }

            _aging.StateChanged += () => _agingDirty = true;

            if (_survivorDetailPanel != null)
            {
                _survivorDetailPanel.AgeProfileProvider = id =>
                {
                    int day = _campaignDay?.Calendar.CurrentDay ?? _simDay;
                    return _aging.EvaluateSurvivor(id, day);
                };
            }
        }

        public void SaveAging()
        {
            if (_aging == null) return;
            var state = _aging.CaptureState();
            AgingSaveStore.TrySave(state);
            if (CaptureSection("aging", AgingSaveStore.TryCapturePersisted(state)))
            {
                _agingDirty = false;
            }
        }

        public void TickAging(int day)
        {
            if (_aging == null) SetupAging();
            if (_aging == null) return;

            // Ensure all active roster survivors are registered in AgingSystem
            var livingIds = new List<string>();
            if (_survivors?.RosterState != null)
            {
                for (int i = 0; i < _survivors.RosterState.Count; i++)
                {
                    var s = _survivors.RosterState[i];
                    if (s == null || string.IsNullOrEmpty(s.Id)) continue;

                    if (s.IsAlive)
                    {
                        livingIds.Add(s.Id);
                    }

                    // Register if not already tracked
                    var rosterEntry = _survivors.Roster?.Find(s.Id);
                    int joinedDay = rosterEntry?.joinedDay ?? 1;
                    _aging.RegisterSurvivor(s.Id, SurvivorAgingProgressionEngine.DefaultRecruitmentAgeYears, joinedDay);
                }
            }

            _aging.AdvanceDay(day, livingIds);
        }

        public SurvivorAgeProfile? EvaluateSurvivorAgeProfile(string survivorId)
        {
            if (_aging == null) SetupAging();
            int day = _campaignDay?.Calendar.CurrentDay ?? _simDay;
            return _aging?.EvaluateSurvivor(survivorId, day);
        }

        public bool RetireSurvivor(string survivorId)
        {
            if (_aging == null) SetupAging();
            if (_aging == null) return false;
            int day = _campaignDay?.Calendar.CurrentDay ?? _simDay;
            return _aging.RetireSurvivor(survivorId, day);
        }

        public bool IsSurvivorRetired(string survivorId)
        {
            if (_aging == null) SetupAging();
            return _aging?.IsRetired(survivorId) ?? false;
        }

        public bool HasLivingElderMentor()
        {
            if (_aging == null) SetupAging();
            if (_aging == null) return false;

            int day = _campaignDay?.Calendar.CurrentDay ?? _simDay;
            var livingIds = new List<string>();
            if (_survivors?.RosterState != null)
            {
                for (int i = 0; i < _survivors.RosterState.Count; i++)
                {
                    var s = _survivors.RosterState[i];
                    if (s != null && s.IsAlive && !string.IsNullOrEmpty(s.Id))
                    {
                        livingIds.Add(s.Id);
                    }
                }
            }

            return _aging.HasLivingElderMentor(day, livingIds);
        }

        public AgingCensus GetAgingCensus() =>
            _aging?.Census ?? default;

        public void FlushAgingIfDirty()
        {
            if (_agingDirty)
            {
                SaveAging();
            }
        }

        public void ResetAging()
        {
            _aging = null;
            _agingDirty = false;
        }
    }
}
