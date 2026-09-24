// SPDX-License-Identifier: MIT
// ============================================================================
// ASHFALL Plan 159 — Shelter Governance & Political System Host Wiring.
// Core ShelterGovernanceEngine is the authority for ideological blocs,
// policy consent, grievance tracking, civil disputes, and stability rating.
// Integrates with LeadershipSystem and Survivor Roster.
// ============================================================================

using System;
using System.Collections.Generic;
using Godot;
using Ashfall.Core;
using Ashfall.Core.Governance;

namespace AtomicWar.GodotApp
{
    public partial class Main
    {
        private ShelterGovernanceHostSession? _shelterGovernance;
        private bool _shelterGovernanceDirty;

        public ShelterGovernanceHostSession? ShelterGovernance => _shelterGovernance;

        public void SetupShelterGovernance()
        {
            if (_shelterGovernance != null) return;

            _shelterGovernance = ShelterGovernanceHostSession.Create(
                _dataDir,
                policySystem: null,
                leadershipSystem: _survivorSocial?.Leadership);

            var saved = ShelterGovernanceSaveStore.TryLoad();
            if (saved != null)
            {
                _shelterGovernance.RestoreState(saved);
            }

            _shelterGovernance.StateChanged += () => _shelterGovernanceDirty = true;

            if (_survivorDetailPanel != null)
            {
                _survivorDetailPanel.PoliticalBlocProvider = id => _shelterGovernance.GetSurvivorBlocDisplayName(id);
            }
        }

        public void SaveShelterGovernance()
        {
            if (_shelterGovernance == null) return;
            var state = _shelterGovernance.CaptureState();
            ShelterGovernanceSaveStore.TrySave(state);
            if (CaptureSection("shelter_governance", ShelterGovernanceSaveStore.TryCapturePersisted(state)))
            {
                _shelterGovernanceDirty = false;
            }
        }

        public void TickShelterGovernance(int day)
        {
            if (_shelterGovernance == null) SetupShelterGovernance();
            if (_shelterGovernance == null) return;

            // Deterministically assign any active survivors who have not yet joined a political bloc
            if (_survivors?.RosterState != null && _survivors.RosterState.Count > 0)
            {
                var defKeys = new List<string>(_shelterGovernance.Definitions.Keys);
                if (defKeys.Count > 0)
                {
                    defKeys.Sort(StringComparer.Ordinal);
                    for (int i = 0; i < _survivors.RosterState.Count; i++)
                    {
                        var s = _survivors.RosterState[i];
                        if (s == null || string.IsNullOrEmpty(s.Id)) continue;
                        if (_shelterGovernance.GetSurvivorBloc(s.Id) == null)
                        {
                            uint hash = (uint)Math.Abs(s.Id.GetHashCode());
                            string chosenBloc = defKeys[(int)(hash % (uint)defKeys.Count)];
                            _shelterGovernance.AssignSurvivorToBloc(s.Id, chosenBloc);
                        }
                    }
                }
            }

            _shelterGovernance.AdvanceDay(day);
        }

        public bool AssignSurvivorToPoliticalBloc(string survivorId, string blocId)
        {
            if (_shelterGovernance == null) SetupShelterGovernance();
            if (_shelterGovernance == null) return false;
            return _shelterGovernance.AssignSurvivorToBloc(survivorId, blocId);
        }

        public string? GetSurvivorPoliticalBloc(string survivorId)
        {
            if (_shelterGovernance == null) SetupShelterGovernance();
            return _shelterGovernance?.GetSurvivorBloc(survivorId);
        }

        public string? GetSurvivorPoliticalBlocDisplayName(string survivorId)
        {
            if (_shelterGovernance == null) SetupShelterGovernance();
            return _shelterGovernance?.GetSurvivorBlocDisplayName(survivorId);
        }

        public ShelterGovernanceCensus GetShelterGovernanceCensus() =>
            _shelterGovernance?.Census ?? default;

        public int GetShelterStabilityRating() =>
            _shelterGovernance?.StabilityRating ?? 100;

        public void FlushShelterGovernanceIfDirty()
        {
            if (_shelterGovernanceDirty)
            {
                SaveShelterGovernance();
            }
        }

        public void ResetShelterGovernance()
        {
            _shelterGovernance = null;
            _shelterGovernanceDirty = false;
        }
    }
}
