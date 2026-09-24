// SPDX-License-Identifier: MIT
// ============================================================================
// Save Store : ShelterGovernanceSaveStore
// Core State : Ashfall.Core.Governance.ShelterGovernanceSaveState
// Host Caller: Main.ShelterGovernance (SetupShelterGovernance / SaveShelterGovernance)
// Purpose    : Plan 159 — Shelter governance & political system: ideological blocs,
//              policy consent, civil disputes, and shelter stability.
// ============================================================================

using System;
using System.Collections.Generic;
using Ashfall.Core.Governance;
using Ashfall.Core.Save;
using Ashfall.Core.Survivors;

namespace AtomicWar.GodotApp
{
    public static class ShelterGovernanceSaveStore
    {
        public const string FileName = "shelter_governance_save.json";
        public const string SectionName = "shelter_governance";

        private static readonly SaveStore<ShelterGovernanceSaveState> s_store =
            SaveStoreHub.Checksummed<ShelterGovernanceSaveState>(FileName, nameof(ShelterGovernanceSaveStore));

        public static string SavePath => s_store.SavePath;
        public static bool Exists => s_store.Exists();

        public static string TryCapturePersisted(ShelterGovernanceSaveState state) => s_store.CaptureBare(state);
        public static ShelterGovernanceSaveState? TryRestorePersisted(string json) => s_store.RestoreBare(json);
        public static bool TrySave(ShelterGovernanceSaveState state) => s_store.TrySave(state);
        public static ShelterGovernanceSaveState? TryLoad() => s_store.TryLoad();
    }

    /// <summary>
    /// Host session manager for Plan 159 (Shelter Governance & Political System).
    /// Binds ideological blocs from shelter_governance_blocs.json, evaluates policy consent
    /// against PolicySystem and LeadershipSystem, tracks civil disputes, and computes stability.
    /// </summary>
    public sealed class ShelterGovernanceHostSession : HostSessionBase
    {
        private readonly ShelterGovernanceEngine _engine;
        private readonly PolicySystem? _policySystem;
        private readonly LeadershipSystem? _leadershipSystem;
        private string _lastEvent = string.Empty;

        public ShelterGovernanceEngine Engine => _engine;
        public PolicySystem? PolicySystem => _policySystem;
        public LeadershipSystem? LeadershipSystem => _leadershipSystem;
        public string LastEvent => _lastEvent;
        public ShelterGovernanceCensus Census => _engine.GetCensus();
        public int StabilityRating => _engine.CalculateStabilityRating();
        public IReadOnlyDictionary<string, IdeologicalBlocDef> Definitions => _engine.Definitions;
        public IReadOnlyDictionary<string, BlocRuntimeState> Blocs => _engine.Blocs;
        public IReadOnlyList<DisputeCaseRecord> Disputes => _engine.Disputes;

        public ShelterGovernanceHostSession(
            string? dataDir = null,
            PolicySystem? policySystem = null,
            LeadershipSystem? leadershipSystem = null,
            ShelterGovernanceEngine? engine = null)
        {
            _policySystem = policySystem;
            _leadershipSystem = leadershipSystem;
            _engine = engine ?? new ShelterGovernanceEngine(policySystem, leadershipSystem);

            if (!string.IsNullOrEmpty(dataDir))
            {
                LoadCatalogs(dataDir);
            }

            _engine.OnGovernanceStateChanged += () => RaiseStateChanged();
            _engine.OnDisputeOpened += dispute =>
            {
                _lastEvent = $"Civil dispute {dispute.case_id} opened: {dispute.dispute_type} ({dispute.initiator_survivor_id} vs {dispute.defendant_survivor_id}).";
                RaiseStateChanged();
            };
            _engine.OnDisputeResolved += dispute =>
            {
                _lastEvent = $"Civil dispute {dispute.case_id} resolved via {dispute.applied_resolution}.";
                RaiseStateChanged();
            };
        }

        public static ShelterGovernanceHostSession Create(
            string? dataDir = null,
            PolicySystem? policySystem = null,
            LeadershipSystem? leadershipSystem = null,
            ShelterGovernanceEngine? engine = null)
        {
            return new ShelterGovernanceHostSession(dataDir, policySystem, leadershipSystem, engine);
        }

        public void LoadCatalogs(string dataDir)
        {
            if (string.IsNullOrEmpty(dataDir)) return;

            try
            {
                var result = ShelterGovernanceCatalogLoader.Load(dataDir);
                if (result.Success && result.Blocs.Count > 0)
                {
                    _engine.BindValidatedBlocs(result.Blocs);
                    _lastEvent = $"Loaded {result.Blocs.Count} ideological bloc definitions.";
                    RaiseStateChanged();
                }
                else if (result.Errors.Count > 0)
                {
                    _lastEvent = $"Failed to load governance catalog: {string.Join("; ", result.Errors)}";
                }
            }
            catch (Exception ex)
            {
                _lastEvent = $"Exception loading governance catalog: {ex.Message}";
            }
        }

        public bool AssignSurvivorToBloc(string survivorId, string blocId)
        {
            bool ok = _engine.AssignSurvivorToBloc(survivorId, blocId);
            if (ok)
            {
                _lastEvent = $"Survivor '{survivorId}' assigned to bloc '{blocId}'.";
                RaiseStateChanged();
            }
            return ok;
        }

        public string? GetSurvivorBloc(string survivorId) => _engine.GetSurvivorBloc(survivorId);

        public string? GetSurvivorBlocDisplayName(string survivorId) => _engine.GetSurvivorBlocDisplayName(survivorId);

        public PolicyConsentEvaluation EvaluatePolicyConsent(string scope, string optionId) =>
            _engine.EvaluatePolicyConsent(scope, optionId);

        public DisputeCaseRecord OpenDispute(
            string initiatorId,
            string defendantId,
            GovernanceDisputeType type,
            int day)
        {
            return _engine.OpenDispute(initiatorId, defendantId, type, day);
        }

        public bool ResolveDispute(string caseId, GovernanceDisputeResolution resolution, int day)
        {
            return _engine.ResolveDispute(caseId, resolution, day);
        }

        public void AdvanceDay(int day)
        {
            _engine.Tick(24f);
            _engine.RecalculateBlocWeights();
            _lastEvent = $"Day {day} governance simulation advanced. Stability: {_engine.CalculateStabilityRating()}%.";
            RaiseStateChanged();
        }

        public ShelterGovernanceSaveState CaptureState() => _engine.CaptureState();

        public void RestoreState(ShelterGovernanceSaveState? state)
        {
            _engine.RestoreState(state);
            RaiseStateChanged();
        }

        public void Reset()
        {
            _engine.RestoreState(new ShelterGovernanceSaveState());
            _lastEvent = string.Empty;
            RaiseStateChanged();
        }
    }
}
