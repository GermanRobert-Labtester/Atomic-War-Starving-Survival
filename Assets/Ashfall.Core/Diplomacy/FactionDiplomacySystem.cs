// SPDX-License-Identifier: MIT
// ============================================================================
// Plan 197 — Faction Diplomacy & Treaty System
// Pure domain authority for managing diplomatic relations, formal treaties,
// envoy missions, obligations, violations, and global diplomatic reputation.
// ============================================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace Ashfall.Core.Diplomacy
{
    // ── Catalog DTOs ────────────────────────────────────────────────────────

    [Serializable]
    public sealed class TreatyTermDef
    {
        public string term_id { get; set; } = string.Empty;
        public string term_type { get; set; } = string.Empty;
        public string description { get; set; } = string.Empty;
        public float value { get; set; } = 1.0f;
        public string condition { get; set; } = "mutual";
    }

    [Serializable]
    public sealed class TreatyTemplateDef
    {
        public string treaty_type_id { get; set; } = string.Empty;
        public string display_name { get; set; } = string.Empty;
        public string category { get; set; } = "peace";
        public int base_duration_days { get; set; } = 60;
        public int reputation_requirement { get; set; } = 20;
        public string relation_requirement { get; set; } = "neutral";
        public List<TreatyTermDef> terms { get; set; } = new List<TreatyTermDef>();
        public string description { get; set; } = string.Empty;
    }

    [Serializable]
    public sealed class TreatyTemplatesCatalog
    {
        public int schema_version { get; set; } = 1;
        public List<TreatyTemplateDef> templates { get; set; } = new List<TreatyTemplateDef>();
    }

    // ── State DTOs ──────────────────────────────────────────────────────────

    [Serializable]
    public sealed class DiplomaticRelationState
    {
        public string FactionId { get; set; } = string.Empty;
        public string RelationLevel { get; set; } = "neutral"; // hostile, unfriendly, neutral, friendly, allied
        public int Trust { get; set; } = 0;                     // -100 to +100
        public int DiplomaticReputation { get; set; } = 50;     // 0 to 100
        public int TreatyCount { get; set; } = 0;
        public int LastDiplomaticActionDay { get; set; } = 0;
        public string? EnvoyAssigned { get; set; }
        public string RelationTrend { get; set; } = "stable";   // improving, stable, declining
    }

    [Serializable]
    public sealed class ActiveTreatyRecord
    {
        public string TreatyId { get; set; } = string.Empty;
        public string TreatyTypeId { get; set; } = string.Empty;
        public string FactionA { get; set; } = "player_shelter";
        public string FactionB { get; set; } = string.Empty;
        public int SignedDay { get; set; } = 1;
        public int DurationDays { get; set; } = 60;
        public string Status { get; set; } = "active"; // active, expired, violated, renounced
        public List<TreatyTermDef> Terms { get; set; } = new List<TreatyTermDef>();
        public List<string> Signatories { get; set; } = new List<string>();
    }

    [Serializable]
    public sealed class DiplomaticMissionRecord
    {
        public string MissionId { get; set; } = string.Empty;
        public string MissionType { get; set; } = "negotiate_treaty";
        public string TargetFactionId { get; set; } = string.Empty;
        public string AssignedEnvoyId { get; set; } = string.Empty;
        public int MissionDay { get; set; } = 1;
        public int DurationDays { get; set; } = 3;
        public int SuccessChance { get; set; } = 50;
        public string Status { get; set; } = "in_progress"; // planned, en_route, in_progress, completed, failed
    }

    [Serializable]
    public sealed class FactionTreatyViolationRecord
    {
        public string ViolationId { get; set; } = string.Empty;
        public string TreatyId { get; set; } = string.Empty;
        public string ViolationType { get; set; } = string.Empty;
        public int ViolationDay { get; set; } = 1;
        public string Violator { get; set; } = "player";
        public List<string> Consequences { get; set; } = new List<string>();
        public int DiplomaticReputationLoss { get; set; } = 25;
    }

    [Serializable]
    public sealed class FactionDiplomacyState
    {
        public int SchemaVersion { get; set; } = 1;
        public int NextSequence { get; set; } = 1;
        public int GlobalReputation { get; set; } = 50;
        public List<DiplomaticRelationState> Relations { get; set; } = new List<DiplomaticRelationState>();
        public List<ActiveTreatyRecord> ActiveTreaties { get; set; } = new List<ActiveTreatyRecord>();
        public List<DiplomaticMissionRecord> Missions { get; set; } = new List<DiplomaticMissionRecord>();
        public List<FactionTreatyViolationRecord> Violations { get; set; } = new List<FactionTreatyViolationRecord>();
        public bool AutoRenewTreaties { get; set; } = false;
        public bool TreatyNotifications { get; set; } = true;
    }

    // ── Domain System ───────────────────────────────────────────────────────

    public sealed class FactionDiplomacySystem
    {
        private FactionDiplomacyState _state;
        private readonly Dictionary<string, TreatyTemplateDef> _templateDefs =
            new Dictionary<string, TreatyTemplateDef>(StringComparer.OrdinalIgnoreCase);

        public event Action<ActiveTreatyRecord>? OnTreatySigned;
        public event Action<ActiveTreatyRecord, FactionTreatyViolationRecord>? OnTreatyViolated;
        public event Action<ActiveTreatyRecord>? OnTreatyExpired;
        public event Action<ActiveTreatyRecord>? OnTreatyRenounced;
        public event Action<DiplomaticRelationState>? OnRelationChanged;
        public event Action<DiplomaticMissionRecord>? OnMissionCompleted;

        public int GlobalReputation => _state.GlobalReputation;
        public int ActiveTreatyCount => _state.ActiveTreaties.Count(t => t.Status == "active");
        public int TotalViolationCount => _state.Violations.Count;

        public FactionDiplomacySystem()
        {
            _state = new FactionDiplomacyState();
        }

        public void LoadCatalog(string json)
        {
            if (string.IsNullOrWhiteSpace(json))
                throw new ArgumentException("Catalog JSON cannot be null or empty", nameof(json));

            var catalog = JsonSerializer.Deserialize<TreatyTemplatesCatalog>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            if (catalog?.templates == null) return;

            _templateDefs.Clear();
            foreach (var template in catalog.templates)
            {
                if (!string.IsNullOrEmpty(template.treaty_type_id))
                {
                    _templateDefs[template.treaty_type_id] = template;
                }
            }
        }

        public IReadOnlyList<TreatyTemplateDef> GetAllTemplates() => _templateDefs.Values.ToList();

        public TreatyTemplateDef? GetTemplate(string treatyTypeId)
        {
            _templateDefs.TryGetValue(treatyTypeId, out var template);
            return template;
        }

        public DiplomaticRelationState GetOrCreateRelation(string factionId)
        {
            var rel = _state.Relations.FirstOrDefault(r => string.Equals(r.FactionId, factionId, StringComparison.OrdinalIgnoreCase));
            if (rel == null)
            {
                rel = new DiplomaticRelationState
                {
                    FactionId = factionId,
                    RelationLevel = "neutral",
                    Trust = 0,
                    DiplomaticReputation = _state.GlobalReputation,
                    TreatyCount = 0,
                    LastDiplomaticActionDay = 1,
                    RelationTrend = "stable"
                };
                _state.Relations.Add(rel);
            }
            return rel;
        }

        public DiplomaticRelationState SetRelation(string factionId, int trust, string? relationLevel = null)
        {
            var rel = GetOrCreateRelation(factionId);
            rel.Trust = Math.Clamp(trust, -100, 100);

            if (!string.IsNullOrEmpty(relationLevel))
            {
                rel.RelationLevel = relationLevel;
            }
            else
            {
                rel.RelationLevel = DeriveRelationLevel(rel.Trust);
            }

            OnRelationChanged?.Invoke(rel);
            return rel;
        }

        public void AssignEnvoy(string factionId, string survivorId)
        {
            var rel = GetOrCreateRelation(factionId);
            rel.EnvoyAssigned = survivorId;
            OnRelationChanged?.Invoke(rel);
        }

        public (bool Success, string Message, ActiveTreatyRecord? Treaty) ProposeTreaty(
            string factionId, string treatyTypeId, int day, int envoySkill = 50)
        {
            if (!_templateDefs.TryGetValue(treatyTypeId, out var template))
                return (false, $"Unknown treaty template '{treatyTypeId}'", null);

            var rel = GetOrCreateRelation(factionId);

            // Check if already active
            if (HasActiveTreaty(factionId, treatyTypeId))
                return (false, $"Treaty of type '{treatyTypeId}' is already active with faction '{factionId}'", null);

            // Check relation requirement
            int currentRank = GetRelationRank(rel.RelationLevel);
            int requiredRank = GetRelationRank(template.relation_requirement);
            if (currentRank < requiredRank)
            {
                return (false, $"Relation level '{rel.RelationLevel}' does not meet required '{template.relation_requirement}'", null);
            }

            // Check reputation requirement
            if (_state.GlobalReputation < template.reputation_requirement)
            {
                return (false, $"Diplomatic reputation '{_state.GlobalReputation}' below required '{template.reputation_requirement}'", null);
            }

            // Calculate proposal success
            float proposalScore = envoySkill * 0.4f + rel.Trust * 0.3f + _state.GlobalReputation * 0.3f;
            if (proposalScore < 20f)
            {
                return (false, "Faction rejected the diplomatic proposal.", null);
            }

            var record = new ActiveTreatyRecord
            {
                TreatyId = $"treaty_{_state.NextSequence++}",
                TreatyTypeId = template.treaty_type_id,
                FactionA = "player_shelter",
                FactionB = factionId,
                SignedDay = day,
                DurationDays = template.base_duration_days,
                Status = "active",
                Terms = template.terms.Select(t => new TreatyTermDef
                {
                    term_id = t.term_id,
                    term_type = t.term_type,
                    description = t.description,
                    value = t.value,
                    condition = t.condition
                }).ToList(),
                Signatories = new List<string> { "player_shelter", factionId }
            };

            if (!string.IsNullOrEmpty(rel.EnvoyAssigned))
            {
                record.Signatories.Add(rel.EnvoyAssigned);
            }

            _state.ActiveTreaties.Add(record);
            rel.TreatyCount = _state.ActiveTreaties.Count(t => t.Status == "active" && string.Equals(t.FactionB, factionId, StringComparison.OrdinalIgnoreCase));
            rel.LastDiplomaticActionDay = day;

            // Reputation reward
            _state.GlobalReputation = Math.Min(100, _state.GlobalReputation + 2);
            rel.Trust = Math.Min(100, rel.Trust + 5);
            rel.RelationLevel = DeriveRelationLevel(rel.Trust);

            OnTreatySigned?.Invoke(record);
            OnRelationChanged?.Invoke(rel);

            return (true, "Treaty successfully ratified.", record);
        }

        public DiplomaticMissionRecord DispatchMission(
            string missionType, string targetFactionId, string envoyId, int day, int durationDays = 3, int envoySkill = 50)
        {
            var rel = GetOrCreateRelation(targetFactionId);
            int successChance = Math.Clamp((int)(envoySkill * 0.5f + _state.GlobalReputation * 0.3f + rel.Trust * 0.2f), 10, 95);

            var mission = new DiplomaticMissionRecord
            {
                MissionId = $"mission_{_state.NextSequence++}",
                MissionType = missionType,
                TargetFactionId = targetFactionId,
                AssignedEnvoyId = envoyId,
                MissionDay = day,
                DurationDays = Math.Max(1, durationDays),
                SuccessChance = successChance,
                Status = "in_progress"
            };

            _state.Missions.Add(mission);
            return mission;
        }

        public void TickDay(int day)
        {
            // Process treaties
            foreach (var treaty in _state.ActiveTreaties)
            {
                if (treaty.Status == "active" && treaty.DurationDays > 0)
                {
                    if (day >= treaty.SignedDay + treaty.DurationDays)
                    {
                        treaty.Status = "expired";
                        var rel = GetOrCreateRelation(treaty.FactionB);
                        rel.TreatyCount = _state.ActiveTreaties.Count(t => t.Status == "active" && string.Equals(t.FactionB, treaty.FactionB, StringComparison.OrdinalIgnoreCase));
                        OnTreatyExpired?.Invoke(treaty);
                    }
                }
            }

            // Process missions
            foreach (var mission in _state.Missions)
            {
                if (mission.Status == "in_progress" && day >= mission.MissionDay + mission.DurationDays)
                {
                    bool isSuccess = mission.SuccessChance >= 40; // Deterministic threshold check
                    mission.Status = isSuccess ? "completed" : "failed";

                    var rel = GetOrCreateRelation(mission.TargetFactionId);
                    if (isSuccess)
                    {
                        _state.GlobalReputation = Math.Min(100, _state.GlobalReputation + 3);
                        rel.Trust = Math.Min(100, rel.Trust + 4);
                    }
                    else
                    {
                        _state.GlobalReputation = Math.Max(0, _state.GlobalReputation - 2);
                        rel.Trust = Math.Max(-100, rel.Trust - 2);
                    }
                    rel.RelationLevel = DeriveRelationLevel(rel.Trust);
                    OnMissionCompleted?.Invoke(mission);
                    OnRelationChanged?.Invoke(rel);
                }
            }
        }

        public bool ViolateTreaty(string treatyId, string reason, string violator = "player", int day = 1)
        {
            var treaty = _state.ActiveTreaties.FirstOrDefault(t => string.Equals(t.TreatyId, treatyId, StringComparison.OrdinalIgnoreCase));
            if (treaty == null || treaty.Status != "active")
                return false;

            treaty.Status = "violated";

            var violation = new FactionTreatyViolationRecord
            {
                ViolationId = $"violation_{_state.NextSequence++}",
                TreatyId = treatyId,
                ViolationType = reason,
                ViolationDay = day,
                Violator = violator,
                Consequences = new List<string> { "Reputation -25", "Faction Trust -50", "Hostile Stance" },
                DiplomaticReputationLoss = 25
            };

            _state.Violations.Add(violation);
            _state.GlobalReputation = Math.Max(0, _state.GlobalReputation - 25);

            var rel = GetOrCreateRelation(treaty.FactionB);
            rel.TreatyCount = _state.ActiveTreaties.Count(t => t.Status == "active" && string.Equals(t.FactionB, treaty.FactionB, StringComparison.OrdinalIgnoreCase));
            rel.Trust = Math.Max(-100, rel.Trust - 50);
            rel.RelationLevel = "hostile";
            rel.RelationTrend = "declining";

            OnTreatyViolated?.Invoke(treaty, violation);
            OnRelationChanged?.Invoke(rel);
            return true;
        }

        public bool RenounceTreaty(string treatyId, int day = 1)
        {
            var treaty = _state.ActiveTreaties.FirstOrDefault(t => string.Equals(t.TreatyId, treatyId, StringComparison.OrdinalIgnoreCase));
            if (treaty == null || treaty.Status != "active")
                return false;

            treaty.Status = "renounced";
            _state.GlobalReputation = Math.Max(0, _state.GlobalReputation - 10);

            var rel = GetOrCreateRelation(treaty.FactionB);
            rel.TreatyCount = _state.ActiveTreaties.Count(t => t.Status == "active" && string.Equals(t.FactionB, treaty.FactionB, StringComparison.OrdinalIgnoreCase));
            rel.Trust = Math.Max(-100, rel.Trust - 15);
            rel.RelationLevel = DeriveRelationLevel(rel.Trust);

            OnTreatyRenounced?.Invoke(treaty);
            OnRelationChanged?.Invoke(rel);
            return true;
        }

        public bool HasActiveTreaty(string factionId, string treatyTypeId)
        {
            return _state.ActiveTreaties.Any(t =>
                t.Status == "active" &&
                string.Equals(t.FactionB, factionId, StringComparison.OrdinalIgnoreCase) &&
                string.Equals(t.TreatyTypeId, treatyTypeId, StringComparison.OrdinalIgnoreCase));
        }

        public IReadOnlyList<ActiveTreatyRecord> GetActiveTreaties() =>
            _state.ActiveTreaties.Where(t => t.Status == "active").ToList();

        public IReadOnlyList<ActiveTreatyRecord> GetActiveTreatiesForFaction(string factionId) =>
            _state.ActiveTreaties.Where(t => t.Status == "active" && string.Equals(t.FactionB, factionId, StringComparison.OrdinalIgnoreCase)).ToList();

        public IReadOnlyList<FactionTreatyViolationRecord> GetViolations() => _state.Violations.ToList();

        public IReadOnlyList<DiplomaticMissionRecord> GetMissions() => _state.Missions.ToList();

        public FactionDiplomacyState CaptureState() => _state;

        public void RestoreState(FactionDiplomacyState? state)
        {
            _state = state ?? new FactionDiplomacyState();
        }

        private static string DeriveRelationLevel(int trust)
        {
            if (trust <= -50) return "hostile";
            if (trust <= -10) return "unfriendly";
            if (trust <= 10) return "neutral";
            if (trust <= 50) return "friendly";
            return "allied";
        }

        private static int GetRelationRank(string level)
        {
            return level.ToLowerInvariant() switch
            {
                "hostile" => 0,
                "unfriendly" => 1,
                "neutral" => 2,
                "friendly" => 3,
                "allied" => 4,
                _ => 2
            };
        }
    }
}
