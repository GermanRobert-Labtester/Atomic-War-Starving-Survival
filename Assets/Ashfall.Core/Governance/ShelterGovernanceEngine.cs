// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.Globalization;
using Ashfall.Core.Survivors;

namespace Ashfall.Core.Governance
{
    public enum GovernanceDisputeType
    {
        ResourceTheft = 0,
        Insult = 1,
        WorkRefusal = 2,
        IdeologicalSchism = 3,
        CurfewViolation = 4
    }

    public enum GovernanceDisputeResolution
    {
        Mediation = 0,
        Restitution = 1,
        OfficialReprimand = 2,
        Exile = 3
    }

    [Serializable]
    public sealed class IdeologicalBlocDef
    {
        public string bloc_id = string.Empty;
        public string display_name = string.Empty;
        public string description = string.Empty;
        public string core_ideology = string.Empty;
        public List<string> supported_policy_scopes = new List<string>();
        public List<string> opposed_policy_scopes = new List<string>();
        public int baseline_weight = 25;
        public int grievance_decay_rate = 5;
    }

    [Serializable]
    public sealed class ShelterGovernanceBlocsCatalogJson
    {
        public int schema_version = 1;
        public List<IdeologicalBlocDef> blocs = new List<IdeologicalBlocDef>();
    }

    [Serializable]
    public sealed class BlocRuntimeState
    {
        public string bloc_id = string.Empty;
        public int influence_weight_bp = 2500; // basis points (0 - 10000)
        public int grievance_bp = 0;          // basis points (0 - 10000, 0% - 100%)
        public List<string> member_survivor_ids = new List<string>();
    }

    [Serializable]
    public sealed class DisputeCaseRecord
    {
        public string case_id = string.Empty;
        public string initiator_survivor_id = string.Empty;
        public string defendant_survivor_id = string.Empty;
        public GovernanceDisputeType dispute_type;
        public int day_initiated;
        public bool is_resolved;
        public GovernanceDisputeResolution applied_resolution;
        public int resolution_day;
    }

    [Serializable]
    public sealed class PolicyConsentEvaluation
    {
        public string scope = string.Empty;
        public string option_id = string.Empty;
        public int net_consent_score_bp; // -10000 to +10000
        public bool has_majority_consent;
        public List<string> supporting_bloc_ids = new List<string>();
        public List<string> opposing_bloc_ids = new List<string>();
        public int projected_grievance_increase_bp;
    }

    [Serializable]
    public sealed class ShelterGovernanceSaveState
    {
        public int schema_version = 1;
        public int next_dispute_seq = 1;
        public Dictionary<string, BlocRuntimeState> blocs = new Dictionary<string, BlocRuntimeState>(StringComparer.OrdinalIgnoreCase);
        public List<DisputeCaseRecord> disputes = new List<DisputeCaseRecord>();
    }

    /// <summary>
    /// Read-only snapshot census for Plan 159 shelter governance diagnostics.
    /// </summary>
    public struct ShelterGovernanceCensus
    {
        public int TotalBlocs { get; }
        public int TotalMembers { get; }
        public int OpenDisputes { get; }
        public int ResolvedDisputes { get; }
        public int StabilityRating { get; }

        public ShelterGovernanceCensus(
            int totalBlocs,
            int totalMembers,
            int openDisputes,
            int resolvedDisputes,
            int stabilityRating)
        {
            TotalBlocs = totalBlocs;
            TotalMembers = totalMembers;
            OpenDisputes = openDisputes;
            ResolvedDisputes = resolvedDisputes;
            StabilityRating = stabilityRating;
        }
    }

    /// <summary>
    /// Plan 159 / C1[27] / DEC-107: Shelter Governance & Political System.
    /// Manages ideological blocs, policy consent evaluations, survivor grievance tracking,
    /// civil dispute resolution, and shelter stability scoring.
    /// Integrates cleanly with PolicySystem and LeadershipSystem.
    /// </summary>
    public sealed class ShelterGovernanceEngine
    {
        public const string SystemId = "shelter_governance";
        public const int BasisPointsMax = 10000;

        private readonly Dictionary<string, IdeologicalBlocDef> _defs =
            new Dictionary<string, IdeologicalBlocDef>(StringComparer.OrdinalIgnoreCase);

        private readonly ShelterGovernanceSaveState _state = new ShelterGovernanceSaveState();
        private readonly PolicySystem? _policySystem;
        private readonly LeadershipSystem? _leadershipSystem;

        public event Action<DisputeCaseRecord>? OnDisputeOpened;
        public event Action<DisputeCaseRecord>? OnDisputeResolved;
        public event Action<string, int>? OnBlocGrievanceChanged; // blocId, newGrievanceBp
        public event Action? OnGovernanceStateChanged;

        public IReadOnlyDictionary<string, IdeologicalBlocDef> Definitions => _defs;
        public IReadOnlyDictionary<string, BlocRuntimeState> Blocs => _state.blocs;
        public IReadOnlyList<DisputeCaseRecord> Disputes => _state.disputes;
        public PolicySystem? PolicySystem => _policySystem;
        public LeadershipSystem? LeadershipSystem => _leadershipSystem;

        public ShelterGovernanceEngine(
            PolicySystem? policySystem = null,
            LeadershipSystem? leadershipSystem = null,
            string? catalogJson = null,
            IJsonSerializer? serializer = null)
        {
            _policySystem = policySystem;
            _leadershipSystem = leadershipSystem;

            if (!string.IsNullOrWhiteSpace(catalogJson) && serializer != null)
            {
                LoadCatalog(catalogJson, serializer);
            }
            else
            {
                RegisterDefaultBlocs();
            }

            if (_policySystem != null)
            {
                _policySystem.OnPolicyChanged += HandlePolicyChanged;
            }
        }

        public void LoadCatalog(string json, IJsonSerializer serializer)
        {
            if (string.IsNullOrWhiteSpace(json) || serializer == null) return;
            try
            {
                var cat = serializer.Deserialize<ShelterGovernanceBlocsCatalogJson>(json);
                if (cat?.blocs != null)
                {
                    foreach (var b in cat.blocs)
                    {
                        RegisterBlocDefinition(b);
                    }
                }
            }
            catch
            {
                // Fallback to defaults if catalog parse fails
                RegisterDefaultBlocs();
            }
        }

        public void BindValidatedBlocs(IEnumerable<IdeologicalBlocDef> blocs)
        {
            if (blocs == null) return;
            foreach (var b in blocs)
            {
                RegisterBlocDefinition(b);
            }
        }

        public void RegisterBlocDefinition(IdeologicalBlocDef def)
        {
            if (def == null || string.IsNullOrEmpty(def.bloc_id)) return;
            _defs[def.bloc_id] = def;
            EnsureBlocRuntimeState(def);
        }

        private void RegisterDefaultBlocs()
        {
            RegisterBlocDefinition(new IdeologicalBlocDef
            {
                bloc_id = "bloc_security_first",
                display_name = "Security & Order Vanguard",
                description = "Hardened survivors prioritizing fortification, strict curfews, and physical survival.",
                core_ideology = "Authoritarian",
                supported_policy_scopes = new List<string> { "curfew", "defense", "ration_triage" },
                opposed_policy_scopes = new List<string> { "open_admission", "luxury_distribution" },
                baseline_weight = 25,
                grievance_decay_rate = 5
            });

            RegisterBlocDefinition(new IdeologicalBlocDef
            {
                bloc_id = "bloc_egalitarian_commons",
                display_name = "Communal Equality Union",
                description = "Advocates of shared burdens, equal ration portions, and consensual council governance.",
                core_ideology = "Collectivist",
                supported_policy_scopes = new List<string> { "equal_rations", "open_council", "shared_work" },
                opposed_policy_scopes = new List<string> { "ration_triage", "leader_only", "privilege_allotment" },
                baseline_weight = 30,
                grievance_decay_rate = 8
            });

            RegisterBlocDefinition(new IdeologicalBlocDef
            {
                bloc_id = "bloc_free_pioneers",
                display_name = "Frontier Freeholders",
                description = "Scavengers and tradesfolk demanding autonomy, minimal regulation, and personal possession rights.",
                core_ideology = "Libertarian",
                supported_policy_scopes = new List<string> { "trade_freedom", "curfew_disabled", "private_keepsakes" },
                opposed_policy_scopes = new List<string> { "curfew", "work_conscription", "resource_confiscation" },
                baseline_weight = 20,
                grievance_decay_rate = 6
            });

            RegisterBlocDefinition(new IdeologicalBlocDef
            {
                bloc_id = "bloc_heritage_archive",
                display_name = "Old World Preservationists",
                description = "Scholars, elders, and archivists committed to historical knowledge and medical ethics.",
                core_ideology = "Traditionalist",
                supported_policy_scopes = new List<string> { "education", "medical_priority", "cultural_preservation" },
                opposed_policy_scopes = new List<string> { "scrap_books", "forced_labor", "irradiated_supplement" },
                baseline_weight = 25,
                grievance_decay_rate = 7
            });
        }

        private BlocRuntimeState EnsureBlocRuntimeState(IdeologicalBlocDef def)
        {
            if (!_state.blocs.TryGetValue(def.bloc_id, out var runtime))
            {
                runtime = new BlocRuntimeState
                {
                    bloc_id = def.bloc_id,
                    influence_weight_bp = def.baseline_weight * 100,
                    grievance_bp = 0,
                    member_survivor_ids = new List<string>()
                };
                _state.blocs[def.bloc_id] = runtime;
            }
            return runtime;
        }

        // ── Survivor Affiliations ─────────────────────────────────

        public bool AssignSurvivorToBloc(string survivorId, string blocId)
        {
            if (string.IsNullOrEmpty(survivorId) || string.IsNullOrEmpty(blocId)) return false;
            if (!_defs.ContainsKey(blocId) || !_state.blocs.ContainsKey(blocId)) return false;

            // Remove from current bloc
            foreach (var b in _state.blocs.Values)
            {
                b.member_survivor_ids.Remove(survivorId);
            }

            _state.blocs[blocId].member_survivor_ids.Add(survivorId);
            RecalculateBlocWeights();
            OnGovernanceStateChanged?.Invoke();
            return true;
        }

        public string? GetSurvivorBloc(string survivorId)
        {
            if (string.IsNullOrEmpty(survivorId)) return null;
            foreach (var kvp in _state.blocs)
            {
                if (kvp.Value.member_survivor_ids.Contains(survivorId))
                    return kvp.Key;
            }
            return null;
        }

        public string? GetSurvivorBlocDisplayName(string survivorId)
        {
            string? blocId = GetSurvivorBloc(survivorId);
            if (blocId != null && _defs.TryGetValue(blocId, out var def) && !string.IsNullOrEmpty(def.display_name))
            {
                return def.display_name;
            }
            return blocId;
        }

        public void RecalculateBlocWeights()
        {
            int totalMembers = 0;
            foreach (var b in _state.blocs.Values)
            {
                totalMembers += b.member_survivor_ids.Count;
            }

            if (totalMembers == 0)
            {
                // Fallback to baseline weights
                int sumBaseline = 0;
                foreach (var def in _defs.Values) sumBaseline += def.baseline_weight;
                if (sumBaseline <= 0) sumBaseline = 1;

                foreach (var def in _defs.Values)
                {
                    if (_state.blocs.TryGetValue(def.bloc_id, out var state))
                    {
                        state.influence_weight_bp = (def.baseline_weight * BasisPointsMax) / sumBaseline;
                    }
                }
                return;
            }

            // Distribute 60% based on membership, 40% based on baseline weight
            int sumBase = 0;
            foreach (var def in _defs.Values) sumBase += def.baseline_weight;
            if (sumBase <= 0) sumBase = 1;

            int allocatedBp = 0;
            var list = new List<BlocRuntimeState>(_state.blocs.Values);
            for (int i = 0; i < list.Count; i++)
            {
                var state = list[i];
                var def = _defs.TryGetValue(state.bloc_id, out var d) ? d : null;
                int basePart = def != null ? (def.baseline_weight * 4000) / sumBase : 1000;
                int memberPart = (state.member_survivor_ids.Count * 6000) / totalMembers;
                int weight = basePart + memberPart;

                if (i == list.Count - 1)
                {
                    weight = BasisPointsMax - allocatedBp;
                }
                else
                {
                    allocatedBp += weight;
                }

                state.influence_weight_bp = weight;
            }
        }

        // ── Policy Consent Evaluation ─────────────────────────────

        public PolicyConsentEvaluation EvaluatePolicyConsent(string scope, string optionId)
        {
            var eval = new PolicyConsentEvaluation
            {
                scope = scope ?? string.Empty,
                option_id = optionId ?? string.Empty
            };

            int netScore = 0;
            int totalWeight = 0;

            foreach (var kvp in _state.blocs)
            {
                string blocId = kvp.Key;
                var runtime = kvp.Value;
                if (!_defs.TryGetValue(blocId, out var def)) continue;

                totalWeight += runtime.influence_weight_bp;

                bool supports = def.supported_policy_scopes.Contains(scope);
                bool opposes = def.opposed_policy_scopes.Contains(scope);

                if (supports && !opposes)
                {
                    eval.supporting_bloc_ids.Add(blocId);
                    netScore += runtime.influence_weight_bp;
                }
                else if (opposes && !supports)
                {
                    eval.opposing_bloc_ids.Add(blocId);
                    netScore -= runtime.influence_weight_bp;
                }
            }

            eval.net_consent_score_bp = totalWeight > 0 ? (netScore * BasisPointsMax) / totalWeight : 0;
            eval.has_majority_consent = eval.net_consent_score_bp >= 0;
            eval.projected_grievance_increase_bp = eval.opposing_bloc_ids.Count > 0 ? 1500 : 0;

            return eval;
        }

        private void HandlePolicyChanged(string scope, string optionId, string proposerId, int day)
        {
            ApplyPolicyEffects(scope, optionId);
        }

        public void RecordPolicyEnactmentGrievance(string scope, string optionId) =>
            ApplyPolicyEffects(scope, optionId);

        public void ApplyPolicyEffects(string scope, string optionId)
        {
            var eval = EvaluatePolicyConsent(scope, optionId);

            // Opposing blocs increase grievance (+1500 bp)
            foreach (var oppId in eval.opposing_bloc_ids)
            {
                AdjustBlocGrievance(oppId, 1500);
            }

            // Supporting blocs decrease grievance (-800 bp)
            foreach (var supId in eval.supporting_bloc_ids)
            {
                AdjustBlocGrievance(supId, -800);
            }
        }

        public void AdjustBlocGrievance(string blocId, int deltaBp)
        {
            if (!_state.blocs.TryGetValue(blocId, out var runtime)) return;

            int prev = runtime.grievance_bp;
            runtime.grievance_bp = Math.Max(0, Math.Min(BasisPointsMax, runtime.grievance_bp + deltaBp));

            if (prev != runtime.grievance_bp)
            {
                OnBlocGrievanceChanged?.Invoke(blocId, runtime.grievance_bp);
                OnGovernanceStateChanged?.Invoke();
            }
        }

        // ── Civil Disputes ────────────────────────────────────────

        public DisputeCaseRecord OpenDispute(
            string initiatorId,
            string defendantId,
            GovernanceDisputeType type,
            int day)
        {
            var record = new DisputeCaseRecord
            {
                case_id = $"disp_{_state.next_dispute_seq++}",
                initiator_survivor_id = initiatorId ?? string.Empty,
                defendant_survivor_id = defendantId ?? string.Empty,
                dispute_type = type,
                day_initiated = day,
                is_resolved = false,
                applied_resolution = GovernanceDisputeResolution.Mediation,
                resolution_day = 0
            };

            _state.disputes.Add(record);

            // If initiator and defendant belong to different blocs, friction occurs
            string? initBloc = GetSurvivorBloc(initiatorId);
            string? defBloc = GetSurvivorBloc(defendantId);
            if (!string.IsNullOrEmpty(initBloc) && !string.IsNullOrEmpty(defBloc) && initBloc != defBloc)
            {
                AdjustBlocGrievance(initBloc, 400);
                AdjustBlocGrievance(defBloc, 400);
            }

            OnDisputeOpened?.Invoke(record);
            OnGovernanceStateChanged?.Invoke();
            return record;
        }

        public bool ResolveDispute(string caseId, GovernanceDisputeResolution resolution, int day)
        {
            var record = _state.disputes.Find(d => d.case_id == caseId);
            if (record == null || record.is_resolved) return false;

            record.is_resolved = true;
            record.applied_resolution = resolution;
            record.resolution_day = day;

            string? initBloc = GetSurvivorBloc(record.initiator_survivor_id);
            string? defBloc = GetSurvivorBloc(record.defendant_survivor_id);

            switch (resolution)
            {
                case GovernanceDisputeResolution.Mediation:
                    // Both parties find common ground: mild grievance reduction
                    if (initBloc != null) AdjustBlocGrievance(initBloc, -500);
                    if (defBloc != null) AdjustBlocGrievance(defBloc, -500);
                    break;

                case GovernanceDisputeResolution.Restitution:
                    // Defendant pays or makes amends: satisfies initiator, slight annoyance for defendant
                    if (initBloc != null) AdjustBlocGrievance(initBloc, -800);
                    if (defBloc != null) AdjustBlocGrievance(defBloc, 300);
                    break;

                case GovernanceDisputeResolution.OfficialReprimand:
                    // Security/Authority pleased, defendant bloc aggrieved
                    AdjustBlocGrievance("bloc_security_first", -500);
                    if (defBloc != null) AdjustBlocGrievance(defBloc, 1000);
                    break;

                case GovernanceDisputeResolution.Exile:
                    // Severe punishment: satisfies order, angers freeholders and defendant bloc
                    AdjustBlocGrievance("bloc_security_first", -800);
                    AdjustBlocGrievance("bloc_free_pioneers", 1200);
                    if (defBloc != null) AdjustBlocGrievance(defBloc, 2000);
                    break;
            }

            OnDisputeResolved?.Invoke(record);
            OnGovernanceStateChanged?.Invoke();
            return true;
        }

        // ── Shelter Stability Calculation ─────────────────────────

        public int CalculateStabilityRating()
        {
            int stability = 100;

            // 1. Penalty from average weighted bloc grievances (up to -40 pts)
            int weightedGrievanceBp = 0;
            int totalWeightBp = 0;
            foreach (var b in _state.blocs.Values)
            {
                weightedGrievanceBp += (b.grievance_bp * b.influence_weight_bp) / BasisPointsMax;
                totalWeightBp += b.influence_weight_bp;
            }

            if (totalWeightBp > 0)
            {
                int avgGrievanceBp = (weightedGrievanceBp * BasisPointsMax) / totalWeightBp;
                int grievancePenalty = (avgGrievanceBp * 40) / BasisPointsMax;
                stability -= grievancePenalty;
            }

            // 2. Penalty from unresolved disputes (5 pts per open case, max -25 pts)
            int openDisputes = 0;
            for (int i = 0; i < _state.disputes.Count; i++)
            {
                if (!_state.disputes[i].is_resolved) openDisputes++;
            }
            stability -= Math.Min(25, openDisputes * 5);

            // 3. Leadership modifier
            if (_leadershipSystem != null)
            {
                string leader = _leadershipSystem.CurrentLeaderId;
                if (string.IsNullOrEmpty(leader))
                {
                    // Leaderless: lack of centralized order
                    stability -= 15;
                }
                else
                {
                    float stress = _leadershipSystem.GetLeaderStress(leader);
                    if (stress >= 70f)
                    {
                        stability -= 10;
                    }
                    else if (stress < 30f)
                    {
                        stability += 5;
                    }

                    // Active leadership challenges indicate contested power
                    bool hasActiveChallenge = false;
                    for (int i = 0; i < _leadershipSystem.Challenges.Count; i++)
                    {
                        if (!_leadershipSystem.Challenges[i].is_resolved)
                        {
                            hasActiveChallenge = true;
                            break;
                        }
                    }
                    if (hasActiveChallenge)
                    {
                        stability -= 10;
                    }
                }
            }

            return Math.Max(0, Math.Min(100, stability));
        }

        public ShelterGovernanceCensus GetCensus()
        {
            int totalMembers = 0;
            foreach (var b in _state.blocs.Values)
            {
                totalMembers += b.member_survivor_ids?.Count ?? 0;
            }

            int openDisputes = 0;
            int resolvedDisputes = 0;
            for (int i = 0; i < _state.disputes.Count; i++)
            {
                if (_state.disputes[i].is_resolved) resolvedDisputes++;
                else openDisputes++;
            }

            int stability = CalculateStabilityRating();

            return new ShelterGovernanceCensus(
                _defs.Count,
                totalMembers,
                openDisputes,
                resolvedDisputes,
                stability);
        }

        // ── Time Step & Simulation ────────────────────────────────

        public void Tick(float gameHours)
        {
            if (gameHours <= 0f) return;

            float dayFraction = gameHours / 24f;

            foreach (var kvp in _state.blocs)
            {
                var runtime = kvp.Value;
                if (runtime.grievance_bp <= 0) continue;

                int decayRate = 5;
                if (_defs.TryGetValue(kvp.Key, out var def))
                {
                    decayRate = def.grievance_decay_rate;
                }

                // Decay = decayRate% per day (decayRate * 100 bp * dayFraction)
                int decayAmount = (int)(decayRate * 100f * dayFraction);
                if (decayAmount > 0)
                {
                    AdjustBlocGrievance(kvp.Key, -decayAmount);
                }
            }
        }

        // ── Save / Load ───────────────────────────────────────────

        public ShelterGovernanceSaveState CaptureState()
        {
            var save = new ShelterGovernanceSaveState
            {
                schema_version = _state.schema_version,
                next_dispute_seq = _state.next_dispute_seq,
                blocs = new Dictionary<string, BlocRuntimeState>(StringComparer.OrdinalIgnoreCase),
                disputes = new List<DisputeCaseRecord>()
            };

            foreach (var kvp in _state.blocs)
            {
                save.blocs[kvp.Key] = new BlocRuntimeState
                {
                    bloc_id = kvp.Value.bloc_id,
                    influence_weight_bp = kvp.Value.influence_weight_bp,
                    grievance_bp = kvp.Value.grievance_bp,
                    member_survivor_ids = new List<string>(kvp.Value.member_survivor_ids)
                };
            }

            for (int i = 0; i < _state.disputes.Count; i++)
            {
                var d = _state.disputes[i];
                save.disputes.Add(new DisputeCaseRecord
                {
                    case_id = d.case_id,
                    initiator_survivor_id = d.initiator_survivor_id,
                    defendant_survivor_id = d.defendant_survivor_id,
                    dispute_type = d.dispute_type,
                    day_initiated = d.day_initiated,
                    is_resolved = d.is_resolved,
                    applied_resolution = d.applied_resolution,
                    resolution_day = d.resolution_day
                });
            }

            return save;
        }

        public void RestoreState(ShelterGovernanceSaveState? save)
        {
            _state.blocs.Clear();
            _state.disputes.Clear();
            _state.next_dispute_seq = 1;

            if (save == null) return;

            _state.schema_version = save.schema_version;
            _state.next_dispute_seq = save.next_dispute_seq;

            if (save.blocs != null)
            {
                foreach (var kvp in save.blocs)
                {
                    _state.blocs[kvp.Key] = new BlocRuntimeState
                    {
                        bloc_id = kvp.Value.bloc_id,
                        influence_weight_bp = kvp.Value.influence_weight_bp,
                        grievance_bp = kvp.Value.grievance_bp,
                        member_survivor_ids = new List<string>(kvp.Value.member_survivor_ids ?? new List<string>())
                    };
                }
            }

            if (save.disputes != null)
            {
                for (int i = 0; i < save.disputes.Count; i++)
                {
                    var d = save.disputes[i];
                    _state.disputes.Add(new DisputeCaseRecord
                    {
                        case_id = d.case_id,
                        initiator_survivor_id = d.initiator_survivor_id,
                        defendant_survivor_id = d.defendant_survivor_id,
                        dispute_type = d.dispute_type,
                        day_initiated = d.day_initiated,
                        is_resolved = d.is_resolved,
                        applied_resolution = d.applied_resolution,
                        resolution_day = d.resolution_day
                    });
                }
            }

            OnGovernanceStateChanged?.Invoke();
        }
    }
}
